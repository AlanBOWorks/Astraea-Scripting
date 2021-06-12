using System.Collections.Generic;
using ___ProjectExclusive;
using AIEssentials;
using IKEssentials;
using KinematicEssentials;
using MEC;
using Player;
using SharedLibrary;
using SMaths;
using UnityEngine;

namespace Blanca
{
    public class BlancaLookAtControlHolder : BlancaLookAtStructure<ILookAtControl>
    {
        public readonly LookAtTarget LookAtTarget;
        public readonly LookAtMovement LookAtMovement;
        public readonly LookAtRandom LookAtRandom;
        public readonly LookAtTarget LookAtPlayer;

        public BlancaLookAtControlHolder(ICharacterTransformData transformData, IKinematicData kinematicData) : base()
        {
            LookAtTarget = new LookAtTarget();
            LookAtMovement = new LookAtMovement(kinematicData,3); // watch further in the movement
            LookAtRandom = new LookAtRandom(transformData.Head, new SRange(1,7));
            AddBasicSetup(LookAtTarget,LookAtMovement,LookAtRandom);

            LookAtPlayer = new LookAtTarget();
            Elements.Add(LookAtPlayer);

            Timing.RunCoroutine(WaitUntilPlayerIsInstantiated());
            IEnumerator<float> WaitUntilPlayerIsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);

                LookAtPlayer.TrackTransform(
                    playerEntity.CharacterTransformData.GetLookAtPoint(), Vector3.zero);

                bool PlayerInstantiated()
                {
                    return playerEntity.CharacterTransformData != null;
                }
            }
        }

        public void UpdateLookAtTargetCurve(AnimationCurve sqrDistanceCurve)
        {
            LookAtTarget.SqrCloseModifier = sqrDistanceCurve;
            LookAtPlayer.SqrCloseModifier = sqrDistanceCurve;
        }

        public Vector3 CalculatePointLookAt(Vector3 headReferencePoint)
        {
            float weightSum = Elements[0].LookAtWeight;
            Vector3 targetDirection = Elements[0].PointOfLookAt(headReferencePoint);
            for (int i = 1; i < Elements.Count; i++)
            {
                DoSum(Elements[i]);
            }

            if (weightSum == 0) //This is just to avoid divide by 0
                return headReferencePoint;

            return targetDirection/weightSum;


            void DoSum(ILookAtControl control)
            {
                weightSum += control.LookAtWeight;
                targetDirection += control.PointOfLookAt(headReferencePoint);
            }
        }
    }
}
