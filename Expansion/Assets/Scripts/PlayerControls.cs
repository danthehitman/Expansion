//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""World"",
            ""id"": ""8ae7e3f1-aeb2-4085-8523-b159bf99a00c"",
            ""actions"": [
                {
                    ""name"": ""MouseZoom"",
                    ""type"": ""Value"",
                    ""id"": ""25b51209-68c5-40e2-9360-9191ffb5bbe9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TriggerZoom"",
                    ""type"": ""Button"",
                    ""id"": ""812a66fe-2bd8-4685-a594-e5eba8ba4f6a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PanMap"",
                    ""type"": ""Value"",
                    ""id"": ""84a2dc69-30f9-470b-9513-1a1c7212cbf7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveCursor"",
                    ""type"": ""Value"",
                    ""id"": ""61fa3b89-37e2-497a-9edb-264d37788a90"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SelectButton"",
                    ""type"": ""Button"",
                    ""id"": ""bbbce34f-1dda-41c3-8133-fdb1091aa093"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1f20275c-df1d-4eef-a455-7e261b667e03"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e480c1b9-2eb3-4ad3-96ba-6e97d7872d16"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PanMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42cfc733-0ef0-4d44-9919-e90f3785b2bb"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52f101a5-d430-4e91-bbcd-7ac1e7b6827a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""GamepadTriggers"",
                    ""id"": ""efd65f34-01ad-4f42-abbc-bbfcbb3f2fd7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerZoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a49593f7-21a3-47fb-a57d-75024ecac06c"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0.01)"",
                    ""groups"": """",
                    ""action"": ""TriggerZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b2630d9c-e5eb-4d02-86fd-4b20327f2b43"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=0.01)"",
                    ""groups"": """",
                    ""action"": ""TriggerZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // World
        m_World = asset.FindActionMap("World", throwIfNotFound: true);
        m_World_MouseZoom = m_World.FindAction("MouseZoom", throwIfNotFound: true);
        m_World_TriggerZoom = m_World.FindAction("TriggerZoom", throwIfNotFound: true);
        m_World_PanMap = m_World.FindAction("PanMap", throwIfNotFound: true);
        m_World_MoveCursor = m_World.FindAction("MoveCursor", throwIfNotFound: true);
        m_World_SelectButton = m_World.FindAction("SelectButton", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // World
    private readonly InputActionMap m_World;
    private IWorldActions m_WorldActionsCallbackInterface;
    private readonly InputAction m_World_MouseZoom;
    private readonly InputAction m_World_TriggerZoom;
    private readonly InputAction m_World_PanMap;
    private readonly InputAction m_World_MoveCursor;
    private readonly InputAction m_World_SelectButton;
    public struct WorldActions
    {
        private @PlayerControls m_Wrapper;
        public WorldActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseZoom => m_Wrapper.m_World_MouseZoom;
        public InputAction @TriggerZoom => m_Wrapper.m_World_TriggerZoom;
        public InputAction @PanMap => m_Wrapper.m_World_PanMap;
        public InputAction @MoveCursor => m_Wrapper.m_World_MoveCursor;
        public InputAction @SelectButton => m_Wrapper.m_World_SelectButton;
        public InputActionMap Get() { return m_Wrapper.m_World; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WorldActions set) { return set.Get(); }
        public void SetCallbacks(IWorldActions instance)
        {
            if (m_Wrapper.m_WorldActionsCallbackInterface != null)
            {
                @MouseZoom.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnMouseZoom;
                @TriggerZoom.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnTriggerZoom;
                @TriggerZoom.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnTriggerZoom;
                @TriggerZoom.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnTriggerZoom;
                @PanMap.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnPanMap;
                @PanMap.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnPanMap;
                @PanMap.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnPanMap;
                @MoveCursor.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnMoveCursor;
                @SelectButton.started -= m_Wrapper.m_WorldActionsCallbackInterface.OnSelectButton;
                @SelectButton.performed -= m_Wrapper.m_WorldActionsCallbackInterface.OnSelectButton;
                @SelectButton.canceled -= m_Wrapper.m_WorldActionsCallbackInterface.OnSelectButton;
            }
            m_Wrapper.m_WorldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseZoom.started += instance.OnMouseZoom;
                @MouseZoom.performed += instance.OnMouseZoom;
                @MouseZoom.canceled += instance.OnMouseZoom;
                @TriggerZoom.started += instance.OnTriggerZoom;
                @TriggerZoom.performed += instance.OnTriggerZoom;
                @TriggerZoom.canceled += instance.OnTriggerZoom;
                @PanMap.started += instance.OnPanMap;
                @PanMap.performed += instance.OnPanMap;
                @PanMap.canceled += instance.OnPanMap;
                @MoveCursor.started += instance.OnMoveCursor;
                @MoveCursor.performed += instance.OnMoveCursor;
                @MoveCursor.canceled += instance.OnMoveCursor;
                @SelectButton.started += instance.OnSelectButton;
                @SelectButton.performed += instance.OnSelectButton;
                @SelectButton.canceled += instance.OnSelectButton;
            }
        }
    }
    public WorldActions @World => new WorldActions(this);
    public interface IWorldActions
    {
        void OnMouseZoom(InputAction.CallbackContext context);
        void OnTriggerZoom(InputAction.CallbackContext context);
        void OnPanMap(InputAction.CallbackContext context);
        void OnMoveCursor(InputAction.CallbackContext context);
        void OnSelectButton(InputAction.CallbackContext context);
    }
}
