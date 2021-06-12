using System;
using MEC;
using UnityEngine;

namespace IKEssentials
{
    public interface IIKSolver
    {
        // It doesn't use the (Transform target) variation for smooth transitions withing points.
        // Yes, this is just a reminder so I'm not feeling tempted of adding this
        //void SetTarget(Transform target);
        void SetTarget(Vector3 targetPoint);
        void SetRotation(Quaternion targetRotation);
        void SetWeight(float weight);
        void SetRotationWeight(float weight);

        float GetCurrentWeight();
        float GetRotationWeight();

        Vector3 SolverPosition { get; }
        Quaternion SolverRotation { get; }
    }


    public interface IMainHandSetter
    {
        bool IsLeftMainHand {get;}
    }
}
