using SMaths;
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

        public Vector3 FilterVelocity(MotorFilteredVelocity velocity)
        {
            return HasStopped 
                ? Vector3.Lerp(
                    velocity.LastFilteredVelocity,
                    Vector3.zero, 
                    Time.deltaTime * _startingAcceleration)
                : velocity.LastFilteredVelocity;
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

        public OnStopFilterRotation(KinematicData kinematicData)
        {
            KinematicData = kinematicData;
        }

        public Quaternion FilterRotation(MotorFilteredRotation rotation)
        {
            Quaternion lastFilterRotation = rotation.LastFilteredRotation;
            if (KinematicData.CurrentSpeed > 0) return lastFilterRotation;
            Quaternion originalRotation = rotation.CurrentRotation;
            return originalRotation;
        }
    }

    public class SmoothRotationFilter : IKinematicRotationFilter
    {
        public float FilterSpeed;

        public SmoothRotationFilter(float filterSpeed = 4)
        {
            FilterSpeed = filterSpeed;
        }

        public Quaternion FilterRotation(MotorFilteredRotation rotation)
        {
            return Quaternion.Lerp(rotation.CurrentRotation,rotation.LastFilteredRotation,
                Time.deltaTime * FilterSpeed);
        }
    }


    public class SmoothVelocityBreakFilter : IKinematicVelocityFilter
    {
        public float BreakSpeed;
        public SmoothVelocityBreakFilter(float breakSpeed = 4f)
        {
            BreakSpeed = breakSpeed;
        }

        public Vector3 FilterVelocity(MotorFilteredVelocity velocity)
        {
            Vector3 filterVelocity = velocity.LastFilteredVelocity;
            return filterVelocity.sqrMagnitude > .0001f 
                ? filterVelocity 
                : Vector3.Lerp(velocity.CurrentVelocity,Vector3.zero, Time.deltaTime * BreakSpeed);
        }
    }

    public class SmoothVelocityFilter : IKinematicVelocityFilter
    {
        public float FilterSpeed;

        public SmoothVelocityFilter(float filterSpeed)
        {
            FilterSpeed = filterSpeed;
        }

        public Vector3 FilterVelocity(MotorFilteredVelocity velocity)
        {
            return Vector3.Lerp(velocity.CurrentVelocity,velocity.LastFilteredVelocity, 
                Time.deltaTime * FilterSpeed);
        }
    }

    public class RoundVelocityFilter : IKinematicVelocityFilter
    {
        protected float Threshold;
        public RoundVelocityFilter(float threshold)
        {
            Threshold = threshold;
        }

        public Vector3 FilterVelocity(MotorFilteredVelocity velocity)
        {
            Vector3 filterVelocity = SVector3.RoundVector3(velocity.LastFilteredVelocity, 100);
            return filterVelocity.sqrMagnitude < Threshold * Threshold 
                ? Vector3.zero 
                : filterVelocity;
        }
    }

}
