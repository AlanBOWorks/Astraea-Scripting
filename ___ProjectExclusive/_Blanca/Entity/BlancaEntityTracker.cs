using System;
using AIEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    /// <summary>
    /// Used to keep track all data of Blanca and Debuging her Status from the Singleton[<seealso cref="BlancaEntitySingleton"/>]
    /// </summary>
    public class BlancaEntityTracker : MonoBehaviour, ISerializationCallbackReceiver
    {

        [TabGroup("Tasks")] 
        [SerializeField] private BlancaTasksDebugger _tasks = new BlancaTasksDebugger();

        [TabGroup("Entity"), PropertyOrder(-100)]
        [ShowInInspector,HideInInlineEditors,InlineProperty, HideInPlayMode]
        public BlancaEntitySingleton Singleton;

        [TabGroup("Entity"), NonSerialized, ShowInInspector, HideInEditorMode]
        public BlancaEntity Entity;

        public void Awake()
        {
            Singleton = BlancaEntitySingleton.Instance;
            Entity = BlancaEntitySingleton.Instance.Entity;
        }

        public void OnBeforeSerialize()
        {
            Singleton = BlancaEntitySingleton.Instance;
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
