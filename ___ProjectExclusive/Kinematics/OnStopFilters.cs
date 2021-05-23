using UnityEngine;

namespace KinematicEssentials
{

    public class OnStopFilterVelocity : IKinematicVelocityFilter, IShortMovementEvent
    {
        protected readonly IKinematicMotorHandler MotorHandler;
        protected bool HasStopped = false;
        private readonly float _startingAcceleration;

        public OnStopFilterVelocity(IKinematicMotorHandler motorHandler,float startingAcceleration = 1)
        {
            MotorHandler = motorHandler;
            _startingAcceleration = startingAcceleration;
        }

        public Vector3 FilterVelocity(Vector3 desiredVelocity)
        {
            return HasStopped 
                ? Vector3.Lerp(
                    MotorHandler.DesiredVelocity,
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

        public OnStopFilterRotation(KinematicData kinematicData,float angleThreshold, float angularSpeed = 2f)
        {
            KinematicData = kinematicData;
            AngleThreshold = angleThreshold;
            StoppingRotation = false;
            AngularSpeed = angularSpeed;
        }

        public Vector3 FilterRotation(Vector3 desiredRotation)
        {
            if (KinematicData.CurrentSpeed > 0) return desiredRotation;

            Vector3 currentForward = KinematicData.CurrentRotationForward;
            float targetAngle = Vector3.Angle(currentForward, desiredRotation);
            bool isSmallAngle = (targetAngle < AngleThreshold);

            if (StoppingRotation || isSmallAngle) return currentForward;

            StoppingRotation = targetAngle < 0.01f;
            return Vector3.Lerp(currentForward,desiredRotation, Time.deltaTime * AngularSpeed);


        }
    }
}
