using System;
using System.Collections.Generic;
using KinematicEssentials;
using MEC;
using SMaths;
using UnityEngine;

namespace AIEssentials
{
    public static class KinematicActionUtils
    {
        public static IEnumerator<float> _StopMotorVelocity(IKinematicMotorHandler motor, float changeSpeed)
        {
            while (motor.DesiredVelocity.sqrMagnitude > 0.01f)
            {
                motor.DesiredVelocity = 
                    Vector3.Lerp(motor.DesiredVelocity, Vector3.zero, Timing.DeltaTime * changeSpeed);
                yield return Timing.DeltaTime;
            }
            motor.DesiredVelocity = Vector3.zero;
        }

        public static IEnumerator<float> _StopMotorVelocity(IKinematicMotorHandler motor, float changeSpeed,
            SRange waitFor)
        {
            float timer = 0;
            float timerCheck = waitFor.RandomInRange();
            while (timer < timerCheck)
            {
                motor.DesiredVelocity =
                    Vector3.Lerp(motor.DesiredVelocity, Vector3.zero, Timing.DeltaTime * changeSpeed);
                yield return Timing.DeltaTime;
                timer += Timing.DeltaTime;
            }
            motor.DesiredVelocity = Vector3.zero;
        }
    }
}
