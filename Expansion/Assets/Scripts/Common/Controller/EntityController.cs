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
        public Action<int> HorizontalDirectionChanged;
        public Action<Collider2D> CollidedWithBlock;

        private enum JumpState
        {
            None = 0, Holding,
        }

        private InputController inputController;
        private BoxCollider2D playerCollider;

        private float gravity = 24;

        private Vector2 velocity;
        private int velocityDirection;

        private float horizAccel = 1;
        private float horizDecel = 10;
        private float horizStartAccel = 1.5f;
        private float horizMaxSpeed = 4;

        private GameObject gameObject;
        private EntityModel playerModel;
        private SideEntityView playerView;

        private LayerMask groundMask;
        private float parallelInsetLen = 0.2f;
        private float perpendicularInsetLen = 0.2f;

        private Raycaster raycasterDown;
        private Raycaster raycasterLeft;
        private Raycaster raycasterRight;
        private Raycaster raycasterUp;

        private float jumpStartSpeed = 3;
        private float jumpMinSpeed = 2;
        private float jumpInputLeewayPeriod = 0.25f;
        private float jumpStartTimer;
        private float jumpHoldTimer;
        private bool jumpInputHeld;
        private float jumpMaxHoldPeriod = 0.15f;
        private JumpState jumpState;

        private bool isClimbing = false;
        private float climbPercentage = 0;
        private int climbDirection = -1;
        private Vector3? climbStartPosition = null;

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
            playerCollider = gameObject.AddComponent<BoxCollider2D>();
            playerCollider.size = new Vector2(.5f, 1);

            if (playerModel == null)
                playerModel = new EntityModel();
            this.playerModel = playerModel;
            playerView = new SideEntityView(gameObject);
            HorizontalDirectionChanged += playerView.HorizontalDirectionChanged;
            lifecycleEventAwares.Add(playerView);
        }

        public void Awake()
        {
            foreach (var aware in lifecycleEventAwares) aware.Awake();
        }

        public void OnDestroy()
        {
            foreach (var aware in lifecycleEventAwares) aware.OnDestroy();
            HorizontalDirectionChanged -= playerView.HorizontalDirectionChanged;
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
            //TODO: Need to adjust the ray positions based on the size of the entity collider.
            //The first number in the start and end vectors is how far the ray is from the center and the second is how long the ray is.
            raycasterDown = new Raycaster(new Vector2(-0.3f, -0.5f), new Vector2(0.3f, -0.5f), Vector2.down, groundMask,
            Vector2.right * parallelInsetLen, Vector2.up * perpendicularInsetLen);

            raycasterLeft = new Raycaster(new Vector2(-0.3f, -0.5f), new Vector2(-0.3f, 0.3f), Vector2.left, groundMask,
            Vector2.up * parallelInsetLen, Vector2.right * perpendicularInsetLen);

            raycasterRight = new Raycaster(new Vector2(0.3f, -0.5f), new Vector2(0.3f, 0.3f), Vector2.right, groundMask,
            Vector2.up * parallelInsetLen, Vector2.left * perpendicularInsetLen);

            raycasterUp = new Raycaster(new Vector2(-0.3f, 0.5f), new Vector2(0.3f, 0.5f), Vector2.up, groundMask,
            Vector2.right * parallelInsetLen, Vector2.down * perpendicularInsetLen);

            //touchDown = new Raycaster(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), Vector2.down, groundMask,
            //Vector2.right * parallelInsetLen, Vector2.up * perpendicularInsetLen, touchDownTestLength);

            foreach (var aware in lifecycleEventAwares) aware.Start();
        }

        public void Update()
        {
            if (isClimbing)
            {
                if (climbPercentage == 0)
                {
                    climbStartPosition = gameObject.transform.position;
                    Debug.Log("Starting climb.");
                }

                var currentPosition = gameObject.transform.position;

                //Climb takes 1000 milliseconds
                var climbTime = 1.0f;
                climbPercentage = (Time.deltaTime / climbTime) + climbPercentage;
                var newPosition = new Vector3(currentPosition.x, currentPosition.y, 0);
                var upPercentage = .75f;
                var overPercentage = 1.0f - upPercentage;
                var totalUpClimb = .68f;
                var totalOverClimb = .5f;
                Debug.Log($"climbPercentage: {climbPercentage}");
                if (climbPercentage <= upPercentage)
                {
                    //Going up
                    var howFarUp = climbPercentage / upPercentage;
                    newPosition.y = climbStartPosition.Value.y + (totalUpClimb * howFarUp);
                }
                else if (climbPercentage > upPercentage && climbPercentage <= 1.0f)
                {
                    newPosition.y = climbStartPosition.Value.y + totalUpClimb;
                    //Going over
                    var howFarOver = (climbPercentage - upPercentage) / overPercentage;
                    newPosition.x = climbStartPosition.Value.x + ((totalOverClimb * howFarOver) * climbDirection);
                }
                else
                {
                    Debug.Log("Climb done");
                    newPosition.y = climbStartPosition.Value.y + totalUpClimb;
                    newPosition.x = climbStartPosition.Value.x + totalOverClimb * climbDirection;
                    climbPercentage = 0;
                    isClimbing = false;
                    climbStartPosition = null;
                }
                Debug.Log($"newX: {newPosition.x} newY: {newPosition.y}");
                gameObject.transform.position = newPosition;
            }
            else
            {
                if (velocityDirection != MathUtils.GetSign(velocity.x))
                    HorizontalDirectionChanged(MathUtils.GetSign(velocity.x));
                foreach (var aware in lifecycleEventAwares) aware.Update();

                jumpStartTimer -= Time.deltaTime;
                bool jumpButtonDownThisFrame = inputController.PlayerControls.World.Jump.IsPressed();
                if (jumpButtonDownThisFrame && !jumpInputHeld)
                {
                    jumpStartTimer = jumpInputLeewayPeriod;
                }

                jumpInputHeld = jumpButtonDownThisFrame;
            }

        }

        public void FixedUpdate()
        {
            if (!isClimbing)
            {
                var hitInfo = raycasterDown.CastForHit(gameObject.transform.position);
                var downCollider = hitInfo.GetFirstCollider();
                if (hitInfo.IsColliding())
                {
                    if (downCollider.gameObject != blockOn)
                    {
                        if (CollidedWithBlock != null)
                            CollidedWithBlock(downCollider);
                        blockOn = downCollider.gameObject;
                        //Debug.Log($"Standing on new block at X:{blockOn.transform.position.x} Y:{blockOn.transform.position.y}");
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
                int inputDirection = MathUtils.GetSign(horizInput);
                velocityDirection = MathUtils.GetSign(velocity.x);


                if (inputDirection != 0)
                {
                    playerCollider.enabled = true;

                    if (inputDirection != velocityDirection)
                    {
                        velocity.x = horizStartAccel * inputDirection;
                    }
                    else
                    {
                        velocity.x = Mathf.MoveTowards(velocity.x, horizMaxSpeed * inputDirection, horizAccel * Time.deltaTime);
                    }
                    //Climb code
                    Raycaster.RaycastHitInfo climbHitInfo = null;
                    if (inputDirection > 0)
                    {
                        climbHitInfo = raycasterRight.CastForHit(gameObject.transform.position);
                    }
                    if (inputDirection < 0)
                    {
                        climbHitInfo = raycasterLeft.CastForHit(gameObject.transform.position);
                    }
                    if (climbHitInfo.ColliderOne != null && climbHitInfo.ColliderTwo == null)
                    {
                        //We arent hitting anything on the upper collider, free to climb.  Up 22px, over 32 px
                        isClimbing = true;
                        climbDirection = inputDirection;
                    }
                }
                else
                {
                    //If not in the air slow down a lot slower.
                    var horizDecelTemp = horizDecel;
                    if (!downCollider)
                        horizDecelTemp = horizDecel * 0.1f;
                    velocity.x = Mathf.MoveTowards(velocity.x, 0, horizDecelTemp * Time.deltaTime);
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
            }
            else
            {
                velocity.x = 0;
                velocity.y = 0;
                playerCollider.enabled = false;
            }

            foreach (var aware in lifecycleEventAwares) aware.FixedUpdate();
        }
    }
}
