using System;
using System.Collections.Generic;
using AIEssentials;
using Companion;
using IKEssentials;
using MEC;
using SMaths;
using UnityEngine;

namespace Blanca.AIEssentials
{
    [Serializable]
    public class BlancaActionRotationSeek : IAction
    {
        [SerializeField] private SRotationVariable _rotationVariable = null;
        public SRange LookAtPathRandomness = new SRange(1,8);
        [Range(.1f,100), Tooltip("It will wait = the random amount of the first check * this")]
        public float SecondRandomCheckModifier = 2f;
        public float AngularSpeed = 2f;
        public float SpeedModifier = 2f;

        public IEnumerator<float> _DoRotation(CoroutineHandle master, IPathCalculator leadPathCalculator)
        {
            while (master.IsRunning)
            {
                Vector3 rotationDirection = leadPathCalculator.DesiredVelocity();
                rotationDirection = Vector3.LerpUnclamped(
                    rotationDirection,
                    CompanionUtils.BlancaToPlayerVector,
                    _rotationVariable.EvaluateRotation(CompanionUtils.DirectDistanceOfSeparation));
                rotationDirection += BlancaUtilsKinematic.KinematicVelocity.DesiredVelocity * SpeedModifier;

                BlancaUtilsKinematic.Motor.DesiredRotationForward = 
                    Vector3.Slerp(BlancaUtilsKinematic.Motor.CurrentRotationForward,
                        rotationDirection,
                        Time.deltaTime * AngularSpeed);
                yield return Timing.DeltaTime;
            }
        }

        public IEnumerator<float> _DoLookAtRotation(CoroutineHandle master)
        {
            float timer = 0;
            float checkTimer = LookAtPathRandomness.RandomInRange();
            while (master.IsRunning)
            {
                float targetWeight = _rotationVariable.EvaluateLookAt(CompanionUtils.DirectDistanceOfSeparation);
                if(timer < checkTimer)
                    BlancaUtilsIK.SetLookAtPlayer(targetWeight);
                else
                {
                    Vector3 lookAtDirection = BlancaUtilsTransform.CharacterTransform.MeshForward * 0.1f;
                    lookAtDirection += BlancaUtilsKinematic.KinematicVelocity.CurrentVelocity * SpeedModifier;
                    BlancaUtilsIK.LookAtDirection(lookAtDirection, targetWeight);
                    if (timer > checkTimer * SecondRandomCheckModifier)
                    {
                        checkTimer = LookAtPathRandomness.RandomInRange();
                        timer = 0;
                    }
                }

                yield return Timing.DeltaTime;
                timer += Time.deltaTime;

            }
        }
    }
}
