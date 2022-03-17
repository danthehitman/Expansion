using Assets.Scripts.Common.Model;
using Assets.Scripts.Common.View;
using Assets.Scripts.Common.WorldInteraction;
using Assets.Scripts.World;
using Assets.Scripts.World.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Controller
{
    public class EntityController<ColliderType> : ILifecycleEventAware where ColliderType : Collider2D
    {
        private float gravity = 8;
        private Vector2 velocity;
        private float horizAccel = 8;
        private float horizDecel = 12;
        private float horizStartAccel = 1.5f;
        private float horizMaxSpeed = 3;

        private GameObject gameObject;
        private PlayerModel playerModel;
        private SidePlayerView playerView;

        private LayerMask groundMask;
        private float parallelInsetLen = 0.2f;
        private float perpendicularInsetLen = 0.2f;
        private float touchDownTestLength = 0.0f;

        private Raycaster raycasterDown;
        private Raycaster raycasterLeft;
        private Raycaster raycasterRight;
        private Raycaster raycasterUp;
        private Raycaster touchDown;

        private InputController inputController;

        protected List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();


        public EntityController(Transform parent, InputController inputController, LayerMask? groundMask = null, PlayerModel playerModel = null)
        {
            this.inputController = inputController;

            if (groundMask == null)
                this.groundMask = LayerMask.GetMask(LayerMask.LayerToName(Constants.GROUND_LAYER_ID));

            gameObject = new GameObject();
            gameObject.transform.parent = parent;
            var entityRb = gameObject.AddComponent<Rigidbody2D>();
            entityRb.isKinematic = true;
            //TODO: Need a way to specify the collider that should be used and then the type of raycast in the Raycaster class.
            var playerCollider = gameObject.AddComponent<ColliderType>();

            if (playerModel == null)
                playerModel = new PlayerModel();
            this.playerModel = playerModel;
            playerView = new SidePlayerView(gameObject, this.playerModel);
            lifecycleEventAwares.Add(playerView);
        }

        public void Awake()
        {
            foreach (var aware in lifecycleEventAwares) aware.Awake();
        }

        public void OnDestroy()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDestroy();
        }

        public void OnDisable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDisable();
        }

        public void OnEnable()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnEnable();
        }

        public void Start()
        {
            raycasterDown = new Raycaster(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), Vector2.down, groundMask,
            Vector2.right * parallelInsetLen, Vector2.up * perpendicularInsetLen);

            raycasterLeft = new Raycaster(new Vector2(-0.5f, -0.5f), new Vector2(-0.5f, 0.5f), Vector2.left, groundMask,
            Vector2.up * parallelInsetLen, Vector2.right * perpendicularInsetLen);

            raycasterRight = new Raycaster(new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f), Vector2.right, groundMask,
            Vector2.up * parallelInsetLen, Vector2.left * perpendicularInsetLen);

            raycasterUp = new Raycaster(new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.up, groundMask,
            Vector2.right * parallelInsetLen, Vector2.down * perpendicularInsetLen);

            touchDown = new Raycaster(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), Vector2.down, groundMask,
            Vector2.right * parallelInsetLen, Vector2.up * perpendicularInsetLen, touchDownTestLength);

            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        public void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();
        }

        public void FixedUpdate()
        {
            float horizInput = inputController.PlayerControls.World.Move.ReadValue<Vector2>().x;
            int inputDiretion = MathUtils.GetSign(horizInput);
            int velocityDirection = MathUtils.GetSign(velocity.x);

            if (inputDiretion != 0)
            {
                if (inputDiretion != velocityDirection)
                {
                    velocity.x = horizStartAccel * inputDiretion;
                }
                else
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, horizMaxSpeed * inputDiretion, horizAccel * Time.deltaTime);
                }
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, horizDecel * Time.deltaTime);
            }

            //velocity.x += horizInput * horizAccel * Time.deltaTime;
            //if (Mathf.Abs(velocity.x) > horizMaxSpeed)
            //{
            //    velocity.x = horizMaxSpeed * Mathf.Sign(horizInput);
            //}
            //This is the same as above
            //if (!Mathf.Approximately(horizInput, 0f))
            //{
            //    velocity.x = Mathf.MoveTowards(velocity.x, horizMaxSpeed * Mathf.Sign(horizInput), horizAccel * Time.deltaTime);
            //}
            //else
            //{
            //    velocity.x = Mathf.MoveTowards(velocity.x, 0, horizDecel * Time.deltaTime);
            //}

            //Can we just use the normal casters and check the distance instead of doing the hit thing?  I think we can.
            if (!touchDown.CastForHit(gameObject.transform.position))
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }

            var displacement = Vector2.zero;
            if (velocity.x > 0)
            {
                displacement.x = raycasterRight.CastForDistance(gameObject.transform.position, velocity.x * Time.deltaTime);
            }
            else if (velocity.x < 0)
            {
                displacement.x = -raycasterLeft.CastForDistance(gameObject.transform.position, -velocity.x * Time.deltaTime);
            }
            if (velocity.y > 0)
            {
                displacement.y = raycasterUp.CastForDistance(gameObject.transform.position, velocity.y * Time.deltaTime);
            }
            else if (velocity.y < 0)
            {
                displacement.y = -raycasterDown.CastForDistance(gameObject.transform.position, -velocity.y * Time.deltaTime);
            }

            gameObject.transform.Translate(displacement);

            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }
    }
}
