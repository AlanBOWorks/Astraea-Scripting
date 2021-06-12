using Blanca;
using Companion;
using IKEssentials;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Player;
using SMaths;
using UnityEngine;

namespace AIEssentials
{
    /// <summary>
    /// Moves the hand towards the holding target for t seconds;
    /// </summary>
    [Name("Reach Player's hand\n [IK - Blanca]")]
    [Category("___ Blanca ___/IK")]
    [Description("Tries to reach the players hand\n [Success if close enough / No Fail state]")]
    public class BlancaTriesToHold : ActionTask
    {
        public AnimationCurve WeightBySqrDistance = new AnimationCurve(
            new Keyframe(0,1),new Keyframe(.2f,0));

        private float _currentWeight;

        protected override void OnExecute()
        {
            BlancaEntity blancaEntity = BlancaEntitySingleton.GetEntity();

            IIKSolver blancaSolver = blancaEntity.MainHandSolver;

            _currentWeight = blancaSolver.GetCurrentWeight();
        }

        protected override void OnUpdate()
        {
            BlancaEntity blancaEntity = BlancaEntitySingleton.GetEntity();
            PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;
            
            IIKSolver blancaSolver = blancaEntity.MainHandSolver;
            IIKSolver playerSolver = playerEntity.MainHandSolver;

            Vector3 blancaSolverPosition = blancaSolver.SolverPosition;
            Vector3 playerSolverPosition = playerSolver.SolverPosition;

            blancaSolver.SetTarget(playerSolverPosition);

            float sqrDistance = (blancaSolverPosition - playerSolverPosition).sqrMagnitude;

            float blancaWeight =
                WeightBySqrDistance.Evaluate(sqrDistance);
            blancaWeight = Mathf.Clamp01(blancaWeight);
            _currentWeight = Mathf.Lerp(_currentWeight, blancaWeight, Time.deltaTime * 3);
            blancaSolver.SetWeight(_currentWeight);
            blancaSolver.SetRotationWeight(_currentWeight);

            if (_currentWeight > .9)
            {
                blancaSolver.SetWeight(1);
                blancaSolver.SetRotationWeight(1);
                this.EndAction(true);
                
            }
        }
    }

    /// <summary>
    /// Keeps them holding until it breaks (then request for disabling the IK [outer request])
    /// </summary>
    [Name("Do Hold Hand\n [IK - Companions]")]
    [Category("___ Companions ___/IK")]
    [Description("Sets players and Blanca hands IK\n [No Success nor Fail state]")]
    public class BothHandHolds : ActionTask
    {
        private float _blancaWeight;
        private float _playerWeight;

        protected override void OnExecute()
        {
            BlancaEntity blancaEntity = BlancaEntitySingleton.GetEntity();
            PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;

            IIKSolver blancaSolver = blancaEntity.MainHandSolver;
            IIKSolver playerSolver = playerEntity.MainHandSolver;

            _blancaWeight = blancaSolver.GetCurrentWeight();
            _playerWeight = playerSolver.GetCurrentWeight();

            CompanionUtils.UpdateHandIkPositionToHoldHand(true, blancaSolver.SolverPosition);

        }

        protected override void OnUpdate()
        {
            float deltaVariation = Time.deltaTime * 4;
            _blancaWeight = Mathf.Lerp(_blancaWeight, 1, deltaVariation);
            _playerWeight = Mathf.Lerp(_playerWeight, 1, deltaVariation);

            SetWeights(_blancaWeight,_playerWeight);
        }

        protected override void OnStop()
        {
            CompanionUtils.UpdateHandIkPositionToHoldHand(false);
            SetWeights(0,0);
        }

        private void SetWeights(float blancaTarget, float playerTarget)
        {
            BlancaEntity blancaEntity = BlancaEntitySingleton.GetEntity();
            PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;

            IIKSolver blancaSolver = blancaEntity.MainHandSolver;
            IIKSolver playerSolver = playerEntity.MainHandSolver;

            blancaSolver.SetWeight(blancaTarget);
            blancaSolver.SetRotationWeight(blancaTarget);
            playerSolver.SetWeight(playerTarget);
            playerSolver.SetRotationWeight(playerTarget);
        }
    }
}
