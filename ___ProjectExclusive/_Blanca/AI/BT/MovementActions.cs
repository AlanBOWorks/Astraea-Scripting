using System;
using AIEssentials;
using Companion;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using ParadoxNotion.Design;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    [Name("Move until reach Path\n [M - Blanca]")]
    [Category("___ Blanca ___/Movement")]
    [Description("Sets the path weights for Blanca movement until it reaches the destination")]
    public class BlancaEnablePathMovement : ActionTask
    {
        protected override string info => $"Move until reach Path.\n<b>[ {TargetPathfinder} ]</b>";

        private IPathCalculator _usingPath;
        [HideInPlayMode]
        public BlancaPathStructure.Types TargetPathfinder 
            = BlancaPathStructure.Types.Base;


        protected override string OnInit()
        {
            _usingPath =
                BlancaUtilsKinematic.PathControls.GetElement(TargetPathfinder);
            return base.OnInit();
        }

        protected override void OnExecute()
        {
           SetControlWeights(1);
           
        }


        protected override void OnUpdate()
        {
            if(_usingPath.HasReachedDestination())
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

    [Name("Until being in Formation\n [D&M - Blanca]")]
    [Category("___ Blanca ___/Destination & Movement")]
    [Description("(Just) Moves Blanca to the local Formation Point and returns success on " +
                 "close enough [Distance threshold]")]
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

    [Name("Remain in Formation\n [D&M - Blanca]")]
    [Category("___ Blanca ___/Destination & Movement")]
    [Description("Makes Blanca go to the formation, except it only returns running. " +
                 "(It should be exit by other means)")]
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

        protected override void OnStop()
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.ToPlayer.VelocityWeight = 0;
            velocityControl.Copy.VelocityWeight = 0;

            //TODO provisional; make a rotation parameter with curve by distance
            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = 0f;
            rotationControl.Copy.RotationWeight = 0f;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.AtPlayer.LookAtWeight = 0f;
            lookAtControl.Movement.LookAtWeight = 0f;
        }

        private static void EnableWeights()
        {
            var velocityControl = BlancaUtilsKinematic.VelocityControls;
            velocityControl.ToPlayer.VelocityWeight = 1;
            velocityControl.Copy.VelocityWeight = 1;

            //TODO provisional; make a rotation parameter with curve by distance
            var rotationControl = BlancaUtilsKinematic.RotationControls;
            rotationControl.Movement.RotationWeight = .3f;
            rotationControl.Copy.RotationWeight = 1f;

            var lookAtControl = BlancaUtilsIK.LookAtControls;
            lookAtControl.AtPlayer.LookAtWeight = .3f;
            lookAtControl.Movement.LookAtWeight = .7f;
        }
    }

    [Name("Until Lead to Target\n [M - Blanca]")]
    [Category("___ Blanca ___/Movement")]
    [Description("Enables Lead Movement and check if is too far (returns [Fail] if so)")]
    public class BlancaLeadToPoint : ActionTask
    {
        public float SeparationThreshold = 2f;

        protected override void OnExecute()
        {
            SetControlWeights(1);

        }


        protected override void OnUpdate()
        {
            IPathCalculator pathCalculator =
                BlancaUtilsKinematic.PathControls.Lead;

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
