using System;
using System.Collections.Generic;
using AIEssentials;
using IKEssentials;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.Actions
{
    [Serializable]
    public class BlancaActionStopLookingAt : BlancaActionStopWeight
    {
        private IHeadIKSolver HeadIK => BlancaEntitySingleton.Instance.Entity.HumanoidIkSolver.HeadIkSolver;

        public BlancaActionStopLookingAt(float changeSpeed = 1f) : base(changeSpeed)
        {
        }

        protected override float TargetWeight
        {
            get => HeadIK.GetAverageWeight();
            set => HeadIK.SetWeight(value);
        }

    }

    [Serializable]
    public class BlancaActionStopHandIK : BlancaActionStopWeight
    {
        private TwoHandSolver HandsIK => BlancaEntitySingleton.Instance.Entity.HumanoidIkSolver.TwoHandSolver;

        public BlancaActionStopHandIK(float changeSpeed = 2f) : base(changeSpeed)
        {}

        protected override float TargetWeight
        {
            get => HandsIK.GetAverageWeight();
            set => HandsIK.SetWeight(value);
        }

    }

    public abstract class BlancaActionStopWeight : IEnumeratorOnEmpty
    {
        protected abstract float TargetWeight { get; set; }

        [SuffixLabel("deltas")]
        [Range(0, 100)]
        public float ChangeSpeed;

        protected BlancaActionStopWeight(float changeSpeed = 2f)
        {
            ChangeSpeed = changeSpeed;
        }

        public void DoAction()
        {
            float targetWeight = TargetWeight;
            targetWeight -= Time.deltaTime * ChangeSpeed;

            TargetWeight = Mathf.Max(0, targetWeight);
        }


        public bool IsActionFinish()
        {
            return TargetWeight <= 0;
        }

        public IEnumerator<float> OnEmptyRequest()
        {
            while (IsActionFinish())
            {
                DoAction();
                yield return Timing.DeltaTime;
            }

            TargetWeight = 0;
        }

        private CoroutineHandle _requestHandle;
        public void DoRequest()
        {
            Timing.KillCoroutines(_requestHandle);
            _requestHandle = Timing.RunCoroutine(OnEmptyRequest());
        }

        public void PauseRequest()
        {
            Timing.PauseCoroutines(_requestHandle);
        }

        public void KillRequest()
        {
            Timing.KillCoroutines(_requestHandle);
        }

        public bool IsRunning()
        {
            return _requestHandle.IsRunning;
        }
    }
}
