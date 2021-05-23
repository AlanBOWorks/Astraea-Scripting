using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class TwoHandSolver
    {
        public IIKSolver MainHandSolver => (IsLeftMain) ? LeftSolver : RightSolver;
        public IIKSolver SecondaryHandSolver => (IsLeftMain) ? RightSolver : LeftSolver;

        [ShowInInspector]
        public IIKSolver LeftSolver;

        [ShowInInspector]
        public IIKSolver RightSolver;

        public bool IsLeftMain;


        public TwoHandSolver(FullBodyBipedIK biped, bool isLeftMain = false)
        {
            LeftSolver = new HandSolver(biped.solver.leftHandEffector);
            RightSolver = new HandSolver(biped.solver.rightHandEffector);

            IsLeftMain = isLeftMain;
        }

        public void SetTarget(Vector3 targetPoint)
        {
            LeftSolver.SetTarget(targetPoint);
            RightSolver.SetTarget(targetPoint);
        }

        public void SetWeight(float weight)
        {
            LeftSolver.SetWeight(weight);
            RightSolver.SetWeight(weight);
        }

        public float GetAverageWeight()
        {
            return (LeftSolver.GetCurrentWeight() + RightSolver.GetCurrentWeight()) * .5f;
        }

        public void ModifyWeight(float variation)
        {
            LeftSolver.ModifyWeight(variation);
            RightSolver.ModifyWeight(variation);

        }
    }


    public class HandSolver : FinalIKSolver
    {
        public void SetRotation(Quaternion rotation)
        {
            Effector.rotation = rotation;
        }

        public void SetRotationWeight(float weight)
        {
            Effector.rotationWeight = weight;
        }

        public HandSolver(IKEffector effector) : base(effector)
        {
        }
    }

    /// <summary>
    /// Used primarily for calculations
    /// </summary>
    public class ArmMappingData
    {
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        private Transform _upperArm;

        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        private Transform _hand;
        public ArmMappingData(IKMappingLimb armLimb)
        {
            _upperArm = armLimb.bone1;
            _hand = armLimb.bone3;
            Vector3 handPosition = _hand.position;
            ArmLength = Vector3.Distance(handPosition, _upperArm.position);
            float toFingerDistance = Vector3.Distance(handPosition, _hand.GetChild(0).position);
            FingerOffset = toFingerDistance * 2;
            MidPalmOffset = toFingerDistance * 0.5f;

        }
        public Vector3 GetUpperArmPosition()
        {
            return _upperArm.position;
        }


        public Vector3 GetWristPosition()
        {
            return _hand.position;
        }


        public Vector3 CalculatePalmIKPoint(Vector3 targetPoint)
        {
            return targetPoint - (targetPoint - _upperArm.position).normalized * MidPalmOffset;
        }

        public Vector3 CalculatePalmIKPoint(Vector3 targetPoint, Vector3 offset)
        {
            Vector3 calculated = CalculatePalmIKPoint(targetPoint);
            calculated += _hand.TransformDirection(offset);
            return calculated;
        }

        public Vector3 CalculateFingerIKPoint(Vector3 targetPoint)
        {
            return targetPoint - (targetPoint - _upperArm.position).normalized * FingerOffset;

        }

        /// <summary>
        /// The length from the shoulder to the wrist;
        /// Used mainly for IK positioning
        /// </summary>
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public readonly float ArmLength;
        /// <summary>
        /// Aprox. the length from the wrist to the middle of the palm;
        /// Used mainly for provide an offset to the IK Positioning (eg: for grabbing; Holding)
        /// </summary>
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public readonly float MidPalmOffset;

        /// <summary>
        /// Aprox. the length towards the tip of the finger; Used mainly for finger touch
        /// </summary>
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public readonly float FingerOffset;

    }
}
       
