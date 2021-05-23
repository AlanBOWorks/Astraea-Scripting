using UnityEngine;

namespace Utility
{
    public class OffsetHelper : MonoBehaviour
    {
        public Vector3 RelativeOffset(Vector3 direction, Vector3 offset)
        {
            return RelativeOffset(direction, offset, transform);
        }

        public static Vector3 RelativeOffset(Vector3 direction, Vector3 offset, Transform helper)
        {
            helper.forward = direction;

            return helper.TransformDirection(offset);
        }

    }
}
