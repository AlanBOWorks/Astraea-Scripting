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

        [NonSerialized, ShowInInspector, HideInEditorMode]
        public BlancaEntity Entity;

        [NonSerialized, ShowInInspector, HideInEditorMode]
        public BlancaPropsEntity PropsEntity;

        public void Awake()
        {
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
            Entity = singleton.Entity;
            PropsEntity = singleton.Props;
        }

#else
        public void Awake()
        {
            Destroy(this);
        }


#endif
    }
}
