using System;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public struct BreakableAcceleration
    {
        public float Acceleration;
        public float BreakForce;

        public BreakableAcceleration(float acceleration, float breakForce)
        {
            Acceleration = acceleration;
            BreakForce = breakForce;
        }

        public float DeltaModifier(Vector3 currentVelocity, Vector3 targetVelocity)
        {
            return currentVelocity.sqrMagnitude < targetVelocity.sqrMagnitude 
                ? Acceleration 
                : BreakForce;
        }
    }
}
