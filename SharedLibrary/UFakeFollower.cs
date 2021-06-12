using System.Collections.Generic;
using UnityEngine;

namespace SharedLibrary
{
    public class UFakeFollower : MonoBehaviour
    {
        [SerializeField] private Transform followThis = null;
        private Transform _fakeFollow;
        [Range(0, 1)] public float positionLerp = .9f;
        [Range(0, 1)] public float rotationLerp = .9f;

        private void Start()
        {
            _fakeFollow = new GameObject($"IO: Fake Follow [{name}]").transform;
            var thisTransform = transform;
            _fakeFollow.parent = thisTransform.root;
            _fakeFollow.position = followThis.position;
            _fakeFollow.rotation = followThis.rotation;

            thisTransform.parent = _fakeFollow;
        }
        
        private void Update()
        {
            Vector3 followPosition = followThis.position;
            Quaternion followRotation = followThis.rotation;

            Vector3 targetPosition = (positionLerp > .98f)
                ? followPosition
                : Vector3.LerpUnclamped(_fakeFollow.position, followPosition, positionLerp);
            Quaternion targetRotation = (rotationLerp > .98f)
                ? followRotation
                : Quaternion.SlerpUnclamped(_fakeFollow.rotation, followRotation, rotationLerp);


            _fakeFollow.position = targetPosition;
            _fakeFollow.rotation = targetRotation;
        }

        

        private void OnDestroy()
        {
            Destroy(_fakeFollow);
        }
    }
}
