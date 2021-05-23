using AIEssentials;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace KinematicEssentials
{
    [CreateAssetMenu(fileName = "N - KinematicDelta [Variable]",
        menuName = "Variable/KineaticDelta")]
    public class KinematicDeltaVariable : ScriptableObject
    {
        public BreakableAcceleration Acceleration = new BreakableAcceleration(2,8);
        public float AngularSpeed = 2f;

        [SerializeField, SuffixLabel("%/Meter")]
        private AnimationCurve _velocityByDistance = new AnimationCurve(
            new Keyframe(0,0),new Keyframe(1,1.5f));

        public Vector3 CalculateDeltaVelocity(Vector3 currentVelocity,Vector3 targetVelocity)
        {
            Vector3 calculatedVelocity = Vector3.Lerp(currentVelocity, targetVelocity,
                Time.deltaTime * Acceleration.DeltaModifier(currentVelocity,targetVelocity));

            return calculatedVelocity;
        }

        public Vector3 CalculateDeltaVelocity(Vector3 currentVelocity, Vector3 targetVelocity,
            float distanceOfDestination)
        {
            targetVelocity *= _velocityByDistance.Evaluate(distanceOfDestination); 
            return CalculateDeltaVelocity(currentVelocity, targetVelocity);
        }

        public Vector3 CalculateForwardRotation(Vector3 currentForward, Vector3 targetForward)
        {
            return Vector3.Slerp(currentForward,targetForward, Time.deltaTime * AngularSpeed);
        }
    }
}
