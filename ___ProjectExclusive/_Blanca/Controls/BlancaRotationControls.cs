using System.Collections.Generic;
using ___ProjectExclusive;
using KinematicEssentials;
using MEC;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public class BlancaRotationControlHolder : BlancaRotationStructure<IRotationControl>
    {
        private readonly KinematicData _characterKinematicData;

        public BlancaRotationControlHolder(KinematicData data) : base()
        {
            _characterKinematicData = data;
            MovementRotationControl movementRotationControl = new MovementRotationControl(data);
            TargetRotationControl targetRotationControl = new TargetRotationControl();
            AddBasicSetup(movementRotationControl,targetRotationControl);

            Timing.RunCoroutine(AddPlayerAfterItsInstantiated());
            IEnumerator<float> AddPlayerAfterItsInstantiated()
            {
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
                yield return Timing.WaitUntilTrue(PlayerInstantiated);
                ICharacterTransformData copyData = playerEntity.CharacterTransformData;
                Elements.Add(new CopyCharacterRotationControl(copyData));

                bool PlayerInstantiated()
                {
                    return playerEntity.CharacterTransformData != null;
                }
            }

            Movement.RotationWeight = 1;
        }


        [ShowInInspector, PropertyRange(-10, 10)]
        public float RotationWeight { get; set; } = 1;
        public Vector3 CalculateForwardEuler()
        {
            //Small forward for avoid zero vector
            Vector3 smallForward = _characterKinematicData.CurrentRotationForward * .1f;
            Vector3 elementsForward = Elements[0].CalculateForwardEuler();
            for (var i = 1; i < Elements.Count; i++)
            {
                IRotationControl control = Elements[i];
                elementsForward += control.CalculateForwardEuler();
            }

            return smallForward + elementsForward * RotationWeight;
        }

    }
}
