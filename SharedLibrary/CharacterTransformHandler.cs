using System;
using ___ProjectExclusive;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    [Serializable]
    public class CharacterTransformHandler : ITicker
    {
        [SerializeField]
        private CharacterTransform _characterTransform = new CharacterTransform();
        public ICharacterTransformData CharacterTransform => _characterTransform;

        public bool Disabled { get; set; }
        public void Tick()
        {
           _characterTransform.Update();
        }
    }

    [Serializable]
    public class CharacterTransform : ICharacterTransformData
    {
        [SerializeField] private Transform _root = null;
        [SerializeField] private Transform _meshRoot = null;
        [SerializeField] private Transform _head = null;
        [SerializeField] private Transform _pelvis = null;

        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 MeshWorldPosition { get; private set; }

        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 MeshForward { get; private set; }

        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 MeshUp { get; private set; }
        [ShowInInspector,DisableInPlayMode,HideInEditorMode]
        public Vector3 MeshRight { get; private set; }


        public Transform Root => _root;
        public Transform MeshRoot => _meshRoot;
        public Transform Head => _head;
        public Vector3 HeadPosition { get; private set; }
        public Transform Pelvis => _pelvis;
        public Vector3 PelvisPosition { get; private set; }

        public virtual void Update()
        {
            MeshWorldPosition = _meshRoot.position;
            MeshForward = _meshRoot.forward;
            MeshUp = _meshRoot.up;
            MeshRight = _meshRoot.right;

            HeadPosition = _head.position;
            PelvisPosition = _pelvis.position;
        }
    }


}
