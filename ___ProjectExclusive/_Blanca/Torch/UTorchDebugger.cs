using System;
using AIEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    /// <summary>
    /// Used to keep track all data of Torch and Debugging her Status from the Singleton[<seealso cref="BlancaEntitySingleton"/>]
    /// </summary>
    public class UTorchDebugger : MonoBehaviour
    {
#if UNITY_EDITOR


        [NonSerialized, ShowInInspector, HideInEditorMode]
        public BlancaPropsEntity PropsEntity;

        public void Awake()
        {
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
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