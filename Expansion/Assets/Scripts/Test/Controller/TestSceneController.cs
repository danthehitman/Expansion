using Assets.Scripts.Common;
using Assets.Scripts.Common.Controller;
using Assets.Scripts.Common.Manager;
using Assets.Scripts.Test.View;
using Assets.Scripts.World.Controller;
using UnityEngine;

namespace Assets.Scripts.Test.Controller
{
    public class TestSceneController : BaseSceneController
    {
        private PlayerControls playerControls;

        private Vector3 zeroZeroOffset = new Vector3(0, -3, 0);

        protected override void Awake()
        {
            var goBackButtonView = new GoBackButtonView(transform);

            GameStateController.Instance.HasVisitedTest = true;
            GenerateLevel();

            playerControls = new PlayerControls();
            var inputController = new InputController(transform.GetChild(0), playerControls);
            lifecycleEventAwares.Add(inputController);

            var playerController = new EntityController<BoxCollider2D>(transform, inputController);
            lifecycleEventAwares.Add(playerController);


            //playerController.CollidedWithBlock += OnPlayerCollidedWithBlock;

            var blockController = new TestSideBlockController(transform, new Vector3(3, 0, 0) + zeroZeroOffset, inputController);
            var blockController1 = new TestSideBlockController(transform, new Vector3(3, 1, 0) + zeroZeroOffset, inputController);
            var blockController2 = new TestSideBlockController(transform, new Vector3(3, 2, 0) + zeroZeroOffset, inputController);

            var blockController3 = new TestSideBlockController(transform, new Vector3(2, 0, 0) + zeroZeroOffset, inputController);
            var blockController4 = new TestSideBlockController(transform, new Vector3(2, 1, 0) + zeroZeroOffset, inputController);

            var blockController5 = new TestSideBlockController(transform, new Vector3(1, 0, 0) + zeroZeroOffset, inputController);

            base.Awake();
        }

        private void OnPlayerCollidedWithBlock(Collider2D collider)
        {
            if (collider != null & collider.isActiveAndEnabled)
            {
                var rb = collider.attachedRigidbody;
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.AddForceAtPosition(new Vector2(100f, 0f), new Vector3(0, 0, 0));
                    collider.enabled = false;
                }
            }
        }

        private void GenerateLevel()
        {
            GameObject floorGo = new GameObject("Floor");
            floorGo.layer = Constants.GROUND_LAYER_ID;
            floorGo.transform.parent = transform;
            floorGo.transform.position = new Vector3(-0.2f, -4, 0.0f);
            floorGo.transform.localScale = new Vector3(20, 1, 0);
            var floorSr = floorGo.AddComponent<SpriteRenderer>();
            floorSr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.TILE_GRASSLAND_SPRITE);
            var floorCollider = floorGo.AddComponent<BoxCollider2D>();
        }
        protected override void Start()
        {
            base.Start();
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            playerControls?.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            playerControls?.Disable();
        }
    }
}