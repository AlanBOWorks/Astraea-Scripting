using System;
using AIEssentials;
using Companion;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using Player;
using UnityEngine;

namespace Blanca
{
    [Name("Go to Transform [Blanca]")]
    [Category("___ Blanca ___")]
    [Description("Makes Blanca go to the target Transform")]
    public class BlancaGoToTransform : ActionTask
    {
        public Transform TargetPoint;
        public float DistanceThreshold = .1f;

        protected override void OnExecute()
        {
           SetControlWeights(1);

            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.Base;
            pathCalculator.SetDestination(TargetPoint.position);
            pathCalculator.SetReachDestinationDistance(DistanceThreshold);
        }


        protected override void OnUpdate()
        {
            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.Base;
            pathCalculator.SetDestination(TargetPoint.position);

            if(pathCalculator.HasReachedDestination())
                EndAction(true);
        }

        protected override void OnStop()
        {
            SetControlWeights(0);
        }

        private static void SetControlWeights(float target)
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.Base.VelocityWeight = target;

            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = target;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.Movement.LookAtWeight = target;
        }
    }

    [Name("Go to Formation [Blanca]")]
    [Category("___ Blanca ___")]
    [Description("Moves Blanca to the local Formation Point")]
    public class BlancaToFormation : ActionTask
    {
        public Vector2 FormationPosition = new Vector2(.2f,.1f);
        [SliderField(-10,10)]
        public float DistanceThreshold = .2f;

        protected override void OnExecute()
        {
            EnableWeights();
        }

        protected override void OnUpdate()
        {
            GoToFormation();
            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.ToPlayer;
            if(pathCalculator.IsCloseEnough(DistanceThreshold))
                EndAction(true);
        }

        protected void GoToFormation()
        {
            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.ToPlayer;
            Vector3 targetPosition =
                PlayerUtilsTransform.CalculateFormationPosition(FormationPosition);
            targetPosition = Vector3.MoveTowards(
                targetPosition,
                BlancaUtilsTransform.GetCharacterPosition(),
                DistanceThreshold);

            pathCalculator.SetDestination(targetPosition);
        }

        protected override void OnStop()
        {
            DisableWeights();
        }

        private static void EnableWeights()
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.ToPlayer.VelocityWeight = 1;

            //TODO provisional; make a rotation parameter with curve by distance
            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = .7f;
            rotationControl.Copy.RotationWeight = .3f;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.AtPlayer.LookAtWeight = .7f;
            lookAtControl.Movement.LookAtWeight = .3f;
        }

        protected static void DisableWeights()
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.ToPlayer.VelocityWeight = 0;

            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = 0;
            rotationControl.Copy.RotationWeight = 0f;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.AtPlayer.LookAtWeight = 0;
            lookAtControl.Movement.LookAtWeight = 0f;

        }
    }

    public class BlancaRemainInFormation : BlancaToFormation
    {
        protected override void OnExecute()
        {
            EnableWeights();
        }

        protected override void OnUpdate()
        {
            base.GoToFormation();
        }

        private static void EnableWeights()
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.ToPlayer.VelocityWeight = 1;

            //TODO provisional; make a rotation parameter with curve by distance
            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = .3f;
            rotationControl.Copy.RotationWeight = .7f;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.AtPlayer.LookAtWeight = .3f;
            lookAtControl.Movement.LookAtWeight = .7f;
        }
    }

    [Name("Lead to Target [Blanca]")]
    [Category("___ Blanca ___")]
    [Description("Leads player towards a target (fails if too far)")]
    public class BlancaLeadToPoint : ActionTask
    {
        public Transform TargetPoint;
        public float DistanceThreshold = .1f;
        public float SeparationThreshold = 2f;

        protected override void OnExecute()
        {
            SetControlWeights(1);

            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.Lead;
            pathCalculator.SetDestination(TargetPoint.position);
            pathCalculator.SetReachDestinationDistance(DistanceThreshold);
        }


        protected override void OnUpdate()
        {
            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.Lead;
            pathCalculator.SetDestination(TargetPoint.position);

            if (pathCalculator.HasReachedDestination())
                EndAction(true);
            if(CompanionUtils.DirectDistanceOfSeparation > SeparationThreshold)
                EndAction(false);
        }

        protected override void OnStop()
        {
            SetControlWeights(0);
        }

        private static void SetControlWeights(float target)
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.Lead.VelocityWeight = target;

            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = target;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.Movement.LookAtWeight = target;
        }
    }
}
