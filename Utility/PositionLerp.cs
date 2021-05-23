using SharedLibrary;
using UnityEngine;

namespace Utility
{
    public class PositionLerp : MonoBehaviour
    {
        [SerializeReference] private TransformReferencer _minLerpReference = null;
        [SerializeReference] private TransformReferencer _maxLerpReference = null;

        [Range(0,1)]
        public float LerpAmount = .5f;

        private void Update()
        {
            transform.position = Vector3.LerpUnclamped(
                _minLerpReference.Reference.position,
                _maxLerpReference.Reference.position,
                LerpAmount);
        }
    }
}
