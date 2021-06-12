using System;
using UnityEngine;

namespace SharedLibrary
{
    /// <summary>
    /// This is made so the sequence of Injections are made in a correct pattern (during the Awake)
    /// </summary>
    public class MonoInjectorSequencer : MonoBehaviour
    {
        [SerializeField] private MonoInjector[] _sequenceOfInjections = new MonoInjector[0];

        public bool destroyGObjectAfter = false;
        public bool destroyElementsAfter = false;

        private void Awake()
        {
            foreach (MonoInjector injector in _sequenceOfInjections)
            {
                injector.DoInjection();
                if(destroyElementsAfter) Destroy(injector);
            }

        }

        private void Start()
        {
            if(destroyGObjectAfter)
                Destroy(gameObject);
            else
            {
                Destroy(this);
            }
        }
    }

    public abstract class MonoInjector : MonoBehaviour
    {
        public abstract void DoInjection();
    }
}
