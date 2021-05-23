using System;
using System.Collections.Generic;
using AIEssentials;
using IKEssentials;
using KinematicEssentials;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blanca.AIEssentials
{
    [Serializable]
    public class BlancaActionLookAtVelocity : IAction
    {
        [Title("Look At - Params")]
        [Range(0, 100)]
        public float VelocityVectorModifier = 2f;



        public Vector3 CalculatePointOfVelocity()
        {
            Vector3 pointOfVelocity = BlancaUtilsIK.CalculateLookAtVelocity(VelocityVectorModifier);
            return pointOfVelocity;
        }

        public virtual void DoLookAction()
        {
            BlancaUtilsIK.LookAtPoint(CalculatePointOfVelocity(),1);
        }

        public IEnumerator<float> ExtractAction(Func<bool> loopCondition, float targetWeight = 1f)
        {
            return BlancaUtilsIK.LookAtVelocityTask(loopCondition, VelocityVectorModifier, targetWeight);
        }
    }

    public class BlancaActionDefaultLookAt : IEnumeratorOnEmpty
    {
        private dynamic _aliveReference;
        public BlancaActionDefaultLookAt(dynamic aliveReference)
        {
            _aliveReference = aliveReference;
        }

        //It uses magic numbers since this instance is unique in the whole game
        public IEnumerator<float> OnEmptyRequest()
        {
            yield return Timing.WaitForOneFrame;
            IHeadIKSolver headSolver = BlancaUtilsIK.HeadIkSolver;
            IPathCalculator mainCalculator = BlancaUtilsKinematic.MainPathCalculator;
            Vector3 headOffset = new Vector3(0,.85f, 0);

            Vector3 randomOffset = Vector3.zero;
            float randomMagnitude = 1f;

            Timing.LinkCoroutines(Timing.CurrentCoroutine, Timing.RunCoroutine(_LargeRandom()));
            Timing.LinkCoroutines(Timing.CurrentCoroutine, Timing.RunCoroutine(_SmallRandom()));

            while (_aliveReference != null)
            {

                Vector3 pointOfReference = mainCalculator.SteeringPoint();
                Vector3 characterForward = BlancaUtilsTransform.CharacterTransform.MeshForward;

                headSolver.SetTarget(
                    pointOfReference + characterForward + headOffset + (randomOffset) * randomMagnitude);
                headSolver.SetWeight(.8f);
                yield return Timing.DeltaTime; 
            }


            IEnumerator<float> _LargeRandom()
            {
                while (_aliveReference != null)
                {
                    float largeWaitTime = Random.Range(2, 30);
                    yield return Timing.WaitForSeconds(largeWaitTime);

                    Vector2 randomGet = Random.insideUnitCircle * Random.Range(-2f, 2f);
                    randomOffset.x = randomGet.x;
                    randomOffset.z = randomGet.y;

                    randomOffset.y = Random.Range(-.1f, .1f); //Horizontal looks are bigger than verticals
                }
            }

            IEnumerator<float> _SmallRandom()
            {
                bool toggleValue = false;
                while (_aliveReference != null)
                {
                    yield return Timing.WaitForSeconds(Random.Range(3, 6));
                    randomMagnitude = toggleValue ? Random.Range(.5f, 1.5f) : Random.value;
                    toggleValue = !toggleValue;
                }

            }
        }
    }
}
