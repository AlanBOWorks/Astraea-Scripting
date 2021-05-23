using System;
using FIMSpace.FLook;
using RootMotion.FinalIK;
using UnityEngine;

namespace IKEssentials
{
    public class HumanoidIKSolver : HumanoidIKStructureBase<IIKSolver>
    {
        public HumanoidIKSolver(FullBodyBipedIK biped, FLookAnimator headLookAt, IrisLookAt irisLookAt, bool isLeftMain = false)
        {
            TwoHandSolver = new TwoHandSolver(biped,isLeftMain);
            RightHand = new FinalIKSolver(biped.solver.rightHandEffector);
            LeftHand = new FinalIKSolver(biped.solver.leftHandEffector);

            HeadIkSolver = new HeadLookAtSolver(headLookAt,irisLookAt);
        }

        public TwoHandSolver TwoHandSolver { get; private set; }
        public IHeadIKSolver HeadIkSolver { get; private set; }
        public sealed override IIKSolver Head
        {
            get => HeadIkSolver;
            set
            {
                if (value is IHeadIKSolver solver)
                    HeadIkSolver = solver;
                else
                {
                    throw new ArgumentException($"Passed [{value}] is not a [{typeof(IHeadIKSolver)}]");
                }
            }
        }

        public sealed override IIKSolver RightHand
        {
            get => TwoHandSolver.RightSolver;
            set => TwoHandSolver.RightSolver = value;
        }

        public sealed override IIKSolver LeftHand
        {
            get => TwoHandSolver.LeftSolver;
            set => TwoHandSolver.LeftSolver = value;
        }
    }

    public class HeadLookAtSolver : IHeadIKSolver
    {
        public readonly FLookAnimator HeadLookAt;
        public readonly IrisLookAt IrisLookAt;

        public HeadLookAtSolver(FLookAnimator headLookAt, IrisLookAt irisLookAt)
        {
            HeadLookAt = headLookAt;
            IrisLookAt = irisLookAt;
        }




        public void SetTarget(Vector3 targetPoint)
        {
            SetHeadTarget(targetPoint);
            SetIrisTarget(targetPoint);
        }

        public void SetWeight(float weight)
        {
            SetHeadWeight(weight);
            SetIrisWeight(weight);
        }

        /// <summary>
        /// Gets the average of Iris and Head
        /// </summary>
        public float GetCurrentWeight()
        {
            return GetAverageWeight();
        }
        public float GetAverageWeight()
        {
            return (GetHeadWeight() + GetIrisWeight()) *.5f;
        }


        public void SetHeadTarget(Vector3 targetPoint)
        {
            HeadLookAt.SetLookPosition(targetPoint);
        }

        public void SetHeadWeight(float weight)
        {
            HeadLookAt.LookAnimatorAmount = weight;
        }

        public float GetHeadWeight()
        {
            return HeadLookAt.LookAnimatorAmount;
        }

        public Vector3 GetHeadLookPoint()
        {
            return HeadLookAt.GetLookAtPosition();
        }


        public void ModifyWeight(float variation)
        {
            float headWeight = GetHeadWeight() + variation;
            float irisWeight = IrisLookAt.Weight + variation;
            headWeight = Mathf.Clamp01(headWeight);
            irisWeight = Mathf.Clamp01(irisWeight);
            SetHeadWeight(headWeight);
            SetIrisWeight(irisWeight);
        }

        public void SetIrisTarget(Vector3 targetPoint)
        {
            IrisLookAt.UpdateDestinationPoint(targetPoint);
        }

        public void SetIrisWeight(float weight)
        {
            IrisLookAt.Weight = weight;
        }

        public float GetIrisWeight()
        {
            return IrisLookAt.Weight;
        }

        public Vector3 GetIrisLookPoint()
        {
            return IrisLookAt.Destination;
        }
    }

    public class FinalIKSolver : IIKSolver
    {
        public readonly IKEffector Effector;

        public FinalIKSolver(IKEffector effector)
        {
            Effector = effector;
        }

        public void SetTarget(Vector3 targetPoint)
        {
            Effector.position = targetPoint;
        }

        public void SetWeight(float weight)
        {
            Effector.positionWeight = weight;
        }

        public float GetCurrentWeight()
        {
            return Effector.positionWeight;
        }

        public void ModifyWeight(float variation)
        {
            Effector.positionWeight += variation;
        }
    }
}
