using IKEssentials;
using KinematicEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public class BlancaHeadIKControl : ITicker, IBlancaLookAtModifiable<float>
    {
        private readonly ICharacterTransformData _characterTransform;
        public readonly HeadLookAtSolverBase HeadIkSolver;
        [ShowInInspector]
        public readonly BlancaLookAtCalculatorsHolder LookAtCalculators;
        [ShowInInspector]
        public readonly BlancaLookWeights HeadWeights;
        [ShowInInspector]
        public readonly BlancaLookWeights IrisWeights;

        public BlancaHeadIKControl(ICharacterTransformData characterTransform,
            HeadLookAtSolverBase headIkSolver, IKinematicData kinematicData)
        {
            _characterTransform = characterTransform;
            HeadIkSolver = headIkSolver;
            HeadIkSolver.SetWeight(1);

            LookAtCalculators = new BlancaLookAtCalculatorsHolder(characterTransform,kinematicData);

            HeadWeights = new BlancaLookWeights();
            IrisWeights = new BlancaLookWeights();
        }


        public void UpdateLookAtTargetCurve(AnimationCurve distanceCurve)
        {
            LookAtCalculators.UpdateLookAtTargetCurve(distanceCurve);
        }

        public bool Disabled { get; set; }
        public void Tick()
        {
            LookAtCalculators.Tick();
            Vector3 headLookAt = 
                LookAtCalculators.CalculatePointLookAt(HeadWeights,1);
            Vector3 irisLookAt =
                LookAtCalculators.CalculatePointLookAt(IrisWeights, 3f); // Iris tends to overshoot more than head

            Vector3 smallForward = _characterTransform.MeshForward * .2f;

            HeadIkSolver.SetHeadTarget(headLookAt + smallForward);
            HeadIkSolver.SetIrisTarget(irisLookAt + smallForward);
        }

        public float Target
        {
            set
            {
                HeadWeights.Target = value;
                IrisWeights.Target = value;
            }
        }
        public float Movement
        {
            set
            {
                HeadWeights.Movement = value;
                IrisWeights.Movement = value;
            }
        }
        public float Random
        {
            set
            {
                HeadWeights.Random = value;
                IrisWeights.Random = value;
            }
        }
        public float AtPlayer
        {
            set
            {
                HeadWeights.AtPlayer = value;
                IrisWeights.AtPlayer = value;
            }
        }
        public float SecondaryTarget
        {
            set
            {
                HeadWeights.SecondaryTarget = value;
                IrisWeights.SecondaryTarget = value;
            }
        }
    }
}
