using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Blanca;
using Companion;
using KinematicEssentials;
using MEC;
using Player;
using RootMotion.FinalIK;
using SharedLibrary;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IKEssentials
{

    // Reminder (for hand rotations)
    // +X Axis: Back of the thumb
    // +Y Axis: forward of fingers
    // +Z Axis: Back of the palm
    public class HoldHandHandler
    {
        private ICharacterTransformData _rightHandedReference;
        private IKinematicVelocity _rightVelocity;
        private ICharacterTransformData _leftHandedReference;
        private IKinematicVelocity _leftVelocity;

        private readonly Transform _targetIk;
        public readonly Transform RightTarget;
        public readonly Transform LeftTarget;

        private IIKSolver _rightOtherHandSolver; //This is the right handed character's left hand
        private IIKSolver _leftOtherHandSolver;

        public HoldHandHandler(ICharacterDataHolder leftHanded, ICharacterDataHolder rightHanded,
            HoldHandTransforms transforms)
        {
            UpdateCharacters(leftHanded,rightHanded);

            _targetIk = transforms.Holder;
            RightTarget = transforms.RightTarget;
            LeftTarget = transforms.LeftTarget;

            _coroutineHandle = Timing.RunCoroutine(Update(),Segment.LateUpdate);
        }

        public void UpdateCharacters(ICharacterDataHolder leftHanded, ICharacterDataHolder rightHanded)
        {
            _rightHandedReference = rightHanded.GetTransformData();
            _rightVelocity = rightHanded.GetKinematicData();
            FullHumanoidIKSolver rightHumanoid = rightHanded.GetHumanoidSolver();
            _rightOtherHandSolver = rightHumanoid.SecondaryHand;

            _leftHandedReference = leftHanded.GetTransformData();
            _leftVelocity = leftHanded.GetKinematicData();
            FullHumanoidIKSolver leftHumanoid = leftHanded.GetHumanoidSolver();
            _leftOtherHandSolver = leftHumanoid.SecondaryHand;
        }


        public void ToggleHandling(bool updateIKs)
        {
            _forceUpdate = updateIKs;
        }
        public void ToggleHandling(bool updateIKs, Vector3 startPosition)
        {
            _forceUpdate = updateIKs;
            _targetIk.position = startPosition;
        }

        private HoldHandParameters _parameters = null;
        private CoroutineHandle _trackHandle;
        public void InjectParameters(HoldHandParameters parameters)
        {
            _parameters = parameters;
            if (!_trackHandle.IsRunning)
            {
                _trackHandle = Timing.RunCoroutine(_TrackParameterVariation(), Segment.LateUpdate);
                Timing.LinkCoroutines(_coroutineHandle, _trackHandle);
            }

            IEnumerator<float> _TrackParameterVariation()
            {
                while (_targetIk != null)
                {
                    float distance = CompanionUtils.DirectDistanceOfSeparation;
                    VariateHeight(distance);
                    VariateRotation(distance);
                    yield return Timing.WaitForOneFrame;
                }
            }
        }

        private float _distanceHeightVariation = 0;
        private void VariateHeight(float distance)
        {

            _distanceHeightVariation = _parameters.HeightDistanceCurve.Evaluate(distance);
        }

        private void VariateRotation(float distance)
        {
            float rotationTarget = _parameters.RotationDistanceCurve.Evaluate(distance);
            _rotationVariation = Quaternion.Euler(0,rotationTarget,0);
        }

        
        private Quaternion _rotationVariation;

        private const float PelvisPointLateralModifier = .05f;
        //The half of the right+left velocities && reduce it to a small proportion 
        private const float VelocityOffsetModifier = .5f * .1f;
        private const float SecondaryHandOffsetModifier = .5f * .3f;

        private CoroutineHandle _coroutineHandle;
        private bool _forceUpdate = false;
        private IEnumerator<float> Update()
        {
            while (_rightHandedReference != null && _leftHandedReference != null)
            {
                if (!_forceUpdate) yield return Timing.WaitForOneFrame;
                Vector3 rightPelvisPosition = _rightHandedReference.PelvisPosition;
                Vector3 rightPosition = rightPelvisPosition
                                            + _rightHandedReference.MeshRight * PelvisPointLateralModifier ;
                Vector3 leftPelvisPosition = _leftHandedReference.PelvisPosition;
                Vector3 leftPosition = leftPelvisPosition
                                        - _leftHandedReference.MeshRight * PelvisPointLateralModifier;

                // Pivot point calculations
                Vector3 directionTowardsSlave = leftPosition - rightPosition;
                Vector3 positionOfHands = rightPosition + directionTowardsSlave * .5f;
                positionOfHands.y += _distanceHeightVariation;

                // Offset calculations
                Vector3 velocityOffset = _rightVelocity.CurrentVelocity + _leftVelocity.CurrentVelocity;
                velocityOffset *= VelocityOffsetModifier;

                Vector3 rightSecondaryHandOffset = rightPelvisPosition - _rightOtherHandSolver.SolverPosition;
                Vector3 leftSecondaryHandOffset = leftPelvisPosition - _leftOtherHandSolver.SolverPosition;
                velocityOffset += 
                    (rightSecondaryHandOffset + leftSecondaryHandOffset) * SecondaryHandOffsetModifier;

                positionOfHands += velocityOffset;

                // Rotation calculations
                Quaternion targetRotation = Quaternion.LookRotation(directionTowardsSlave, Vector3.up)
                                            * _rotationVariation;


                float deltaVariation = Time.deltaTime * 10;
                _targetIk.position 
                    = Vector3.SlerpUnclamped(_targetIk.position, positionOfHands,deltaVariation); 
                _targetIk.rotation 
                    = Quaternion.SlerpUnclamped(_targetIk.rotation,targetRotation, deltaVariation);

                yield return Timing.WaitForOneFrame; 
            }
        }
    }

    [Serializable]
    public class HoldHandTransforms
    {
        [SerializeField] private Transform holder = null;
        [SerializeField] private Transform leftTarget = null;
        [SerializeField] private Transform rightTarget = null;

        public Transform Holder => holder;
        public Transform LeftTarget => leftTarget;
        public Transform RightTarget => rightTarget;
    }

    [Serializable]
    public class HoldHandParameters
    {

        [SerializeField] private bool _isPlayerRightHanded = true;
        public bool IsPlayerRightHanded => _isPlayerRightHanded;

        [SerializeField] private AnimationCurve heightDistanceCurve 
            = new AnimationCurve(new Keyframe(0,-5f), new Keyframe(.2f,0));
        public AnimationCurve HeightDistanceCurve => heightDistanceCurve;

        [SerializeField] private AnimationCurve rotationDistanceCurve
        = new AnimationCurve(new Keyframe(0,10f), new Keyframe(.2f,80f));
        public AnimationCurve RotationDistanceCurve => rotationDistanceCurve;

    }
}
