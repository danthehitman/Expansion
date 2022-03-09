using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class GameController : MonoBehaviour
    {
        public WorldController WorldController;
        InputController InputController;


        private PlayerControls playerControls;


        public void Awake()
        {
            ////TODO: Move this to a cursor view.
            //var cursorGameObject = new GameObject(Constants.CURSOR, typeof(RectTransform));
            //cursorGameObject.transform.position = new Vector3(0, 0, 0);
            //var cursor_sr = cursorGameObject.AddComponent<SpriteRenderer>();
            //cursor_sr.sprite = SpriteManager.Instance.GetSpriteByName(Constants.CURSOR);
            //cursorGameObject.transform.SetParent(transform);

            var cursorView = GetComponent<CursorView>();

            //Initialize game
            playerControls = new PlayerControls();
            InputController = new InputController(cursorView.CursorSprite.transform, playerControls);
            WorldController = new WorldController(transform, InputController);

        }

        public void OnEnable()
        {
            //TODO: Create an interface for non mono game components and add them all
            //to a list and call all of these methods in a loop for each of them.
            playerControls?.Enable();
            InputController?.OnEnable();
        }

        public void OnDisable()
        {
            playerControls?.Disable();
            InputController?.OnDisable();
        }

        void Start()
        {



            WorldController?.Start();
            InputController?.Start();
        }

        void Update()
        {
            WorldController?.Update();
            InputController?.Update();
        }
    }
}
