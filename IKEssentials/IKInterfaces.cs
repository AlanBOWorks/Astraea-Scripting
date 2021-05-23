using System;
using MEC;
using UnityEngine;

namespace IKEssentials
{
    public interface IIKSolver
    {
        void SetTarget(Vector3 targetPoint);
        void SetWeight(float weight);

        float GetCurrentWeight();
        /// <summary>
        /// For subtraction or addition
        /// </summary>
        void ModifyWeight(float variation);
    }

    public interface IHeadIKSolver : IIKSolver
    {
        float GetAverageWeight();
        void SetHeadTarget(Vector3 targetPoint);
        void SetHeadWeight(float weight);
        float GetHeadWeight();
        Vector3 GetHeadLookPoint();

        void SetIrisTarget(Vector3 targetPoint);
        void SetIrisWeight(float weight);
        float GetIrisWeight();
        Vector3 GetIrisLookPoint();
    }

    public interface IMainHandSetter
    {
        bool IsLeftMainHand {get;}
    }
}
