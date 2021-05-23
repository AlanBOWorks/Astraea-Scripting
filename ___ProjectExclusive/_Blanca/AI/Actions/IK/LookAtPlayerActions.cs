using System;
using System.Collections.Generic;
using AIEssentials;
using Companion;
using MEC;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blanca.AIEssentials
{
    [Serializable]
    public class LookAtPlayerAction : BlancaActionLookAtVelocity
    {
       
        [SerializeField, InfoBox("In Unit",InfoMessageType.Warning)]
        private AnimationCurve _lookAtPlayerByDistance = new AnimationCurve(
            new Keyframe(0,.2f), new Keyframe(1f,1));



        public override void DoLookAction()
        {
            BlancaUtilsIK.LookAtPoint(EvaluatedLookAtPoint(),1);
        }

        public Vector3 EvaluatedLookAtPoint()
        {
            Vector3 pointOfVelocity = CalculatePointOfVelocity();


            Vector3 playerPoint = BlancaUtilsIK.LookAtPlayerPoint();
            float weightOfLookPlayer =
                _lookAtPlayerByDistance.Evaluate(CompanionEntitySingleton.Instance.Entity.DistanceOfSeparation);

            Vector3 finalLookAt = Vector3.LerpUnclamped(pointOfVelocity, playerPoint, weightOfLookPlayer);

            return finalLookAt;
        }
    }

    [Serializable]
    public class LookAtPlayerRandomTiming
    {
        [SuffixLabel("Seconds")]
        public SRange RandomChecks = new SRange(1,3);

        [SuffixLabel("Seconds")] 
        public SRange OffRandomChecks = new SRange(3,8);


        public IEnumerator<float> _DoLookAtPlayer(Func<bool> aliveCondition,float targetWeight = 1)
        {
            bool randomToggle = true;
            float randomWeight = 1f;
            Timing.RunCoroutine(_RandomTick());

            while (aliveCondition())
            {
                if (randomToggle)
                {
                    BlancaUtilsIK.LerpToTargetWeight(targetWeight);
                    BlancaUtilsIK.SetLookAtPlayer();
                }
                else
                {
                    BlancaUtilsIK.LerpToTargetWeight(randomWeight);
                }

                yield return Timing.DeltaTime;
            }

            IEnumerator<float> _RandomTick()
            {
                while (aliveCondition())
                {
                    float waitSeconds = (randomToggle) 
                        ? OffRandomChecks.RandomInRange()
                        : RandomChecks.RandomInRange() ;
                    yield return Timing.WaitForSeconds(waitSeconds);
                    randomToggle = !randomToggle;
                    randomWeight = Random.value;
                }
            }
        }
    }
}
