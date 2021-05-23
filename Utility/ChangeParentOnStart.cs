 using Sirenix.OdinInspector;
 using UnityEngine;

namespace Utility
{
    public class ChangeParentOnStart : MonoBehaviour
    {
        public Transform change;
        public Transform parent;

        [ShowIf("ChangePosition")]
        public Vector3 localPositionOnChange;

        public bool ChangePosition = true;

        // Start is called before the first frame update
        void Start()
        {
            change.parent = parent;
            if (ChangePosition)
            {
                change.localPosition = localPositionOnChange;
            }
            Destroy(this);
        }

        
    }
}
