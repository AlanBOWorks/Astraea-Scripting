using System;
using System.Collections.Generic;
using AIEssentials;
using KinematicEssentials;
using MEC;
using SMaths;
using UnityEngine;
using Utility;

namespace Blanca.AIEssentials
{
    public class BlancaActionStopMoving : IAction, IEnumeratorOnEmpty
    {
        private readonly float _breakSpeed;
        public BlancaActionStopMoving(float breakSpeed = 4f)
        {
            _breakSpeed = breakSpeed;
        }
        public IEnumerator<float> OnEmptyRequest()
        {
            return KinematicActionUtils._StopMotorVelocity(BlancaUtilsKinematic.Motor, _breakSpeed);
        }

        public static IEnumerator<float> _StopMotorVelocity(float breakSpeed = 4)
        {
            return KinematicActionUtils._StopMotorVelocity(BlancaUtilsKinematic.Motor, breakSpeed);
        }

        public static IEnumerator<float> _StopMotorVelocity(SRange waitFor, float breakSpeed = 4)
        {
            return KinematicActionUtils._StopMotorVelocity(BlancaUtilsKinematic.Motor, breakSpeed, waitFor);
        }

    }
}
