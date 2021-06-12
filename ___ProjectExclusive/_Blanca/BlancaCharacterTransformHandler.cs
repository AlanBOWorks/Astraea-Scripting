using System;
using ___ProjectExclusive;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
    [Serializable]
    public class BlancaTransformHandler : ITicker
    {
        [SerializeField]
        private BlancaTransform characterTransform = new BlancaTransform();
        public ICharacterTransformData CharacterTransform => characterTransform;

        public bool Disabled { get; set; }
        public void Tick()
        {
            characterTransform.Update();
        }

        [Serializable]
        internal class BlancaTransform : CharacterTransform
        {
            
        }
    }

    
}
