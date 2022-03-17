using Assets.Scripts.Common.Controller;

namespace Assets.Scripts.World.Controller
{
    public class WorldSceneController : BaseSceneController
    {
        private PlayerControls playerControls;


        protected override void Awake()
        {
            base.Awake();

            playerControls = new PlayerControls();
            var inputController = new InputController(transform.GetChild(0), playerControls);
            lifecycleEventAwares.Add(inputController);
            lifecycleEventAwares.Add(new WorldController(transform, inputController));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //TODO: Create an interface for non mono game components and add them all
            //to a list and call all of these methods in a loop for each of them.
            playerControls?.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            playerControls?.Disable();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
