using AIEssentials;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{

    [Name("Set Destination Path\n [Blanca]")]
    [Category("___ Blanca ___/Destination")]
    [Description("Sets the path destination and returns [Success] once it has path.")]
    public class BlancaSetDestination : ActionTask
    {
        protected override string info
        {
            get
            {
                string infoText = $"Set Destination Path.\n<b>[ {TargetPathfinder} ]</b>";

                if (OnTransform != null) infoText += $"\n < {OnTransform.name} >" +
                                                     $"\n {OnTransform.position}";
                return infoText;
            }
        }

        protected IPathCalculator UsingPath;
        [HideInPlayMode]
        public BlancaPathStructure.Types TargetPathfinder
            = BlancaPathStructure.Types.Base;

        public Transform OnTransform = null;
        public float DistanceThreshold = .2f;
        public bool PerTickUpdate = true;

        protected override string OnInit()
        {
            UsingPath =
                BlancaUtilsKinematic.PathControls.GetElement(TargetPathfinder);
            return null;
        }

        protected override void OnExecute()
        {
            UsingPath.SetDestination(OnTransform.position);
            UsingPath.SetReachDestinationDistance(DistanceThreshold);
        }

        protected override void OnUpdate()
        {
            if(!UsingPath.HasPath()) return;

            if (PerTickUpdate)
            {
                UsingPath.SetDestination(OnTransform.position);
                if(UsingPath.HasReachedDestination())
                    base.EndAction(true);
            }
            else
            {
                base.EndAction(true);

            }
        }
    }

    [Name("Reach Destination Path\n [Blanca]")]
    [Category("___ Blanca ___/Destination")]
    public class HasReachedDestination : ConditionTask
    {
        
        protected override string info => $"Has reach Destination Path.\n<b>[ {TargetPathfinder} ]</b>";

        protected IPathCalculator UsingPath;
        [HideInPlayMode]
        public BlancaPathStructure.Types TargetPathfinder
            = BlancaPathStructure.Types.Base;

        protected override string OnInit()
        {
            UsingPath =
                BlancaUtilsKinematic.PathControls.GetElement(TargetPathfinder);
            return null;
        }

        protected override bool OnCheck()
        {
            return UsingPath.HasPath() && UsingPath.HasReachedDestination();
        }
    }
}
