using System.Collections.Generic;
using ___ProjectExclusive;
using AIEssentials;
using IKEssentials;
using KinematicEssentials;
using MEC;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace Blanca
{
    public class BlancaLookAtCalculatorsHolder : BlancaLookAtStructure<ILookAtCalculator>, ITicker
    {
        [HideInPlayMode]
        public readonly LookAtTarget LookAtTarget;
        [HideInPlayMode]
        public readonly LookAtMovement LookAtMovement;
        [HideInPlayMode]
        public readonly LookAtRandom LookAtRandom;
        [HideInPlayMode]
        public readonly LookAtTarget LookAtPlayer;
        [HideInPlayMode]
        public readonly LookAtTarget LookAtSecondary;
        [ShowInInspector, DisableInPlayMode]
        public readonly List<Vector3> ClampedDirections;

        private readonly Transform _headReference;

        public BlancaLookAtCalculatorsHolder(ICharacterTransformData transformData, IKinematicData kinematicData) : base()
        {
            ClampedDirections = BlancaLookNormalizedDirections.GenerateList();
            _headReference = transformData.Head;

            LookAtTarget = new LookAtTarget();
            LookAtMovement = new LookAtMovement(kinematicData,3); // watch further in the movement
            LookAtRandom = new LookAtRandom(transformData.Head, new SRange(3,7));
            AddBasicSetup(LookAtTarget,LookAtMovement,LookAtRandom);

            LookAtPlayer = new LookAtTarget();
            Elements.Add(LookAtPlayer);

            LookAtSecondary = new LookAtTarget();
            Elements.Add(LookAtSecondary);

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

        public void UpdateLookAtTargetCurve(AnimationCurve distanceCurve)
        {
            LookAtTarget.WeightByDistance = distanceCurve;
            LookAtPlayer.WeightByDistance = distanceCurve;
            LookAtSecondary.WeightByDistance = distanceCurve;
        }
        
        public Vector3 CalculatePointLookAt(BlancaLookAtStructure<float> weights, float modifier)
        {
            // It needs to be a point because direction can overshoot in the air instead of the 
            // position that it should be looking at
            Vector3 direction = Vector3.zero;
            for (int i = 0; i < ClampedDirections.Count; i++)
            {
                Vector3 elementDirection = ClampedDirections[i];
                direction += elementDirection * weights.Elements[i];
            }

            return _headReferencePoint + direction * modifier;
        }

        private Vector3 _headReferencePoint;
        public bool Disabled { get; set; }
        public void Tick()
        {
            _headReferencePoint = _headReference.position;
            for (int i = 0; i < Elements.Count; i++)
            {
                Vector3 direction = Elements[i].DirectionLookAt(_headReferencePoint);
                direction = Vector3.ClampMagnitude(direction,1);
                ClampedDirections[i] = direction;
            }
        }
    }
}
