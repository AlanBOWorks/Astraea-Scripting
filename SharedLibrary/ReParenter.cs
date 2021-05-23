using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    public class ReParenter : MonoBehaviour
    {
        public Transform OnTransforms = null;
        public TransformReferencer TargetParent = null;
        public Transform DestroyAfter = null;

        public bool ChangePosition = true;
        [ShowIf("ChangePosition")]
        public Vector3 LocalPosition;

        public bool ChangeRotation = true;
        [ShowIf("ChangeRotation")]
        public Vector3 LocalRotation;


        // Start is called before the first frame update
        void Start()
        {
            OnTransforms.parent = TargetParent.Reference;
            

            if (ChangePosition)
                OnTransforms.localPosition = LocalPosition;
            if(ChangeRotation)
                OnTransforms.localRotation = Quaternion.Euler(LocalRotation);

            if (DestroyAfter != null)
                Destroy(DestroyAfter.gameObject);
            Destroy(this);
        }
    }
}
