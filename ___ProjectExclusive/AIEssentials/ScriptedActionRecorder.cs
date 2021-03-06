// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/___ProjectExclusive/AIEssentials/ScriptedActionRecorder.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AIEssentials
{
    public class @ScriptedActionRecorderMaps : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @ScriptedActionRecorderMaps()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""ScriptedActionRecorder"",
    ""maps"": [
        {
            ""name"": ""ScriptedActionRecorder"",
            ""id"": ""054aba70-ddea-4988-8471-83b1ed15d253"",
            ""actions"": [
                {
                    ""name"": ""AddScriptedAction"",
                    ""type"": ""Button"",
                    ""id"": ""07c43db7-4253-4a4d-9e6a-e94e9074a267"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveScriptedAction"",
                    ""type"": ""Button"",
                    ""id"": ""23468ef3-3624-446a-ba15-d2c6d83da4a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""VariateIndex"",
                    ""type"": ""Value"",
                    ""id"": ""5c9eb83e-055c-490d-b67d-ffa8cabce461"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookAtDistance"",
                    ""type"": ""Button"",
                    ""id"": ""bf125aec-aead-4a76-b8bc-4a397b8ba11c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AreaDistance"",
                    ""type"": ""Button"",
                    ""id"": ""d66e5981-bb52-4641-a80d-d1a5e2374197"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""8f9fb8d4-bbe2-417a-9410-22db620a2f85"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""f4da048d-aad6-4215-a8bd-51e5f789c3d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action3"",
                    ""type"": ""Button"",
                    ""id"": ""0d1d2da2-61ce-4b37-99b0-a82f35daf705"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UpdateCurrent"",
                    ""type"": ""Button"",
                    ""id"": ""749b6ada-2069-41db-afcd-1f49b9a32ecd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9e16c88a-0c8b-450d-9b69-e84da4959bcc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AddScriptedAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7f47e8b-903a-4e8a-9be8-2a53e63476d2"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RemoveScriptedAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89f3b43a-7946-42b1-b079-944e05320e6b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca805809-ac8b-4c9b-9e2b-5fb2c84e49b8"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf392620-0df3-422c-9024-e7d7eb2c77bb"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c5e14a8-1823-4b2b-9d8f-8a04cc3e890c"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4ed4c49-dd10-4052-a786-2997cc442d93"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97979468-a2cc-4e63-801e-abbe07d164d1"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Variation"",
                    ""id"": ""c8c5191b-7f12-406c-b0f1-d5ef86014162"",
                    ""path"": ""1DAxis(minValue=0,maxValue=10)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAtDistance"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7a4517d5-8e8b-4e02-8580-4e6380c768ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAtDistance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0e06d5bb-b8d7-4f1e-bc59-4b826a896011"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAtDistance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fa00866e-4475-4d51-8c70-720299096dcd"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UpdateCurrent"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57ac410e-a37d-4c3c-b7e3-0e31930422e4"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VariateIndex"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Variation"",
                    ""id"": ""5fa27610-df7f-48a9-a640-2605abefe0d1"",
                    ""path"": ""1DAxis(minValue=0,maxValue=10)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaDistance"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ec7cfb79-cf2a-41d6-8a01-37ebdb3f6f18"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaDistance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1ecbbcba-a685-4f06-a1e3-debc21e50df5"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AreaDistance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // ScriptedActionRecorder
            m_ScriptedActionRecorder = asset.FindActionMap("ScriptedActionRecorder", throwIfNotFound: true);
            m_ScriptedActionRecorder_AddScriptedAction = m_ScriptedActionRecorder.FindAction("AddScriptedAction", throwIfNotFound: true);
            m_ScriptedActionRecorder_RemoveScriptedAction = m_ScriptedActionRecorder.FindAction("RemoveScriptedAction", throwIfNotFound: true);
            m_ScriptedActionRecorder_VariateIndex = m_ScriptedActionRecorder.FindAction("VariateIndex", throwIfNotFound: true);
            m_ScriptedActionRecorder_LookAtDistance = m_ScriptedActionRecorder.FindAction("LookAtDistance", throwIfNotFound: true);
            m_ScriptedActionRecorder_AreaDistance = m_ScriptedActionRecorder.FindAction("AreaDistance", throwIfNotFound: true);
            m_ScriptedActionRecorder_Action1 = m_ScriptedActionRecorder.FindAction("Action1", throwIfNotFound: true);
            m_ScriptedActionRecorder_Action2 = m_ScriptedActionRecorder.FindAction("Action2", throwIfNotFound: true);
            m_ScriptedActionRecorder_Action3 = m_ScriptedActionRecorder.FindAction("Action3", throwIfNotFound: true);
            m_ScriptedActionRecorder_UpdateCurrent = m_ScriptedActionRecorder.FindAction("UpdateCurrent", throwIfNotFound: true);
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

        // ScriptedActionRecorder
        private readonly InputActionMap m_ScriptedActionRecorder;
        private IScriptedActionRecorderActions m_ScriptedActionRecorderActionsCallbackInterface;
        private readonly InputAction m_ScriptedActionRecorder_AddScriptedAction;
        private readonly InputAction m_ScriptedActionRecorder_RemoveScriptedAction;
        private readonly InputAction m_ScriptedActionRecorder_VariateIndex;
        private readonly InputAction m_ScriptedActionRecorder_LookAtDistance;
        private readonly InputAction m_ScriptedActionRecorder_AreaDistance;
        private readonly InputAction m_ScriptedActionRecorder_Action1;
        private readonly InputAction m_ScriptedActionRecorder_Action2;
        private readonly InputAction m_ScriptedActionRecorder_Action3;
        private readonly InputAction m_ScriptedActionRecorder_UpdateCurrent;
        public struct ScriptedActionRecorderActions
        {
            private @ScriptedActionRecorderMaps m_Wrapper;
            public ScriptedActionRecorderActions(@ScriptedActionRecorderMaps wrapper) { m_Wrapper = wrapper; }
            public InputAction @AddScriptedAction => m_Wrapper.m_ScriptedActionRecorder_AddScriptedAction;
            public InputAction @RemoveScriptedAction => m_Wrapper.m_ScriptedActionRecorder_RemoveScriptedAction;
            public InputAction @VariateIndex => m_Wrapper.m_ScriptedActionRecorder_VariateIndex;
            public InputAction @LookAtDistance => m_Wrapper.m_ScriptedActionRecorder_LookAtDistance;
            public InputAction @AreaDistance => m_Wrapper.m_ScriptedActionRecorder_AreaDistance;
            public InputAction @Action1 => m_Wrapper.m_ScriptedActionRecorder_Action1;
            public InputAction @Action2 => m_Wrapper.m_ScriptedActionRecorder_Action2;
            public InputAction @Action3 => m_Wrapper.m_ScriptedActionRecorder_Action3;
            public InputAction @UpdateCurrent => m_Wrapper.m_ScriptedActionRecorder_UpdateCurrent;
            public InputActionMap Get() { return m_Wrapper.m_ScriptedActionRecorder; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ScriptedActionRecorderActions set) { return set.Get(); }
            public void SetCallbacks(IScriptedActionRecorderActions instance)
            {
                if (m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface != null)
                {
                    @AddScriptedAction.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAddScriptedAction;
                    @AddScriptedAction.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAddScriptedAction;
                    @AddScriptedAction.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAddScriptedAction;
                    @RemoveScriptedAction.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnRemoveScriptedAction;
                    @RemoveScriptedAction.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnRemoveScriptedAction;
                    @RemoveScriptedAction.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnRemoveScriptedAction;
                    @VariateIndex.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnVariateIndex;
                    @VariateIndex.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnVariateIndex;
                    @VariateIndex.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnVariateIndex;
                    @LookAtDistance.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnLookAtDistance;
                    @LookAtDistance.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnLookAtDistance;
                    @LookAtDistance.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnLookAtDistance;
                    @AreaDistance.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAreaDistance;
                    @AreaDistance.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAreaDistance;
                    @AreaDistance.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAreaDistance;
                    @Action1.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction1;
                    @Action1.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction1;
                    @Action1.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction1;
                    @Action2.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction2;
                    @Action2.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction2;
                    @Action2.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction2;
                    @Action3.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction3;
                    @Action3.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction3;
                    @Action3.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnAction3;
                    @UpdateCurrent.started -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnUpdateCurrent;
                    @UpdateCurrent.performed -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnUpdateCurrent;
                    @UpdateCurrent.canceled -= m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface.OnUpdateCurrent;
                }
                m_Wrapper.m_ScriptedActionRecorderActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @AddScriptedAction.started += instance.OnAddScriptedAction;
                    @AddScriptedAction.performed += instance.OnAddScriptedAction;
                    @AddScriptedAction.canceled += instance.OnAddScriptedAction;
                    @RemoveScriptedAction.started += instance.OnRemoveScriptedAction;
                    @RemoveScriptedAction.performed += instance.OnRemoveScriptedAction;
                    @RemoveScriptedAction.canceled += instance.OnRemoveScriptedAction;
                    @VariateIndex.started += instance.OnVariateIndex;
                    @VariateIndex.performed += instance.OnVariateIndex;
                    @VariateIndex.canceled += instance.OnVariateIndex;
                    @LookAtDistance.started += instance.OnLookAtDistance;
                    @LookAtDistance.performed += instance.OnLookAtDistance;
                    @LookAtDistance.canceled += instance.OnLookAtDistance;
                    @AreaDistance.started += instance.OnAreaDistance;
                    @AreaDistance.performed += instance.OnAreaDistance;
                    @AreaDistance.canceled += instance.OnAreaDistance;
                    @Action1.started += instance.OnAction1;
                    @Action1.performed += instance.OnAction1;
                    @Action1.canceled += instance.OnAction1;
                    @Action2.started += instance.OnAction2;
                    @Action2.performed += instance.OnAction2;
                    @Action2.canceled += instance.OnAction2;
                    @Action3.started += instance.OnAction3;
                    @Action3.performed += instance.OnAction3;
                    @Action3.canceled += instance.OnAction3;
                    @UpdateCurrent.started += instance.OnUpdateCurrent;
                    @UpdateCurrent.performed += instance.OnUpdateCurrent;
                    @UpdateCurrent.canceled += instance.OnUpdateCurrent;
                }
            }
        }
        public ScriptedActionRecorderActions @ScriptedActionRecorder => new ScriptedActionRecorderActions(this);
        public interface IScriptedActionRecorderActions
        {
            void OnAddScriptedAction(InputAction.CallbackContext context);
            void OnRemoveScriptedAction(InputAction.CallbackContext context);
            void OnVariateIndex(InputAction.CallbackContext context);
            void OnLookAtDistance(InputAction.CallbackContext context);
            void OnAreaDistance(InputAction.CallbackContext context);
            void OnAction1(InputAction.CallbackContext context);
            void OnAction2(InputAction.CallbackContext context);
            void OnAction3(InputAction.CallbackContext context);
            void OnUpdateCurrent(InputAction.CallbackContext context);
        }
    }
}
