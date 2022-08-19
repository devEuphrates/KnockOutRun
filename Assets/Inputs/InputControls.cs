// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/InputControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""GameInputs"",
            ""id"": ""198c3075-008a-4c33-a90d-5f6f4d35b979"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""Value"",
                    ""id"": ""cb12a1fe-8eea-4ace-b340-c4b229b7d39a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ad3efeb1-997b-4618-a464-55875c884f54"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameInputs
        m_GameInputs = asset.FindActionMap("GameInputs", throwIfNotFound: true);
        m_GameInputs_TouchInput = m_GameInputs.FindAction("TouchInput", throwIfNotFound: true);
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

    // GameInputs
    private readonly InputActionMap m_GameInputs;
    private IGameInputsActions m_GameInputsActionsCallbackInterface;
    private readonly InputAction m_GameInputs_TouchInput;
    public struct GameInputsActions
    {
        private @InputControls m_Wrapper;
        public GameInputsActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchInput => m_Wrapper.m_GameInputs_TouchInput;
        public InputActionMap Get() { return m_Wrapper.m_GameInputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameInputsActions set) { return set.Get(); }
        public void SetCallbacks(IGameInputsActions instance)
        {
            if (m_Wrapper.m_GameInputsActionsCallbackInterface != null)
            {
                @TouchInput.started -= m_Wrapper.m_GameInputsActionsCallbackInterface.OnTouchInput;
                @TouchInput.performed -= m_Wrapper.m_GameInputsActionsCallbackInterface.OnTouchInput;
                @TouchInput.canceled -= m_Wrapper.m_GameInputsActionsCallbackInterface.OnTouchInput;
            }
            m_Wrapper.m_GameInputsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchInput.started += instance.OnTouchInput;
                @TouchInput.performed += instance.OnTouchInput;
                @TouchInput.canceled += instance.OnTouchInput;
            }
        }
    }
    public GameInputsActions @GameInputs => new GameInputsActions(this);
    public interface IGameInputsActions
    {
        void OnTouchInput(InputAction.CallbackContext context);
    }
}
