// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerEssentials/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""FreeMovement"",
            ""id"": ""7f32a630-5d84-489f-8d68-011fe9e4053f"",
            ""actions"": [
                {
                    ""name"": ""Translation"",
                    ""type"": ""Value"",
                    ""id"": ""94b2f237-27c4-45c7-9a40-ef0290154a9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HoldHand"",
                    ""type"": ""Button"",
                    ""id"": ""6ef3c105-1db9-4c47-a8d4-2523c24fb28e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookAround"",
                    ""type"": ""Value"",
                    ""id"": ""16d51d9d-627d-4228-b7f0-fd97d14bafab"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""a225952a-70a5-48ec-a0c7-233566a33ae0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ASDW"",
                    ""id"": ""23b5c89b-26f2-4f94-89a0-ee943a743d27"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d5cd7de0-952c-4124-8bbc-3a02283bfdbd"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ff2eafd9-231c-42a7-a8d2-c280e1d76c70"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0f8ce597-e1d6-488d-8dcf-eb5788aefdce"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1af0c636-9587-42d5-8114-8f4a0e02858a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Translation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""MouseHold"",
                    ""id"": ""a664f369-101b-4b0f-aa50-00d7b602ca14"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldHand"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""button"",
                    ""id"": ""fa7c708c-f874-4ee1-8876-82a266c0481b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""2687aea2-a387-4bab-8c06-f6d4d96ab67a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldHand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2b2967ed-be27-4fd8-84b4-daf7472d9fcd"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a71c29be-97ea-44e3-a2db-6e5e7178e586"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // FreeMovement
        m_FreeMovement = asset.FindActionMap("FreeMovement", throwIfNotFound: true);
        m_FreeMovement_Translation = m_FreeMovement.FindAction("Translation", throwIfNotFound: true);
        m_FreeMovement_HoldHand = m_FreeMovement.FindAction("HoldHand", throwIfNotFound: true);
        m_FreeMovement_LookAround = m_FreeMovement.FindAction("LookAround", throwIfNotFound: true);
        m_FreeMovement_Sprint = m_FreeMovement.FindAction("Sprint", throwIfNotFound: true);
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

    // FreeMovement
    private readonly InputActionMap m_FreeMovement;
    private IFreeMovementActions m_FreeMovementActionsCallbackInterface;
    private readonly InputAction m_FreeMovement_Translation;
    private readonly InputAction m_FreeMovement_HoldHand;
    private readonly InputAction m_FreeMovement_LookAround;
    private readonly InputAction m_FreeMovement_Sprint;
    public struct FreeMovementActions
    {
        private @PlayerControls m_Wrapper;
        public FreeMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Translation => m_Wrapper.m_FreeMovement_Translation;
        public InputAction @HoldHand => m_Wrapper.m_FreeMovement_HoldHand;
        public InputAction @LookAround => m_Wrapper.m_FreeMovement_LookAround;
        public InputAction @Sprint => m_Wrapper.m_FreeMovement_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_FreeMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FreeMovementActions set) { return set.Get(); }
        public void SetCallbacks(IFreeMovementActions instance)
        {
            if (m_Wrapper.m_FreeMovementActionsCallbackInterface != null)
            {
                @Translation.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnTranslation;
                @Translation.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnTranslation;
                @Translation.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnTranslation;
                @HoldHand.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnHoldHand;
                @HoldHand.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnHoldHand;
                @HoldHand.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnHoldHand;
                @LookAround.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnLookAround;
                @LookAround.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnLookAround;
                @LookAround.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnLookAround;
                @Sprint.started -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_FreeMovementActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_FreeMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Translation.started += instance.OnTranslation;
                @Translation.performed += instance.OnTranslation;
                @Translation.canceled += instance.OnTranslation;
                @HoldHand.started += instance.OnHoldHand;
                @HoldHand.performed += instance.OnHoldHand;
                @HoldHand.canceled += instance.OnHoldHand;
                @LookAround.started += instance.OnLookAround;
                @LookAround.performed += instance.OnLookAround;
                @LookAround.canceled += instance.OnLookAround;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public FreeMovementActions @FreeMovement => new FreeMovementActions(this);
    public interface IFreeMovementActions
    {
        void OnTranslation(InputAction.CallbackContext context);
        void OnHoldHand(InputAction.CallbackContext context);
        void OnLookAround(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
}
