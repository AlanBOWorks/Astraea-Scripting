using FIMSpace.FLook;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class FullHumanoidIKSolver : HumanoidIKSolver
    {
        public FullArmTargetHandler LeftTargetHandler { get; private set; }
        public FullArmTargetHandler RightTargetHandler { get; private set; }
        public FullArmTargetHandler MainTargetHandler { get; private set; }
        public FullArmTargetHandler SecondaryTargetHandler { get; private set; }


        public FullHumanoidIKSolver(FullBodyBipedIK biped, FLookAnimator headLookAt, IrisLookAt irisLookAt,
            FingersSolverConstructor fingersSolverConstructor,
            bool isLeftMain = false) : base(biped, headLookAt, irisLookAt, isLeftMain)
        {
            Initialize(biped,fingersSolverConstructor);
        }
        public FullHumanoidIKSolver(FullBodyBipedIK biped, LookAtIK headLookAt, IrisLookAt irisLookAt,
            FingersSolverConstructor fingersSolverConstructor,
            bool isLeftMain = false) : base(biped, headLookAt, irisLookAt, isLeftMain)
        {
            Initialize(biped,fingersSolverConstructor);
        }

        private void Initialize(FullBodyBipedIK biped,FingersSolverConstructor fingersSolverConstructor)
        {
            IFingersIKSolver leftFingersSolver = fingersSolverConstructor.GetOrCreateLeftSolver(biped);
            IFingersIKSolver rightFingersSolver = fingersSolverConstructor.GetOrCreateRightSolver(biped);


            LeftTargetHandler = new FullArmTargetHandler(LeftHand,leftFingersSolver,biped.solver.leftArmChain);
            RightTargetHandler = new FullArmTargetHandler(RightHand,rightFingersSolver,biped.solver.rightArmChain);
            SwitchTargetHandlers();
        }

        public override void SwitchMainHand(bool isLeftMain)
        {
            base.SwitchMainHand(isLeftMain);
            SwitchTargetHandlers();
        }

        private void SwitchTargetHandlers()
        {
            if (IsLeftMain)
            {
                MainTargetHandler = LeftTargetHandler;
                SecondaryTargetHandler = RightTargetHandler;
            }
            else
            {
                MainTargetHandler = RightTargetHandler;
                SecondaryTargetHandler = LeftTargetHandler;
            }
        }

        /// <param name="smoothSpeed">For values less or equal to 0 the lerp will be instant</param>
        [Button("Handle Target")]
        public void HandleHandTarget(UHandTargetBase targetData, TargetTypes handTargetType,
            float smoothSpeed = 2)
        {
            switch (handTargetType)
            {
                case TargetTypes.Left:
                    LeftTargetHandler.HandleHandTarget(targetData,smoothSpeed);
                    break;
                case TargetTypes.Right:
                    RightTargetHandler.HandleHandTarget(targetData,smoothSpeed);
                    break;
                default:
                case TargetTypes.Main:
                    MainTargetHandler.HandleHandTarget(targetData,smoothSpeed);
                    break;
                case TargetTypes.Secondary:
                    SecondaryTargetHandler.HandleHandTarget(targetData,smoothSpeed);
                    break;
            }
        }

    }



    public class HumanoidIKSolver : IHumanoidIKStructure<IIKSolver>
    {
        public enum TargetTypes
        {
            Main,
            Secondary,
            Left,
            Right
        }

        private HumanoidIKSolver(FullBodyBipedIK biped, bool isLeftMain = false)
        {
            IsLeftMain = isLeftMain;
            LeftHand = new FinalIKSolver(biped.solver.leftHandEffector);
            RightHand = new FinalIKSolver(biped.solver.rightHandEffector);
            SwitchHandSolvers();
        }

        public HumanoidIKSolver
            (FullBodyBipedIK biped, FLookAnimator headLookAt, IrisLookAt irisLookAt, bool isLeftMain = false)
        : this(biped,isLeftMain)
        {
            HeadIkSolver = new FHeadLookAtSolver(headLookAt,irisLookAt);
        }

        public HumanoidIKSolver
            (FullBodyBipedIK biped, LookAtIK headLookAt, IrisLookAt irisLookAt, bool isLeftMain = false)
            : this(biped, isLeftMain)
        {
            HeadIkSolver = new FinalLookAtSolver(headLookAt, irisLookAt);
        }

        
        [ShowInInspector]
        public HeadLookAtSolverBase HeadIkSolver { get; }

        public IIKSolver Head => HeadIkSolver;
        [field: ShowInInspector]
        public IIKSolver RightHand { get; }

        [field: ShowInInspector]
        public IIKSolver LeftHand { get; }

        [field: ShowInInspector]
        public IIKSolver MainHand { get; private set; }

        [field: ShowInInspector]
        public IIKSolver SecondaryHand { get; private set; }

        protected bool IsLeftMain;
        public virtual void SwitchMainHand(bool isLeftMain)
        {
            IsLeftMain = isLeftMain;
            SwitchHandSolvers();
        }

        private void SwitchHandSolvers()
        {
            if (IsLeftMain)
            {
                MainHand = LeftHand;
                SecondaryHand = RightHand;
            }
            else
            {
                MainHand = RightHand;
                SecondaryHand = LeftHand;
            }
        }
    }

    public abstract class HeadLookAtSolverBase : IIKSolver
    {
        public readonly IrisLookAt IrisLookAt;

        protected HeadLookAtSolverBase(IrisLookAt irisLookAt)
        {
            IrisLookAt = irisLookAt;
        }

        public abstract void SetTarget(Transform target);

        public void SetTarget(Vector3 targetPoint)
        {
            SetHeadTarget(targetPoint);
            SetIrisTarget(targetPoint);
        }

        public abstract void SetRotation(Quaternion targetRotation);

        public void SetWeight(float weight)
        {
            SetHeadWeight(weight);
            SetIrisWeight(weight);
        }

        public abstract void SetRotationWeight(float weight);

        /// <summary>
        /// Gets the average of Iris and Head
        /// </summary>
        public float GetCurrentWeight() => GetHeadWeight();
        public abstract float GetRotationWeight();

        public abstract void SetHeadTarget(Vector3 targetPoint);
        public abstract void SetHeadWeight(float weight);
        public abstract float GetHeadWeight();

        public abstract Vector3 SolverPosition { get; }
        public abstract Quaternion SolverRotation { get; }

        public void SetIrisTarget(Vector3 targetPoint)
        {
            IrisLookAt.UpdateDestinationPoint(targetPoint);
        }
        public void SetIrisWeight(float weight)
        {
            IrisLookAt.Weight = weight;
        }

    }

    public class FinalLookAtSolver : HeadLookAtSolverBase
    {
        public readonly IKSolverLookAt HeadLookAt;

        public FinalLookAtSolver(LookAtIK headLookAt,IrisLookAt irisLookAt) : base(irisLookAt)
        {
            HeadLookAt = headLookAt.solver;
        }

        public override void SetTarget(Transform target)
        {
            HeadLookAt.target = target;
            IrisLookAt.Target = target;
        }

        public override void SetRotation(Quaternion targetRotation)
        {
        }

        public override void SetRotationWeight(float weight)
        {
        }

        public override float GetRotationWeight()
        {
            return 0;
        }

        public override void SetHeadTarget(Vector3 targetPoint)
        {
            HeadLookAt.IKPosition = targetPoint;
        }
        public override void SetHeadWeight(float weight)
        {
            HeadLookAt.headWeight = weight;
        }

        public override float GetHeadWeight() => HeadLookAt.headWeight;
        public override Vector3 SolverPosition => HeadLookAt.head.solverPosition;
        public override Quaternion SolverRotation => HeadLookAt.head.solverRotation;
    }

    public class FHeadLookAtSolver : HeadLookAtSolverBase
    {
        public readonly FLookAnimator HeadLookAt;

        public FHeadLookAtSolver(FLookAnimator headLookAt, IrisLookAt irisLookAt) : base(irisLookAt)
        {
            HeadLookAt = headLookAt;
        }

        public override void SetTarget(Transform target)
        {
            HeadLookAt.ObjectToFollow = target;
            IrisLookAt.Target = target;

            HeadLookAt.FollowMode = target is null 
                ? FLookAnimator.EFFollowMode.FollowJustPosition 
                : FLookAnimator.EFFollowMode.FollowObject;
        }

        public override void SetRotation(Quaternion targetRotation)
        { }

        public override void SetRotationWeight(float weight)
        { }

        public override float GetRotationWeight() => 0;

        public override void SetHeadTarget(Vector3 targetPoint)
        {
            HeadLookAt.SetLookPosition(targetPoint);
        }

        public override void SetHeadWeight(float weight)
        {
            HeadLookAt.LookAnimatorAmount = weight;
        }

        public override float GetHeadWeight()=> HeadLookAt.LookAnimatorAmount;


        public override Vector3 SolverPosition => HeadLookAt.HeadReference.position;
        public override Quaternion SolverRotation => HeadLookAt.HeadReference.rotation;
    }



    public class FinalIKSolver : IIKSolver
    {
        public readonly IKEffector Effector;

        public FinalIKSolver(IKEffector effector)
        {
            Effector = effector;
        }

        public void SetTarget(Transform target)
        {
            Effector.target = target;
        }

        public void SetTarget(Vector3 targetPoint)
        {
            Effector.position = targetPoint;
        }

        public void SetRotation(Quaternion targetRotation)
        {
            Effector.rotation = targetRotation;
        }

        public void SetWeight(float weight)
        {
            Effector.positionWeight = weight;
        }

        public void SetRotationWeight(float weight)
        {
            Effector.rotationWeight = weight;
        }

        public float GetCurrentWeight()
        {
            return Effector.positionWeight;
        }

        public float GetRotationWeight()
        {
            return Effector.rotationWeight;
        }


        public Vector3 SolverPosition => Effector.bone.position;
        public Quaternion SolverRotation => Effector.bone.rotation;
    }
}
