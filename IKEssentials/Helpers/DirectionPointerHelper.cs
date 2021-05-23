using System.Collections.Generic;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class DirectionPointerHelper : MonoBehaviour
    {
        [Title("Params")] 
        [Range(0,10)]
        public float ArmLength = .5f;

        [Range(0, 10)] 
        public float MinPointLength = .05f;

        [Range(0, 1)] public float DirectionNormalizedStrength = .2f;

        [Button]
        private void TestPosition(Transform targetPoint, Transform moveTransform)
        {

            Timing.RunCoroutine(_doTest());
            IEnumerator<float> _doTest()
            {
                while (true)
                {
                    yield return Timing.WaitForOneFrame;
                    moveTransform.position = PositionByGlobalDirection(targetPoint.position - transform.position);
                }
            }
        }


        private Vector3 PositionByTarget(Transform target)
        {
            Vector3 direction = transform.InverseTransformPoint(target.position);
            return ClampedPointWorld(direction);
        }

        private Vector3 PositionByGlobalDirection(Vector3 direction)
        {
            direction = transform.InverseTransformDirection(direction);

            return ClampedPointWorld(direction);
        }

        private Vector3 ClampedPointWorld(Vector3 direction)
        {
            return transform.TransformPoint(ClampLocalDirection(direction));
        }
        private Vector3 ClampLocalDirection(Vector3 direction)
        {
            direction.y = Mathf.Max(MinPointLength, direction.y);
            Vector3 normalizedDirection = direction.normalized * ArmLength;
            direction = Vector3.ClampMagnitude(direction,ArmLength);

            return Vector3.LerpUnclamped(direction,normalizedDirection,DirectionNormalizedStrength);
        }

#if UNITY_EDITOR

        [Button, InfoBox("The Y axis will be used as a center for the calculations;\n" +
                 "It's recommendable to set the Y axis pointing not perpendicular to the torso but a little forward [eg: 30º]")]
        private void ReLocateToShoulder(Transform upperArmReference ,Vector3 rotationOffset)
        {
            transform.position = upperArmReference.position;
            transform.rotation = upperArmReference.rotation * Quaternion.Euler(rotationOffset);
        }
#endif

    }
}
