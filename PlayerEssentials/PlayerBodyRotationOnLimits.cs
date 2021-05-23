using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerEssentials
{

    public class PlayerBodyRotationOnLimits : MonoBehaviour
    {
        private IPlayerTransformData _playerTransform;
        private IInputMovement _inputMovement;
        
        [Title("Params")]
        [Range(-1,1)] public float AngleDotThreshold = -.3f;
        public AnimationCurve RotationSpeedByAngle = new AnimationCurve(
            new Keyframe(0,2),new Keyframe(180f,20f));

        public void Injection(IPlayerTransformData transformData, IInputMovement inputMovement)
        {
            _playerTransform = transformData;
            _inputMovement = inputMovement;
        }

        private CoroutineHandle _angleHandle;
        private CoroutineHandle _movementHandle;

        private void Start()
        {
            AngleLogic();
            MovementLogic();
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_angleHandle);
            Timing.ResumeCoroutines(_movementHandle);
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_angleHandle);
            Timing.PauseCoroutines(_movementHandle);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_angleHandle);
            Timing.KillCoroutines(_movementHandle);
        }

        private void AngleLogic()
        {
            // Coroutines instead input's events mainly because the body rotation could be moved through
            // animation or external forces
            _angleHandle = Timing.RunCoroutine(_starChecking());
            IEnumerator<float> _starChecking()
            {

            yield return Timing.WaitForOneFrame;

                while (IsInRange())
                {
                    yield return Timing.WaitForOneFrame;
                }

                _angleHandle = Timing.RunCoroutine(_LerpToRotation()); // Switching Loop
            }

            IEnumerator<float> _LerpToRotation()
            {
                while (DotValue() < .9f)
                {
                    LerpRotation();
                    yield return Timing.WaitForOneFrame;
                }
                _angleHandle = Timing.RunCoroutine(_starChecking()); // Switching Loop
            }


            bool IsInRange()
            {
                return DotValue() > AngleDotThreshold;
            }

            float DotValue()
            {
                return Vector3.Dot(transform.right, _playerTransform.CameraRight); ;
            }
        }

        private void MovementLogic()
        {
            _movementHandle = Timing.RunCoroutine(_CheckMovement());

            IEnumerator<float> _CheckMovement()
            {
                yield return Timing.WaitForOneFrame;

                while (!IsMoving())
                {
                    yield return Timing.WaitForOneFrame;
                }
                Timing.PauseCoroutines(_angleHandle);
                _movementHandle = Timing.RunCoroutine(_CheckStop()); // Switching Loop

            }

            IEnumerator<float> _CheckStop()
            {
                while (IsMoving())
                {
                    LerpRotation();
                    yield return Timing.WaitForOneFrame;
                }
                Timing.ResumeCoroutines(_angleHandle);
                _movementHandle = Timing.RunCoroutine(_CheckMovement()); // Switching Loop
            }

            bool IsMoving()
            {
                return _inputMovement.IsMoving;
            }

        }

        private void LerpRotation()
        {
            Quaternion currentRotation = transform.rotation;
            Vector3 targetDirection = Vector3.ProjectOnPlane(_playerTransform.CameraForward,_playerTransform.MeshUp);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, _playerTransform.MeshUp);

            float deltaVariation = RotationSpeedByAngle.Evaluate(Quaternion.Angle(currentRotation, targetRotation));

            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * deltaVariation);
        }
    }
}
