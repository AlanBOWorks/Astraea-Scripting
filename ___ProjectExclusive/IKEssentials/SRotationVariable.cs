using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    [CreateAssetMenu(fileName = "Rotations - N [Variable]",
        menuName = "Variable/Rotations")]
    public class SRotationVariable : ScriptableObject
    {
        [SerializeField,SuffixLabel("%,Meters")]
        private AnimationCurve _rotationTowardsPoint
            = new AnimationCurve(new Keyframe(1f, .5f), new Keyframe(3f, .8f));
        [SerializeField,SuffixLabel("%,Meters")]
        private AnimationCurve _lookAtWeight
            = new AnimationCurve(new Keyframe(1f, .8f), new Keyframe(3f, .2f));

        public float EvaluateRotation(float distance)
        {
            return _rotationTowardsPoint.Evaluate(distance);
        }

        public float EvaluateLookAt(float distance)
        {
            return _lookAtWeight.Evaluate(distance);
        }
    }
}
