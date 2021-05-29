using System;
using AIEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    /// <summary>
    /// Used to keep track all data of Blanca and Debugging her Status from the Singleton[<seealso cref="BlancaEntitySingleton"/>]
    /// </summary>
    public class BlancaEntityDebugger : MonoBehaviour
    {
#if UNITY_EDITOR

        [TabGroup("Entity"), NonSerialized, ShowInInspector, HideInEditorMode]
        public BlancaEntity Entity;

        public void Awake()
        {
            Entity = BlancaEntitySingleton.Instance.Entity;
        }

#else
        public void Awake()
        {
            Destroy(this);
        }


#endif
    }
}
