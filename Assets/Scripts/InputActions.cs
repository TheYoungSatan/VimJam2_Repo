// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""MiniGame"",
            ""id"": ""f0c36062-f867-4077-a13b-907e9ec03368"",
            ""actions"": [
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""f4f9bdaa-04ed-4d25-8f37-6c86aa6fe3fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""4fd88213-3a24-4bcb-b950-32367c8898e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""90ee411c-53cc-44e9-9c39-d0b650533454"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""68eca46d-31b3-4577-88da-1ae2bfda72b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse"",
                    ""type"": ""Value"",
                    ""id"": ""1ac6f9f5-adde-4618-bb7b-6cc65cb9e3d6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b5138aa4-444a-4243-8324-c388c4b9011a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""6f6af874-9d1c-450b-bd48-5e40e8916654"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""988b1358-7da5-41ca-839b-2e062b007e84"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""597f0b04-caeb-4529-910f-079259fbd39b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""450c3216-5c59-4e71-8723-7eba9bdcef36"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0ebee7b-b66d-4d24-8968-060ebcfa34bb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03e49976-e021-4ec9-b608-47d668aa4527"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b186239-08b2-44b7-ac73-b077dbb8b9f6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33bef7e2-6df7-4882-9c8a-dd9454ee7b49"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1dfa4b1-80e8-4daa-912f-b2a1ec527458"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70512d40-321e-4d56-bf86-ead8e2bcb4fb"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4054ef78-5f15-49c5-883d-354cc2c12148"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""a03c1805-0d10-44a2-a6a1-4ed31655861b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""30cc15f5-e017-436d-bf57-8720470079eb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a1f11ddb-ae96-4d7d-9fac-e1c0660c9957"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""85b96cd2-e2b7-4b0d-a5ab-5d987d905bb6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c86f368f-cd1e-4573-ae8a-a97469d8cb41"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cb0ae484-3b8a-4bf7-a6f2-3547226dd300"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbad4367-bdcb-45d3-a75c-bc915df36783"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MiniGame
        m_MiniGame = asset.FindActionMap("MiniGame", throwIfNotFound: true);
        m_MiniGame_Up = m_MiniGame.FindAction("Up", throwIfNotFound: true);
        m_MiniGame_Down = m_MiniGame.FindAction("Down", throwIfNotFound: true);
        m_MiniGame_Left = m_MiniGame.FindAction("Left", throwIfNotFound: true);
        m_MiniGame_Right = m_MiniGame.FindAction("Right", throwIfNotFound: true);
        m_MiniGame_Mouse = m_MiniGame.FindAction("Mouse", throwIfNotFound: true);
        m_MiniGame_Move = m_MiniGame.FindAction("Move", throwIfNotFound: true);
        m_MiniGame_Interact = m_MiniGame.FindAction("Interact", throwIfNotFound: true);
        m_MiniGame_MouseClick = m_MiniGame.FindAction("MouseClick", throwIfNotFound: true);
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

    // MiniGame
    private readonly InputActionMap m_MiniGame;
    private IMiniGameActions m_MiniGameActionsCallbackInterface;
    private readonly InputAction m_MiniGame_Up;
    private readonly InputAction m_MiniGame_Down;
    private readonly InputAction m_MiniGame_Left;
    private readonly InputAction m_MiniGame_Right;
    private readonly InputAction m_MiniGame_Mouse;
    private readonly InputAction m_MiniGame_Move;
    private readonly InputAction m_MiniGame_Interact;
    private readonly InputAction m_MiniGame_MouseClick;
    public struct MiniGameActions
    {
        private @InputActions m_Wrapper;
        public MiniGameActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Up => m_Wrapper.m_MiniGame_Up;
        public InputAction @Down => m_Wrapper.m_MiniGame_Down;
        public InputAction @Left => m_Wrapper.m_MiniGame_Left;
        public InputAction @Right => m_Wrapper.m_MiniGame_Right;
        public InputAction @Mouse => m_Wrapper.m_MiniGame_Mouse;
        public InputAction @Move => m_Wrapper.m_MiniGame_Move;
        public InputAction @Interact => m_Wrapper.m_MiniGame_Interact;
        public InputAction @MouseClick => m_Wrapper.m_MiniGame_MouseClick;
        public InputActionMap Get() { return m_Wrapper.m_MiniGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MiniGameActions set) { return set.Get(); }
        public void SetCallbacks(IMiniGameActions instance)
        {
            if (m_Wrapper.m_MiniGameActionsCallbackInterface != null)
            {
                @Up.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnUp;
                @Down.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnDown;
                @Left.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnLeft;
                @Right.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnRight;
                @Mouse.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouse;
                @Mouse.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouse;
                @Mouse.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouse;
                @Move.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnInteract;
                @MouseClick.started -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouseClick;
                @MouseClick.performed -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouseClick;
                @MouseClick.canceled -= m_Wrapper.m_MiniGameActionsCallbackInterface.OnMouseClick;
            }
            m_Wrapper.m_MiniGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
                @Mouse.started += instance.OnMouse;
                @Mouse.performed += instance.OnMouse;
                @Mouse.canceled += instance.OnMouse;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @MouseClick.started += instance.OnMouseClick;
                @MouseClick.performed += instance.OnMouseClick;
                @MouseClick.canceled += instance.OnMouseClick;
            }
        }
    }
    public MiniGameActions @MiniGame => new MiniGameActions(this);
    public interface IMiniGameActions
    {
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
        void OnMouse(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnMouseClick(InputAction.CallbackContext context);
    }
}
