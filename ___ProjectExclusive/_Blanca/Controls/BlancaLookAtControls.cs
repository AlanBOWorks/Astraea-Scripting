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
            Elements.Add(new LookAtTarget());
            Elements.Add(new LookAtMovement(velocity));
            Elements.Add(new LookAtRandom(transformData.Head, new SRange(1,7)));

            Timing.RunCoroutine(WaitUntilPlayerIsInstantiated());
            IEnumerator<float> WaitUntilPlayerIsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);
                ICharacterTransformData headData = playerEntity.CharacterTransformData;
                LookAtHeadData lookAtHead = new LookAtHeadData(headData) {LookAtWeight = 1};
                Elements.Add(lookAtHead);

                bool PlayerInstantiated()
                {
                    return playerEntity.CharacterTransformData != null;
                }
            }
        }

        public Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint)
        {
            Vector3 targetDirection = Elements[0].CalculateDirectionToLookAt(headReferencePoint);
            for (int i = 1; i < Elements.Count; i++)
            {
                targetDirection += Elements[i].CalculateDirectionToLookAt(headReferencePoint);
            }
            return targetDirection;
        }
    }
}
