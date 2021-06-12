using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using AIEssentials;
using KinematicEssentials;
using MEC;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blanca
{
    public class BlancaVelocityControlHolder : BlancaMovementStructure<IVelocityControl>
    {
        public BlancaVelocityControlHolder(IBlancaPathStructure<VelocityPathCalculator> pathStructure):
            base()
        {
            PathVelocityControl baseControl = new PathVelocityControl(pathStructure.Base);
            PathVelocityControl leadControl = new PathVelocityControl(pathStructure.Lead);
            PathVelocityControl toPlayerControl = new PathVelocityControl(pathStructure.ToPlayer);
            AddBasicSetup(baseControl,leadControl,toPlayerControl);

            Timing.RunCoroutine(AddPlayerAftersItsInstantiated());
            IEnumerator<float> AddPlayerAftersItsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);
                Elements.Add(new CopyVelocityControl(playerEntity.InputData));

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



        public void SetWeights(BlancaVelocityWeight target)
        {
            Base.VelocityWeight = target.Base;
            Lead.VelocityWeight = target.Lead;
            ToPlayer.VelocityWeight = target.ToPlayer;
            Copy.VelocityWeight = target.Copy;
        }

    }

    [Serializable]
    public class SerializedBlancaPathControls : SerializableBlancaPathStructure<VelocityPathCalculator>
    { }


}
