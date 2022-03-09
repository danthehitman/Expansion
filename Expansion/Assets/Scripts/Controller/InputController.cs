using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Controller
{

    public class InputController
    {
        private Vector3 mouseLastPosition;
        private int lastWorldX;
        private int lastWorldY;
        private bool isPanning = false;
        private float panningThreshold = .015f;
        private Vector3 panningMouseStart = Vector3.zero;
        private Mouse virtualMouse;
        private PlayerInput playerInput;
        private Transform cursorTransform;

        protected Action<int, int> OnCursorOverWorldCoordinateChanged;
        protected PlayerControls PlayerControls;

        public InputController(Transform cursorTransform, PlayerControls playerControls)
        {
            this.cursorTransform = cursorTransform;
            PlayerControls = playerControls;
        }

        public void RegisterOnCursorOverWorldCoordinateChangedCallback(Action<int, int> callback)
            => OnCursorOverWorldCoordinateChanged += callback;

        public void UnregisterOnCursorOverWorldCoordinateChangedCallback(Action<int, int> callback)
            => OnCursorOverWorldCoordinateChanged -= callback;

        public void OnEnable()
        {

            if (virtualMouse == null)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }

            //InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            if (cursorTransform != null)
            {
                Vector2 position = cursorTransform.position;
                InputState.Change(virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += UpdateGamepadCursor;
            InputSystem.onAfterUpdate += HandleContinuousUpdates;
        }

        public void OnDisable()
        {
            InputSystem.RemoveDevice(virtualMouse);
            InputSystem.onAfterUpdate -= UpdateGamepadCursor;
            InputSystem.onAfterUpdate -= HandleContinuousUpdates;
        }


        // Use this for initialization
        public void Start()
        {

        }

        // Update is called once per frame
        public void Update()
        {
            //HandleContinuousUpdates();
        }

        private void HandlePanMap(CallbackContext context)
        {

        }

        private void UpdateGamepadCursor()
        {
            var speed = 1000;
            if (virtualMouse != null && Gamepad.current != null)
            {
                Vector2 stickValue = PlayerControls.World.MoveCursor.ReadValue<Vector2>();
                stickValue *= speed * Time.deltaTime;
                Vector2 currentPosition = virtualMouse.position.ReadValue();
                Vector2 newPosition = currentPosition + stickValue;

                newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

                InputState.Change(virtualMouse.position, newPosition);
                InputState.Change(virtualMouse.delta, stickValue);

                //TODO: This probably needs to track the previous state and make sure it doesnt already line up with the button press.
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, PlayerControls.World.SelectButton.IsPressed());
                InputState.Change(virtualMouse, mouseState);

                AnchorCursor(newPosition);
            }
        }

        private void AnchorCursor(Vector2 position)
        {
            cursorTransform.position = position;
        }


        private void HandleContinuousUpdates()
        {
            //var mouseCurrentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseCurrentPosition = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));

            //Check if the screen is being dragged.
            if (Vector3.Distance(panningMouseStart, mouseCurrentPosition) > panningThreshold * Camera.main.orthographicSize)
            {
                isPanning = true;
            }

            //Handle screen drag.
            //if (Input.GetMouseButton(0) && isPanning)
            if (Mouse.current.leftButton.isPressed && isPanning)
            {
                Vector3 diff = mouseLastPosition - mouseCurrentPosition;
                Camera.main.transform.Translate(diff);
            }

            var panMapVector = PlayerControls.World.PanMap.ReadValue<Vector2>();
            if (PlayerControls.World.PanMap.IsPressed())
            {
                var moveSpeed = 10;
                Camera.main.transform.Translate(moveSpeed * Time.deltaTime * panMapVector);
            }

            //If the left mouse button isnt down then we arent panning.
            //if (!Input.GetMouseButton(0))
            if (Mouse.current.leftButton.isPressed)
            {
                isPanning = false;
            }

            UpdateMouseWorldTilePosition(mouseCurrentPosition);

            //Zoom in/out
            //Handled here instead of in a callback because we want continuous press in less code.

            var scroll = 0.0f;
            //Mouse Zoom
            var mouseZoom = PlayerControls.World.MouseZoom.ReadValue<float>();
            if (mouseZoom != 0)
            {
                scroll = mouseZoom > 0 ? 0.1f : -0.1f;
            }
            //Trigger Zoom
            var triggerZoom = PlayerControls.World.TriggerZoom.ReadValue<float>();
            if (triggerZoom != 0)
            {
                scroll = triggerZoom * 0.005f;
            }

            Camera.main.orthographicSize -= Camera.main.orthographicSize * scroll * 2;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 200f);

        }

        private void UpdateMouseWorldTilePosition(Vector3 mouseCurrentPosition)
        {

            CompareCurrentWithLastCoordsAndNotify(currentXFloor, currentYFloor);

            //MouseLastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseLastPosition = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));

            lastWorldX = Mathf.FloorToInt(mouseLastPosition.x + 0.5f);
            lastWorldY = Mathf.FloorToInt(mouseLastPosition.y + 0.5f);
        }

        private void CompareCurrentWithLastCoordsAndNotify(Vector3 mouseCurrentPosition)
        {
            int currentXFloor = Mathf.FloorToInt(mouseCurrentPosition.x + 0.5f);
            int currentYFloor = Mathf.FloorToInt(mouseCurrentPosition.y + 0.5f);

            if (
                   (lastWorldX != currentX || lastWorldY != currentY)
                   && OnCursorOverWorldCoordinateChanged != null
               )
            {
                OnCursorOverWorldCoordinateChanged(currentX, currentY);
            }

        }
    }

}
