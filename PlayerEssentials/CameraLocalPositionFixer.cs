using UnityEngine;

namespace PlayerEssentials
{
    public class CameraLocalPositionFixer : MonoBehaviour
    {
        [SerializeField] private Transform _copyPosition = null;

        public float LerpForce = 10f;

        public bool IgnoreXZ = true;

        private void LateUpdate()
        {
            Transform parent = transform.parent;
            Vector3 targetLocal = parent.InverseTransformPoint(_copyPosition.position);

            if (IgnoreXZ)
            {
                targetLocal.x = 0;
                targetLocal.z = 0;
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocal, Time.deltaTime * LerpForce);
        }
    }
}
