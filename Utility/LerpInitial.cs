using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace Utility
{
    public class LerpInitial : MonoBehaviour
    {
        [Range(0f,100f)]
        public float PositionLerpForce = 10f;
        [Range(0f,100f)]
        public float RotationLerpForce = 10f;

        [SerializeField] private bool _saveInitialPosition = true;
        [SerializeField] private bool _saveInitialRotation = true;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            if(_saveInitialPosition)
                _initialPosition = transform.localPosition;
            if(_saveInitialRotation)
                _initialRotation = transform.localRotation;
        }

        private void LateUpdate()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,_initialPosition, Time.deltaTime * PositionLerpForce);
            transform.localRotation = Quaternion.Lerp(transform.localRotation,_initialRotation, Time.deltaTime * RotationLerpForce);
        }
    }
}
