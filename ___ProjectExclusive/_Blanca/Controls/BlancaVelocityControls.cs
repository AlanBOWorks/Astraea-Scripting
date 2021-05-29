using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using AIEssentials;
using KinematicEssentials;
using MEC;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blanca
{
    public class BlancaVelocityControlHolder : BlancaMovementStructure<IVelocityControl>,
        IVelocityControl
    {
        public BlancaVelocityControlHolder(IBlancaPathStructure<VelocityPathCalculator> pathStructure):
            base()
        {
            Elements.Add( new PathVelocityControl(pathStructure.Base));
            Elements.Add(new PathVelocityControl(pathStructure.Lead));
            Elements.Add(new PathVelocityControl(pathStructure.ToPlayer));

            Timing.RunCoroutine(AddPlayerAftersItsInstantiated());
            IEnumerator<float> AddPlayerAftersItsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);
                IKinematicVelocity copyVelocity = playerEntity.KinematicData;
                Elements.Add(new CopyVelocityControl(copyVelocity));

                bool PlayerInstantiated()
                {
                    return playerEntity.KinematicData != null;
                }
            }
        }


        [ShowInInspector, PropertyRange(-10, 10)]
        public float VelocityWeight { get; set; } = 1;
        public Vector3 CalculateTargetVelocity()
        {
            Vector3 targetVelocity = Elements[0].CalculateTargetVelocity();
            for (int i = 1; i < Elements.Count; i++)
            {
                targetVelocity += Elements[i].CalculateTargetVelocity();
            }

            return targetVelocity * VelocityWeight;
        }
    }

    [Serializable]
    public class SerializedBlancaPathControls : SerializableBlancaPathStructure<VelocityPathCalculator>
    { }

}
