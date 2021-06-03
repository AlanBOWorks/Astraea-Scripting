using System;
using UnityEngine;

namespace KinematicEssentials
{
    [Serializable]
    public class CurveRotationFilter : IKinematicRotationFilter
    {
        public AnimationCurve rotationSpeedByAngle = new AnimationCurve(
            new Keyframe(0, 2), new Keyframe(180f, 20f));


        public Quaternion FilterRotation(Quaternion currentRotation, Quaternion desiredRotation)
        {

            float deltaVariation = rotationSpeedByAngle.Evaluate(Quaternion.Angle(currentRotation, desiredRotation));

            return Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * deltaVariation);
        }
    }
}
