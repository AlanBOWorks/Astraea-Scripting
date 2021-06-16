using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public class TorchHoldersTransform : ITorchStructure<Transform>
    {
        [ShowInInspector]
        public Transform Original { get; set; } = null;
        [ShowInInspector]
        public Transform Procedural { get; set; } = null;

        [ShowInInspector]
        public Transform Animation { get; set; } = null;

        [ShowInInspector]
        public Transform UnEquip { get; set; } = null;

        public TorchHoldersTransform(ITorchStructure<UTorchReferences> references)
        {
            Original = DoInjection(references.Original);
            Procedural = DoInjection(references.Procedural);
            Animation = DoInjection(references.Animation);
            UnEquip = DoInjection(references.UnEquip);

            Transform DoInjection(UTorchReferences reference)
            {
                return reference is null ? null : reference.Holder;
            }
        }
    }

    [Serializable]
    public class TorchHoldersReferences : ITorchStructure<UTorchReferences>
    {
        [SerializeField] private UTorchReferences _originalHolder = null;
        [SerializeField] private UTorchReferences _procedural = null;
        [SerializeField] private UTorchReferences _animationHolder = null;
        [SerializeField] private UTorchReferences _rigidBodyHolder = null;

        public UTorchReferences Original
        {
            get => _originalHolder;
            set => _originalHolder = value;
        }

        public UTorchReferences Procedural
        {
            get => _procedural;
            set => _procedural = value;
        }

        public UTorchReferences Animation
        {
            get => _animationHolder;
            set => _animationHolder = value;
        }

        public UTorchReferences UnEquip
        {
            get => _rigidBodyHolder;
            set => _rigidBodyHolder = value;
        }
    }
}
