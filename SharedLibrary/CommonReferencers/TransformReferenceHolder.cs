using UnityEngine;

namespace SharedLibrary
{
    public class TransformReferenceHolder : DestroyableMonoReferencer<Transform>
    {
        public TransformReferencer TransformReferencer = null;

        private void Awake()
        {
            TransformReferencer.InjectReference(Reference ?? transform);
        }
    }
}
