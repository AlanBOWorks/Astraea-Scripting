using System;
using System.Collections.Generic;
using AIEssentials;
using Companion;
using MEC;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.AIEssentials
{
    [Serializable]
    public class BlancaActionWaitForPlayerMovement : IAction
    {
        [SuffixLabel("Seconds/Meter")]
        [SerializeField] private AnimationCurve _waitByDistance = 
            new AnimationCurve(new Keyframe(0,.2f),new Keyframe(3,1.2f));

        [SuffixLabel("Degrees")] 
        [SerializeField] private float _angleThreshold = 30f;

        public IEnumerator<float> _CheckForDirection(IPathCalculator pathCalculator)
        {
            Vector3 pathDirection = pathCalculator.DesiredVelocity();
            float timer = 0;
            while (timer < _waitByDistance.Evaluate(CompanionUtils.DirectDistanceOfSeparation))
            {
                if (PlayerIsDirectionCorrect())
                {
                    timer += Timing.DeltaTime;
                }
                else
                {
                    pathDirection = pathCalculator.DesiredVelocity();
                    timer = 0;
                }

                yield return Timing.DeltaTime;
            }

            bool PlayerIsDirectionCorrect()
            {
                return Vector3.Angle(pathDirection, PlayerUtilsKinematic.GetPlayerKinematicData().CurrentVelocity)
                       < _angleThreshold
                    &&
                    PlayerUtilsKinematic.IsPlayerMoving;
            }
        }
    }
}
