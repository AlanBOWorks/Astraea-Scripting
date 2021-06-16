using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class IrisLookAt : MonoBehaviour
    {
        [Title("References")] [SerializeReference]
        private Transform _bodyHead = null;
        [SerializeReference] private Transform _leftIrisRotation = null;
        [SerializeReference] private Transform _rightIrisRotation = null;


        [Title("Params")]
        public Vector3 ForwardOffset;
        private Vector3 _randomOffset;

        public Transform Target { set; private get; }
        public Vector3 Destination { get; private set; }

        [SuffixLabel("Seconds")]
        public float DestinationUpdateRate = .2f;

        public float VerticalMax = .1f;
        public float HorizontalMax = .15f;
        public float MinForward = .8f;

        public float VibrationStrength = .1f;
        public float RandomWaitModifier = 1f;

        [Title("Control")] 
        [Range(0, 1)] public float Weight = 1f;

        private CoroutineHandle _randomHandle;

        public void UpdateDestinationPoint(Vector3 point)
        {
            Destination = point;
        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_randomHandle);
        }
        private void OnDisable()
        {
            Timing.PauseCoroutines(_randomHandle);
            ResetRotations();
        }
        private void Start()
        {
            _randomHandle = Timing.RunCoroutine(_UpdateRandom());
        }

        [Button]
        private void ResetRotations()
        {
            _leftIrisRotation.rotation = _bodyHead.rotation;
            _rightIrisRotation.localRotation = _bodyHead.rotation;
        }


      


        // In Update because Coroutines doesn't works as I expect and don't know why this happens
        private float _timer = 0;

        public Vector3 ClampedLocalDirection { get; private set; }
        public Quaternion IrisRotation { get; private set; }
        private void Update()
        {
            if (_timer > DestinationUpdateRate)
            {
                _timer = 0;

                if (Target) Destination = Target.position;
                Vector3 lookForward = Destination - _bodyHead.position;
                
                //To local
                lookForward = _bodyHead.InverseTransformDirection(lookForward);
                lookForward += ForwardOffset + _randomOffset;

                //Clamp
                lookForward.y = Mathf.Clamp(lookForward.y, -VerticalMax, VerticalMax);
                lookForward.x = Mathf.Clamp(lookForward.x, -HorizontalMax, HorizontalMax);
                lookForward.z = Mathf.Max(MinForward, lookForward.z);

                ClampedLocalDirection = lookForward;

                //To world
                lookForward = _bodyHead.TransformDirection(lookForward);

                Quaternion targetRotation = ProjectedDirectionOnHead();
                targetRotation = Quaternion.LerpUnclamped(_bodyHead.rotation, targetRotation, Weight);
                IrisRotation = targetRotation;

                _leftIrisRotation.rotation = targetRotation;
                _rightIrisRotation.rotation = targetRotation;

                Quaternion ProjectedDirectionOnHead()
                {
                    return Quaternion.LookRotation(lookForward, _bodyHead.up);
                }
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

        private IEnumerator<float> _UpdateRandom()
        {
            bool doneRandom = false;
            while (transform)
            {
                if (doneRandom)
                {
                    doneRandom = false;
                    _randomOffset = Random.insideUnitCircle * VibrationStrength;
                    yield return Timing.WaitForSeconds(NearWait());
                }
                else
                {
                    doneRandom = true;
                    _randomOffset = Vector3.zero;
                    yield return Timing.WaitForSeconds(ZeroWait());
                }


                float ZeroWait()
                {
                    return Random.Range(1f, 3f) * RandomWaitModifier;

                }

                float NearWait()
                {
                    return Random.Range(.2f, .6f) * RandomWaitModifier;
                }
            }
        }
    }
}
