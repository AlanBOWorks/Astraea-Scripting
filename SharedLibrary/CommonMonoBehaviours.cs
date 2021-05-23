using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    public class MonoReferencer<T> : MonoBehaviour
    {
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public T Reference { get; private set; }

        public void InjectReference(T reference)
        {
            Reference = reference;
        }
    }

    public class DestroyableMonoReferencer<T> : MonoReferencer<T>
    {
        private void LateUpdate()
        {
            Destroy(this);
        }
    }

    public abstract class DestroyableMonoReferencerHolder<T> : MonoBehaviour
    {
        protected abstract ScriptableReferencer<T> GetScriptableReferencer();
        protected abstract T GetReference();

        protected virtual void Awake()
        {
            GetScriptableReferencer().InjectReference(GetReference());
        }

        private void LateUpdate()
        {
            Destroy(this);
        }
    }

}
