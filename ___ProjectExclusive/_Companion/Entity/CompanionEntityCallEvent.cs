using System;
using UnityEngine;

namespace Companion
{
    [Serializable]
    public class CompanionEntityCallEvent 
    {
        [SerializeField] 
            private CompanionEntityScriptableBehaviour _behaviourHolder = null;
        
        // Start is called before the first frame update
        public void DoCall()
        {
            _behaviourHolder.CallForCompanionEntityInitialization();

        }

    }
}
