using System;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class HandChainHelper : MonoBehaviour
    {
        [SerializeField, HideInPlayMode] private FullBodyBipedIK _biped = null;

        [SerializeField] private bool _isLeftHand = false;
        private void Start()
        {
            _pelvis = _biped.references.pelvis;

            _handReference = _isLeftHand
                ? _biped.references.leftHand 
                : _biped.references.rightHand;


            _biped = null;
        }



        [Tooltip("[Local Vector] Normally the opposite direction of the thumb while having the palm open")]
        [SerializeField] private Vector3 _handJointExteriorVector = new Vector3(0, 0, -1f);

        [Tooltip("Using an horizontal offset it could make the helper bend more towards to the lateral")]
        [SerializeField] private Vector3 _pelvisOffsetDirection = Vector3.zero;

        private Transform _handReference;
        private Transform _pelvis = null;
        private void LateUpdate()
        {
            Vector3 directionFromReference = _handReference.TransformDirection(_handJointExteriorVector);
            transform.position = _pelvis.TransformPoint(_pelvisOffsetDirection) + directionFromReference;

        }
    }
}
