using Assets.Scripts.Common.Controller;
using Assets.Scripts.Common.Manager;
using Assets.Scripts.Test.View;
using Assets.Scripts.World;
using Assets.Scripts.World.Controller;
using UnityEngine;

namespace Assets.Scripts.Test.Controller
{
    public class TestSceneController : BaseSceneController
    {
        private PlayerControls playerControls;

        protected override void Awake()
        {
            var goBackButtonView = new GoBackButtonView(transform);

            GameStateController.Instance.HasVisitedTest = true;
            GenerateLevel();

            playerControls = new PlayerControls();
            var inputController = new InputController(transform.GetChild(0), playerControls);
            lifecycleEventAwares.Add(inputController);

            var playerController = new EntityController<CircleCollider2D>(transform, inputController);
            lifecycleEventAwares.Add(playerController);

            base.Awake();
        }

        private void GenerateLevel()
        {
            GameObject floorGo = new GameObject("Floor");
            floorGo.layer = Constants.GROUND_LAYER_ID;
            floorGo.transform.parent = transform;
            floorGo.transform.position = new Vector3(-0.2f, -4.7f, 0.0f);
            floorGo.transform.localScale = new Vector3(20, 2, 0);
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