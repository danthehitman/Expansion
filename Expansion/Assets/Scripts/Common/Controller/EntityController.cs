using Assets.Scripts.Common.Model;
using Assets.Scripts.Common.View;
using Assets.Scripts.Common.WorldInteraction;
using Assets.Scripts.World.Controller;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Controller
{
    public class EntityController<ColliderType> : ILifecycleEventAware where ColliderType : Collider2D
    {
        private enum JumpState
        {
            None = 0, Holding,
        }

        private InputController inputController;

        private float gravity = 32;
        private Vector2 velocity;
        private float horizAccel = 4;
        private float horizDecel = 12;
        private float horizStartAccel = 4.5f;
        private float horizMaxSpeed = 8;

        private GameObject gameObject;
        private EntityModel playerModel;
        private SideEntityView playerView;

        private LayerMask groundMask;
        private float parallelInsetLen = 0.2f;
        private float perpendicularInsetLen = 0.2f;
        private float touchDownTestLength = 0.0f;

        private Raycaster raycasterDown;
        private Raycaster raycasterLeft;
        private Raycaster raycasterRight;
        private Raycaster raycasterUp;

        private float jumpStartSpeed = 16;
        private float jumpMinSpeed = 8;
        private float jumpInputLeewayPeriod = 0.25f;
        private float jumpStartTimer;
        private float jumpHoldTimer;
        private bool jumpInputHeld;
        private float jumpMaxHoldPeriod = 0.25f;
        private JumpState jumpState;

        private GameObject blockOn = null;

        protected List<ILifecycleEventAware> lifecycleEventAwares = new List<ILifecycleEventAware>();


        public EntityController(Transform parent, InputController inputController, LayerMask? groundMask = null, EntityModel playerModel = null)
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
                playerModel = new EntityModel();
            this.playerModel = playerModel;
            playerView = new SideEntityView(gameObject);
            lifecycleEventAwares.Add(playerView);
        }

        public Action<Collider2D> CollidedWithBlock;

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

            //touchDown = new Raycaster(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), Vector2.down, groundMask,
            //Vector2.right * parallelInsetLen, Vector2.up * perpendicularInsetLen, touchDownTestLength);

            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        public void Update()
        {
            foreach (var aware in lifecycleEventAwares) aware.Update();

            jumpStartTimer -= Time.deltaTime;
            bool jumpButtonDownThisFrame = inputController.PlayerControls.World.Jump.IsPressed();
            if (jumpButtonDownThisFrame && !jumpInputHeld)
            {
                jumpStartTimer = jumpInputLeewayPeriod;
            }

            jumpInputHeld = jumpButtonDownThisFrame;
        }

        public void FixedUpdate()
        {
            var downCollider = raycasterDown.CastForHit(gameObject.transform.position);
            if (downCollider)
            {
                if (downCollider.gameObject != blockOn)
                {
                    if (CollidedWithBlock != null)
                        CollidedWithBlock(downCollider);
                    blockOn = downCollider.gameObject;
                    Debug.Log($"Standing on new block at X:{blockOn.transform.position.x} Y:{blockOn.transform.position.y}");
                }
            }
            else
            {
                blockOn = null;
            }

            switch (jumpState)
            {
                case JumpState.None:
                    if (downCollider && jumpStartTimer >= 0)
                    {
                        jumpStartTimer = 0;
                        jumpState = JumpState.Holding;
                        jumpHoldTimer = 0;
                        velocity.y = jumpStartSpeed;
                    }
                    break;
                case JumpState.Holding:
                    jumpHoldTimer += Time.deltaTime;
                    if (!jumpInputHeld || jumpHoldTimer >= jumpMaxHoldPeriod)
                    {
                        jumpState = JumpState.None;
                        velocity.y = Mathf.Lerp(jumpMinSpeed, jumpStartSpeed, jumpHoldTimer / jumpMaxHoldPeriod);

                        //Lerp function = 
                        //float p = jumpHoldTimer / jumpMaxHoldPeriod;
                        //veclocity.y = jumpMinSpeed + (jumpStartSpeed - jumpMinSpeed) * p;
                    }
                    break;
            }


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

            if (jumpState == JumpState.None)
            {
                velocity.y -= gravity * Time.deltaTime;
            }

            var displacement = Vector2.zero;
            Vector2 wantedDisplacement = velocity * Time.deltaTime;

            if (velocity.x > 0)
            {
                displacement.x = raycasterRight.CastForDistance(gameObject.transform.position, wantedDisplacement.x);
            }
            else if (velocity.x < 0)
            {
                displacement.x = -raycasterLeft.CastForDistance(gameObject.transform.position, -wantedDisplacement.x);
            }
            if (velocity.y > 0)
            {
                displacement.y = raycasterUp.CastForDistance(gameObject.transform.position, wantedDisplacement.y);
            }
            else if (velocity.y < 0)
            {
                displacement.y = -raycasterDown.CastForDistance(gameObject.transform.position, -wantedDisplacement.y);
            }

            if (Mathf.Approximately(displacement.x, wantedDisplacement.x) == false)
            {
                velocity.x = 0;
            }
            if (Mathf.Approximately(displacement.y, wantedDisplacement.y) == false)
            {
                velocity.y = 0;
            }

            gameObject.transform.Translate(displacement);

            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }
    }
}
