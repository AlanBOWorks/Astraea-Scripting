using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class HoldHandPosition : MonoBehaviour
    {
        [Title("References")]
        [SerializeReference] private Transform _handReference = null;
        [SerializeReference] private TransformReferencer  _companionPointReference = null;

        [Title("Param")] 
        [Range(0.01f,2f)]public float HoldDistance = .2f;

        public Vector3 Point { get; private set; }

        private void Update()
        {
            Point = Vector3.MoveTowards(  _handReference.position, _companionPointReference.Reference.position, HoldDistance);



            transform.position = Point;
        }

    }
}
