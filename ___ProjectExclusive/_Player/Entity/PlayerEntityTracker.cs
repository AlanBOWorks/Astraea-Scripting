using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerEntityTracker : MonoBehaviour, ISerializationCallbackReceiver
    {
        [TabGroup("Entity"), PropertyOrder(-100)]
        [ShowInInspector, HideInInlineEditors, InlineProperty, HideInPlayMode]
        public PlayerEntitySingleton Singleton;

        [TabGroup("Entity"), NonSerialized, ShowInInspector, HideInEditorMode]
        public PlayerEntity Entity;

        public void Awake()
        {
            Singleton = PlayerEntitySingleton.Instance;
            Entity = PlayerEntitySingleton.Instance.Entity;
        }

        public void OnBeforeSerialize()
        {
            Singleton = PlayerEntitySingleton.Instance;
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
