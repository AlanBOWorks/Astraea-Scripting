using System;
using ___ProjectExclusive;
using PlayerEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerTransformHandler : ITicker
    {
        [SerializeField]
        private PlayerTransform _playerTransform = new PlayerTransform();
        public IPlayerTransformData CharacterTransform => _playerTransform;

        public bool Disabled { get; set; }
        public void Tick()
        {
            _playerTransform.Update();
        }
    }

    [Serializable]
    public class PlayerTransform : CharacterTransform, IPlayerTransformData
    {
        [SerializeField] private Camera _camera = null;
        [SerializeField] private Transform _lookAtPoint = null;
        public Camera PlayerCamera => _camera;
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 CameraForward { get; private set; }
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 CameraRight { get; private set; }
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 CameraUp { get; private set; }
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 CameraProjectedForward { get; private set; }

        public Transform GetLookAtPoint() => _lookAtPoint;


        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 CameraPlanarForward { get; private set; }

        public override void Update()
        {
            base.Update();
            Transform cameraTransform = _camera.transform;
            CameraForward = cameraTransform.forward;
            CameraRight = cameraTransform.right;
            CameraUp = cameraTransform.up;

            CameraProjectedForward = Vector3.ProjectOnPlane(CameraForward,MeshUp);
            CameraPlanarForward = CameraProjectedForward.normalized;


            //TODO make also the leftHand 
        }
    }

}
