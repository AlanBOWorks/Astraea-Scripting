using System;
using IKEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public class UTorchDeclarationInjector : MonoInjector
    {
        [SerializeField] private bool pinOnInjection = true;

        [Title("Torch")] 
        [SerializeField] private FakeFollower proceduralTransformConstructor = null;
        [SerializeField] private UHandTargetBase leftTarget;
        [SerializeField] private UHandTargetBase rightTarget;


        [Title("References")] 
        [SerializeField] private TorchHoldersReferences references
            = new TorchHoldersReferences();

        public override void DoInjection()
        {
            BlancaPropsEntity entity = BlancaEntitySingleton.Instance.Props;

            // ---- Initializations
            proceduralTransformConstructor.StartFollowing(transform);

            // ---- Holders
            TorchHoldersTransform holdersTransform = new TorchHoldersTransform(references)
            {
                Procedural = proceduralTransformConstructor.InjectionTarget
            };

            // ---- Controller
            TorchTransformController transformController 
                = new TorchTransformController(references,holdersTransform);

            // ---- Entity (Injection)
            entity.TransformController = transformController;
            entity.TorchLeftTarget = leftTarget;
            entity.TorchRightTarget = rightTarget;


            // ---- Others
            if (pinOnInjection)
            {
                FullHumanoidIKSolver solver = BlancaEntitySingleton.Instance.Entity.HumanoidIkSolver;
                solver.RightTargetHandler.HandleHandTarget(rightTarget, -10);
                solver.LeftTargetHandler.HandleHandTarget(leftTarget, -10);
            }
        }


    }
}
