using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SharedLibrary
{
    [Serializable]
    public class FakeFollower 
    {
        [Title("Params")]
        [SerializeField] private Transform followThis = null;
        private Transform _fakeFollow;
        [SuffixLabel("deltas")] public float positionSpeed = 4f;
        [SuffixLabel("deltas")] public float angularSpeed = 4f;

        public float distanceThreshold = .1f;

        [NonSerialized]
        public Transform InjectionTarget;

        public void StartFollowing(Transform transform)
        {
            string name = transform.name;
            _fakeFollow = new GameObject($"IO: Fake Follow [{name}]").transform;
            var thisTransform = transform;
            _fakeFollow.parent = thisTransform.root;
            _fakeFollow.position = followThis.position;
            _fakeFollow.rotation = followThis.rotation;

            InjectionTarget = new GameObject($"IO: Injection Target [{name}]").transform;
            InjectionTarget.position = thisTransform.position;
            InjectionTarget.rotation = thisTransform.rotation;
            InjectionTarget.parent = _fakeFollow;

            Timing.RunCoroutine(_LoopUpdate());
        }


        private IEnumerator<float> _LoopUpdate()
        {
            while (followThis != null)
            {
                Update();
                yield return Timing.WaitForOneFrame;
            }
            Object.Destroy(_fakeFollow);
            Object.Destroy(InjectionTarget);
        }

        private void Update()
        {
            Vector3 followPosition = followThis.position;
            Vector3 fakePosition = _fakeFollow.position;

            Quaternion followRotation = followThis.rotation;
            Quaternion fakeRotation = _fakeFollow.rotation;

            Vector3 targetPosition;
            Quaternion targetRotation;

            float sqrDistance = (fakePosition - followPosition).sqrMagnitude;

            if (sqrDistance < distanceThreshold)
            {

                targetPosition 
                    = Vector3.Lerp(fakePosition, followPosition, positionSpeed * Time.deltaTime);
                targetRotation 
                    = Quaternion.Slerp(fakeRotation, followRotation, angularSpeed * Time.deltaTime);

            }
            else
            {
                //if too far, snap to the 90% lerp
                targetPosition = Vector3.LerpUnclamped(fakePosition,followPosition, .9f);
                targetRotation = Quaternion.SlerpUnclamped(fakeRotation,followRotation,.9f);
            }

            _fakeFollow.position = targetPosition;
            _fakeFollow.rotation = targetRotation;
        }


    }
}
