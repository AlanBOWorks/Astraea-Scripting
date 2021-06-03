using UnityEngine;

namespace KinematicEssentials
{

    public class OnStopFilterVelocity : IKinematicVelocityFilter, IShortMovementEvent
    {
        protected bool HasStopped = false;
        private readonly float _startingAcceleration;

        public OnStopFilterVelocity(float startingAcceleration = 1)
        {
            _startingAcceleration = startingAcceleration;
        }

        public Vector3 FilterVelocity(Vector3 currentVelocity, Vector3 desiredVelocity)
        {
            return HasStopped 
                ? Vector3.Lerp(
                    currentVelocity,
                    desiredVelocity, 
                    Time.deltaTime * _startingAcceleration)
                : desiredVelocity;
        }


        public void SwitchToMoveState(float currentSpeed)
        {
            HasStopped = false;
        }

        public void SwitchToStopState(float currentSpeed)
        {
            HasStopped = true;
        }
    }

    public class OnStopFilterRotation : IKinematicRotationFilter
    {
        protected readonly KinematicData KinematicData;
        protected bool StoppingRotation;
        public float AngleThreshold;
        public float AngularSpeed;

        public OnStopFilterRotation(KinematicData kinematicData,float angleThreshold = 30f, float angularSpeed = 2f)
        {
            KinematicData = kinematicData;
            AngleThreshold = angleThreshold;
            StoppingRotation = false;
            AngularSpeed = angularSpeed;
        }

        public Quaternion FilterRotation(Quaternion currentRotation, Quaternion desiredRotation)
        {
            if (KinematicData.CurrentSpeed > 0) return desiredRotation;

            float targetAngle = Quaternion.Angle(currentRotation, desiredRotation);
            bool isSmallAngle = (targetAngle < AngleThreshold);

            if (StoppingRotation || isSmallAngle) return currentRotation;

            StoppingRotation = targetAngle < 0.01f;
            return Quaternion.Slerp(currentRotation,desiredRotation, Time.deltaTime * AngularSpeed);


        }
    }
}
