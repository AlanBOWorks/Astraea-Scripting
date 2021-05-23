using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerEssentials
{
    [Serializable]
    public class InputRotation : ITicker
    {
        [Title("References")]
        [InfoBox("Normally the camera")]
        public Transform RotateTransform = null;


        public void Injection(ICameraParameters parameters,ICharacterTransformData transform,IInputLookAround lookAround)
        {
            _parameters = parameters;
            _playerTransformData = transform;
            _lookAround = lookAround;
        }

        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        private ICameraParameters _parameters;
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        private ICharacterTransformData _playerTransformData;
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        private IInputLookAround _lookAround;
        public bool Disabled { get; set; }

        public void Tick()
        {
            Transform upReference = _playerTransformData.Root;

            Vector2 axisDirection = _lookAround.LookDeltaAxis;
            Vector3 rotationForward = RotateTransform.forward;

            Vector3 referencedForward = upReference.InverseTransformDirection(rotationForward);
            referencedForward.y
                = Mathf.Clamp(referencedForward.y + axisDirection.y, -1, 1);
            referencedForward = upReference.TransformDirection(referencedForward);

            Vector3 horizontalDirection = RotateTransform.TransformDirection(axisDirection.x, 0, 0);

            Vector3 targetDirection = horizontalDirection + referencedForward;

            float angle = Vector2.Angle(Vector2.zero, axisDirection);
            float deltaVariation = _parameters.CalculateAngleDelta(angle);

            targetDirection = Vector3.Slerp(rotationForward, targetDirection, deltaVariation * Time.deltaTime);

            RotateTransform.rotation = Quaternion.LookRotation(targetDirection,upReference.up);
        }
    }
}
