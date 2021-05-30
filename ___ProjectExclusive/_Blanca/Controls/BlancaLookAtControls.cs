using System.Collections.Generic;
using ___ProjectExclusive;
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
        public BlancaLookAtControlHolder(ICharacterTransformData transformData, IKinematicVelocity velocity) : base()
        {
            LookAtTarget lookAtTarget = new LookAtTarget();
            LookAtMovement lookAtMovement = new LookAtMovement(velocity,3); // watch further in the movement
            LookAtRandom lookAtRandom = new LookAtRandom(transformData.Head, new SRange(1,7));
            AddBasicSetup(lookAtTarget,lookAtMovement,lookAtRandom);

            Timing.RunCoroutine(WaitUntilPlayerIsInstantiated());
            IEnumerator<float> WaitUntilPlayerIsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);
                ICharacterTransformData headData = playerEntity.CharacterTransformData;
                LookAtHeadData lookAtHead = new LookAtHeadData(headData);
                Elements.Add(lookAtHead);

                bool PlayerInstantiated()
                {
                    return playerEntity.CharacterTransformData != null;
                }
            }
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
