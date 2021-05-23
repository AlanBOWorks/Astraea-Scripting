using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerEssentials
{
    [Serializable]
    public class PlayerInputHandler : ITicker
    {

        [Title("Mouse -Input Actions")]
        [SerializeField, HideInPlayMode] private InputActionReference _lookAroundAction = null;


        [Title("Keyboard -Input Actions")]
        [SerializeField, HideInPlayMode] private InputActionReference _movementAction = null;
        [SerializeField, HideInPlayMode] private InputActionReference _sprintAction = null;
        [SerializeField, HideInPlayMode] private InputActionReference _onHoldHandAction = null;

        public InputActionReference HoldHandInputAction()
        {
            return _onHoldHandAction;
        }

        [SerializeField,HideInEditorMode]
        private PlayerInputData _inputData = new PlayerInputData();
        public PlayerInputData InputData => _inputData;

        private IPlayerTransformData _transformData;

        private bool _isFullInitialized = false;
        public void Injection(IPlayerTransformData transformData)
        {
            _transformData = transformData;
            if(!_isFullInitialized) FullInitialization();
        }

        private void FullInitialization()
        {
            SubscribeMouseListener();
            SubscribeKeyboardListener();
            _movementAction = null;

            _isFullInitialized = true;

            void SubscribeMouseListener()
            {
                _lookAroundAction.action.performed += LookAtInputUpdate;
                _lookAroundAction.action.canceled += LookAtInputUpdate;
            }
            void SubscribeKeyboardListener()
            {
                _movementAction.action.performed += TranslationInputUpdate;
                _movementAction.action.canceled += TranslationInputUpdate;

                _sprintAction.action.performed += SprintPush;
                _sprintAction.action.canceled += SprintRelease;

                void SprintPush(InputAction.CallbackContext context)
                {
                    _inputData.UpdateIsSprintPress(true);
                }

                void SprintRelease(InputAction.CallbackContext context)
                {
                    _inputData.UpdateIsSprintPress(false);
                }
            }
        }

        public bool Disabled { get; set; }

        public void Tick()
        {
            UpdateLookAtData();
            UpdateMovementData();
        }


        // ___________ MOUSE ___________________
        private Vector2 _lookAtInput;
        private void LookAtInputUpdate(InputAction.CallbackContext context)
        {
            _lookAtInput = context.ReadValue<Vector2>();
        }

        [Title("Params")] 
        [Range(0,4f)]
        public float LookAtSensitivity = .3f;
        public float LookAtChangeSpeed = 12f;
        private void UpdateLookAtData()
        {
            Vector2 deltaChange = Vector2.Lerp(_inputData.LookDeltaAxis,_lookAtInput * LookAtSensitivity, Time.deltaTime * LookAtChangeSpeed);
            _inputData.UpdateLookAtAxis(deltaChange);
        }

        // ___________ KEYBOARD ___________________
        private Vector2 _moveInput;
        private void UpdateMovementData()
        {
            Vector3 cameraForward = _transformData.CameraForward;
            Vector3 cameraRight = _transformData.CameraRight;

            cameraForward = Vector3.ProjectOnPlane(cameraForward, _transformData.MeshUp);
            cameraForward.Normalize();
            Vector3 cameraVelocity = _moveInput.y * cameraForward
                                      + _moveInput.x * cameraRight;
            cameraVelocity = Vector3.ClampMagnitude(cameraVelocity, 1);

            _inputData.UpdateGlobalDirection(cameraVelocity);

            _inputData.UpdateIsMoving(_inputData.GlobalDesiredDirection.magnitude > 0);
            _inputData.UpdateKeyboardDirection(_moveInput);
        }


        public void TranslationInputUpdate(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }
    }



}
