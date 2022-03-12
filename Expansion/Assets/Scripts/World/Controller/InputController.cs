using Assets.Scripts.Common;
using Assets.Scripts.World.View;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.World.Controller
{
    public enum InputType
    {
        Gamepad,
        Mouse,
    }

    public class InputController : ILifecycleEventAware
    {
        private const float DOUBLE_CLICK_INTERVAL = 0.25f;
        private PlayerControls playerControls;
        private Vector3 mouseLastPosition;
        private int lastWorldX;
        private int lastWorldY;
        private bool isPanning = false;
        private float panningThreshold = .015f;
        private Vector3 panningMouseStart = Vector3.zero;
        private Mouse virtualMouse;
        private Transform cursorTransform;
        private InputType inputType;
        private double? leftButtonDownStart = null;


        protected Action<int, int> OnCursorOverWorldCoordinateChanged;
        protected Action OnDoubleClickWorldCoordinate;


        public InputController(Transform uiTransform, PlayerControls playerControls)
        {
            var cursorView = new CursorView(uiTransform);
            cursorTransform = cursorView.GameObject.transform;
            this.playerControls = playerControls;
        }

        public void RegisterOnCursorOverWorldCoordinateChangedCallback(Action<int, int> callback)
            => OnCursorOverWorldCoordinateChanged += callback;

        public void UnregisterOnCursorOverWorldCoordinateChangedCallback(Action<int, int> callback)
            => OnCursorOverWorldCoordinateChanged -= callback;

        public void RegisterOnDoubleClickWorldCoordinateCallback(Action callback)
            => OnDoubleClickWorldCoordinate += callback;

        public void UnregisterOnDoubleClickWorldCoordinateCallback(Action callback)
            => OnDoubleClickWorldCoordinate -= callback;


        public void Awake()
        {
        }


        // Use this for initialization
        public void Start()
        {

        }

        public void OnEnable()
        {
            inputType = InputType.Mouse;

            if (virtualMouse == null)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }

            if (cursorTransform != null)
            {
                Vector2 position = cursorTransform.position;
                InputState.Change(virtualMouse.position, position);
            }

            InputSystem.onAfterUpdate += HandleOnAfterUpdate;
            playerControls.World.WakeupController.started += (CallbackContext ctx) => { SetActiveInputType(InputType.Gamepad); };
            playerControls.World.WakeupMouse.started += (CallbackContext ctx) => { SetActiveInputType(InputType.Mouse); };
            playerControls.World.MouseLeftButtonClick.started += HandleOnMouseLeftButtonClick;
            playerControls.World.MouseLeftButtonClick.canceled += HandleOnMouseLeftButtonClick;
        }

        public void OnDisable()
        {
            InputSystem.RemoveDevice(virtualMouse);
            InputSystem.onAfterUpdate -= HandleOnAfterUpdate;
            playerControls.World.WakeupController.started -= (CallbackContext ctx) => { SetActiveInputType(InputType.Gamepad); };
            playerControls.World.WakeupMouse.started -= (CallbackContext ctx) => { SetActiveInputType(InputType.Mouse); };
            playerControls.World.MouseLeftButtonClick.started -= HandleOnMouseLeftButtonClick;
            playerControls.World.MouseLeftButtonClick.canceled -= HandleOnMouseLeftButtonClick;
        }

        // Update is called once per frame
        public void Update()
        {
        }

        public void OnDestroy()
        {
        }

        private void SetActiveInputType(InputType inputType)
        {
            //TODO: Cant figure out why it takes two controller button pushes to make the switch.
            //The first time you switch from mouse to controller this method gets called twice,
            //first for gamepad switch, and then for mouse switch.
            if (this.inputType != inputType && inputType == InputType.Mouse)
            {
                cursorTransform.gameObject.SetActive(false);
            }
            if (this.inputType != inputType && inputType == InputType.Gamepad)
            {
                cursorTransform.gameObject.SetActive(true);
            }
            Debug.Log($"ExistingInputType: {this.inputType} NewInputType: {inputType}");
            this.inputType = inputType;
        }

        private void HandleOnMouseLeftButtonClick(CallbackContext ctx)
        {
            Debug.Log($"Started: {ctx.started} Canceled: {ctx.canceled} Performed: {ctx.performed}");
            Debug.Log($"Time: {ctx.time} StartTime: {ctx.startTime} Duration: {ctx.duration}");
            if (ctx.started)
            {
                if (leftButtonDownStart != null)
                {
                    if (ctx.time - leftButtonDownStart < DOUBLE_CLICK_INTERVAL)
                    {
                        OnDoubleClickWorldCoordinate();
                    }
                    leftButtonDownStart = ctx.startTime;
                }
                else
                {
                    leftButtonDownStart = ctx.startTime;
                }
            }
            else if (ctx.canceled)
            {
            }
        }

        private void HandleOnAfterUpdate()
        {
            if (inputType == InputType.Gamepad) HandleGamepadMouse();
            if (inputType == InputType.Mouse) HandleMouseInput();
            HandlZoomInput();
        }

        private void HandleGamepadMouse()
        {
            var speed = 1000;
            if (virtualMouse != null && Gamepad.current != null)
            {
                Vector2 stickValue = playerControls.World.MoveCursor.ReadValue<Vector2>();
                stickValue *= speed * Time.deltaTime;
                Vector2 currentPosition = virtualMouse.position.ReadValue();
                Vector2 newPosition = currentPosition + stickValue;

                newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

                InputState.Change(virtualMouse.position, newPosition);
                InputState.Change(virtualMouse.delta, stickValue);

                //TODO: This probably needs to track the previous state and make sure it doesnt already line up with the button press.
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, playerControls.World.SelectButton.IsPressed());
                InputState.Change(virtualMouse, mouseState);

                UpdateMouseWorldTilePosition(Camera.main.ScreenToWorldPoint(new Vector3(newPosition.x, newPosition.y, 0)), () => { return newPosition; });

                AnchorCursor(newPosition);
            }

            var panMapVector = playerControls.World.PanMap.ReadValue<Vector2>();
            if (playerControls.World.PanMap.IsPressed())
            {
                var moveSpeed = 10;
                Camera.main.transform.Translate(moveSpeed * Time.deltaTime * panMapVector);
            }
        }

        private void AnchorCursor(Vector2 position)
        {
            cursorTransform.position = position;
        }


        private void HandleMouseInput()
        {
            var currentMouseVector = Mouse.current.position.ReadValue();
            var mouseCurrentPosition = Camera.main.ScreenToWorldPoint(new Vector3(currentMouseVector.x, currentMouseVector.y, 0));


            //If the left mouse button isnt down then we arent panning.
            if (!Mouse.current.leftButton.isPressed)
            {
                isPanning = false;
            }
            else
            {
                //Check if the screen is being dragged.
                if (Vector3.Distance(panningMouseStart, mouseCurrentPosition) > panningThreshold * Camera.main.orthographicSize)
                {
                    isPanning = true;
                }

                //Handle screen drag.
                if (Mouse.current.leftButton.isPressed && isPanning)
                {
                    Vector3 diff = mouseLastPosition - mouseCurrentPosition;
                    Camera.main.transform.Translate(diff);
                }
            }

            UpdateMouseWorldTilePosition(mouseCurrentPosition, () => { return Mouse.current.position.ReadValue(); });

        }

        private void HandlZoomInput()
        {
            //Zoom in/out
            //Handled here instead of in a callback because we want continuous press in less code.

            var scroll = 0.0f;
            //Mouse Zoom
            var mouseZoom = playerControls.World.MouseZoom.ReadValue<float>();
            if (mouseZoom != 0)
            {
                scroll = mouseZoom > 0 ? 0.1f : -0.1f;
            }
            //Trigger Zoom
            var triggerZoom = playerControls.World.TriggerZoom.ReadValue<float>();
            if (triggerZoom != 0)
            {
                scroll = triggerZoom * 0.005f;
            }

            Camera.main.orthographicSize -= Camera.main.orthographicSize * scroll * 2;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 3f, 200f);
        }

        private void UpdateMouseWorldTilePosition(Vector3 mouseCurrentPosition, Func<Vector2> getMouseCurrentPosition)
        {
            int currentXFloor = Mathf.FloorToInt(mouseCurrentPosition.x + 0.5f);
            int currentYFloor = Mathf.FloorToInt(mouseCurrentPosition.y + 0.5f);
            if (
                (lastWorldX != currentXFloor || lastWorldY != currentYFloor)
                && OnCursorOverWorldCoordinateChanged != null
            )
            {
                OnCursorOverWorldCoordinateChanged(currentXFloor, currentYFloor);
            }

            //Even though we just got the value, if we dont get it again here the screen gets the jitters.
            var currentMouseVector = getMouseCurrentPosition();
            mouseLastPosition = Camera.main.ScreenToWorldPoint(new Vector3(currentMouseVector.x, currentMouseVector.y, 0));

            lastWorldX = Mathf.FloorToInt(mouseLastPosition.x + 0.5f);
            lastWorldY = Mathf.FloorToInt(mouseLastPosition.y + 0.5f);
        }
    }

}
