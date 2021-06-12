using System;
using UnityEngine;

namespace KinematicEssentials
{
    [Serializable]
    public class CurveRotationFilter : IKinematicRotationFilter
    {
        public AnimationCurve rotationSpeedByAngle = new AnimationCurve(
            new Keyframe(0, 2), new Keyframe(180f, 20f));


        public Quaternion FilterRotation(MotorFilteredRotation rotation)
        {
            Quaternion lastFilterRotation = rotation.LastFilteredRotation;
            Quaternion desiredRotation = rotation.DesiredRotation;
            float deltaVariation 
                = rotationSpeedByAngle.Evaluate(Quaternion.Angle(lastFilterRotation, desiredRotation));

            return Quaternion.Lerp(lastFilterRotation, desiredRotation, Time.deltaTime * deltaVariation);
        }
    }

    [Serializable]
    public class CurveVelocityFilter : IKinematicVelocityFilter
    {

        public AnimationCurve moduleSpeedByMagnitude = new AnimationCurve(
            new Keyframe(0.2f,0), new Keyframe(.4f,1));

        public Vector3 FilterVelocity(MotorFilteredVelocity velocity)
        {
            Vector3 lastFilterVelocity = velocity.LastFilteredVelocity;
            float magnitude = lastFilterVelocity.magnitude;

            return lastFilterVelocity * moduleSpeedByMagnitude.Evaluate(magnitude);
        }
    }
}
