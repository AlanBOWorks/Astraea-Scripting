using System.Collections.Generic;
using MEC;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class FullArmTargetHandler
    {
        private readonly IIKSolver _handSolver;
        private readonly IFingersIKSolver _fingersSolver;
        private readonly IKConstraintBend _bend;
        private readonly FBIKChain _chain;


        public FullArmTargetHandler(IIKSolver hand, IFingersIKSolver finger, FBIKChain chain)
        {
            _handSolver = hand;
            _fingersSolver = finger;
            _chain = chain;
            _bend = chain.bendConstraint;
        }

       
        private Transform _currentTarget;
        private Transform _currentBendTarget;

        private CoroutineHandle _tickHandle;

        private IEnumerator<float> Tick()
        {
            while (_handSolver != null)
            {
                Vector3 targetPoint = _currentTarget.position;
                Quaternion targetRotation = _currentTarget.rotation;

                _handSolver.SetTarget(targetPoint);
                _handSolver.SetRotation(targetRotation);
                if (_currentBendTarget != null)
                    _bend.direction = _currentBendTarget.position - targetPoint;

                yield return Timing.WaitForOneFrame;
            }
        }


        private CoroutineHandle _smoothHandle;
        public void HandleHandTarget(UHandTargetBase targetData, float smoothSpeed)
        {
            IHandTransformsTarget targets = targetData.GetTransformsTarget();
            if (targets is null) return;
            IHandPersistentTargetData data = targetData.GetPersistentData();

            Timing.KillCoroutines(_smoothHandle);
            Timing.PauseCoroutines(_tickHandle);

            _currentTarget = targets.GetHandTarget();
            _currentBendTarget = targets.GetBendTarget();
            Vector3 currentTargetPoint = _handSolver.SolverPosition;
            Quaternion currentTargetRotation = _handSolver.SolverRotation;
            Vector3 currentBendDirection = _bend.direction;


            float lerpAmount = (smoothSpeed <= 0) ? 1 : 0;
            if (data != null)
                _smoothHandle = Timing.RunCoroutine(_DoSmoothDataLerp(),Segment.LateUpdate);
            else
                _smoothHandle = Timing.RunCoroutine(_DoSmoothDefaultLerp(), Segment.LateUpdate);



            IEnumerator<float> _DoSmoothDataLerp()
            {
                while (lerpAmount < 1)
                {
                    DoHandLerp();
                    DoTransformsLerp();
                    yield return Timing.WaitForOneFrame;
                    lerpAmount += Time.deltaTime * smoothSpeed;
                }
                lerpAmount = 1;
                DoHandLerp();
                DoTransformsLerp();

                InvokeLoopUpdate();


                void DoHandLerp()
                {
                    float handPositionWeight = CalculateLerp(
                        _handSolver.GetCurrentWeight(),
                        data.PositionWeight);
                    float handRotationWeight = CalculateLerp(
                        _handSolver.GetRotationWeight(),
                        data.RotationWeight);

                    _handSolver.SetWeight(handPositionWeight);
                    _handSolver.SetRotationWeight(handRotationWeight);

                    OverrideValue(ref _chain.reach, data.ReachWeight);
                    OverrideValue(ref _chain.push, data.PushWeight);
                    OverrideValue(ref _chain.pushParent, data.PushParent);
                    OverrideValue(ref _bend.weight, data.BendWeight);

                    _fingersSolver.HandleWeights(data, lerpAmount);
                }

            }

            IEnumerator<float> _DoSmoothDefaultLerp()
            {
                float bendWeight = (_bend.bendGoal is null) ? 0 : 1;
                while (lerpAmount < 1)
                {
                    DoLerp();
                    DoTransformsLerp();
                    yield return Timing.WaitForOneFrame;
                    lerpAmount += Time.deltaTime * smoothSpeed;
                }
                lerpAmount = 1;
                DoLerp();
                DoTransformsLerp();

                InvokeLoopUpdate();

                void DoLerp()
                {
                    float handPositionWeight = CalculateLerp(
                        _handSolver.GetCurrentWeight(),
                        1);
                    float handRotationWeight = CalculateLerp(
                        _handSolver.GetRotationWeight(),
                        1);

                    _handSolver.SetWeight(handPositionWeight);
                    _handSolver.SetRotationWeight(handRotationWeight);

                    OverrideValue(ref _chain.reach, 0);
                    OverrideValue(ref _chain.push, 0);
                    OverrideValue(ref _chain.pushParent, 0);
                    OverrideValue(ref _bend.weight, bendWeight);

                    _fingersSolver.HandleWeights(FingersWeight.Zero);
                }
            }


            float CalculateLerp(float a, float b)
            {
                return Mathf.LerpUnclamped(a, b, lerpAmount);
            }

            void OverrideValue(ref float a, float target)
            {
                a = Mathf.LerpUnclamped(a, target, lerpAmount);
            }


            void DoTransformsLerp()
            {
                Vector3 targetPoint = _currentTarget.position;
                Quaternion targetRotation = _currentTarget.rotation;
                Vector3 handPoint = Vector3.LerpUnclamped(
                    currentTargetPoint, targetPoint, lerpAmount);
                Quaternion handRotation = Quaternion.SlerpUnclamped(
                    currentTargetRotation, targetRotation, lerpAmount);

                _handSolver.SetTarget(handPoint);
                _handSolver.SetRotation(handRotation);

                if (_currentBendTarget is null) return;

                Vector3 bendDirection = Vector3.LerpUnclamped(
                    currentBendDirection, _currentBendTarget.position - targetPoint, lerpAmount);
                _bend.direction = bendDirection;
            }

            void InvokeLoopUpdate()
            {
                if (_tickHandle.IsValid)
                {
                    Timing.ResumeCoroutines(_tickHandle);
                }
                else
                {
                    _tickHandle = Timing.RunCoroutine(Tick(), Segment.LateUpdate);
                }
            }
        }

        [ShowInInspector]
        public bool Disabled
        {
            set
            {
                if (value)
                {
                    Timing.PauseCoroutines(_tickHandle);
                    Timing.PauseCoroutines(_smoothHandle);

                }
                else
                {
                    Timing.ResumeCoroutines(_tickHandle);
                    Timing.ResumeCoroutines(_smoothHandle);
                }
            }
        }
    }
}
