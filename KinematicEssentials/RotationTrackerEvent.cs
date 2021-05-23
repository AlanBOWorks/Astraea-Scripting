using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KinematicEssentials
{
    public class RotationTrackerEvent
    {
        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private IKinematicRotation _trackingRotation;
        private IKinematicMotorHandler _motor;

        [Title("Params")]
        [Range(-1, 1)] public float AngleDotThreshold = -.3f;

        private CoroutineHandle _angleHandle;

        public RotationTrackerEvent(IKinematicMotorHandler motor, IKinematicRotation trackingRotation)
        {
            _trackingRotation = trackingRotation;
            _motor = motor;

            AngleLogic();
        }

        private void AngleLogic()
        {
            // Coroutines instead input's events mainly because the body rotation could be moved through
            // animation or external forces
            _angleHandle = Timing.RunCoroutine(_WaitUntilBigRotation());
            IEnumerator<float> _WaitUntilBigRotation()
            {

                yield return Timing.WaitForOneFrame;

                while (IsInRange())
                {
                    yield return Timing.WaitForOneFrame;
                }

                _angleHandle = Timing.RunCoroutine(_WaitUntilRotationsIsSmall()); // Switching Loop
            }

            IEnumerator<float> _WaitUntilRotationsIsSmall()
            {
                while (DotValue() < .9f)
                {
                    yield return Timing.WaitForOneFrame;
                }
                _angleHandle = Timing.RunCoroutine(_WaitUntilBigRotation()); // Switching Loop
            }


            bool IsInRange()
            {
                return DotValue() > AngleDotThreshold;
            }

            float DotValue()
            {
                return Vector3.Dot(_trackingRotation.CurrentRotationForward, _trackingRotation.DesiredGlobalRotationForward); 
            }
        }
    }

    public interface IRotationTriggerEvent
    {
        void InvokeRotation(float dotAngle);
    }
}
