using System;
using ___ProjectExclusive;
using AIEssentials;
using Animancer;
using AnimancerEssentials;
using Companion;
using IKEssentials;
using KinematicEssentials;
using PlayerEssentials;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerEntity : ICharacterDataHolder
    {
        [TabGroup("Data","Transform"), ShowInInspector, HideInEditorMode]
        private IPlayerTransformData _playerTransformData = null;
        public IPlayerTransformData CharacterTransformData
        {
            get => _playerTransformData;
            set
            {
                _playerTransformData = value;
                PlayerEntitySingleton.Instance.TransformData.Injection(value);
                PlayerUtilsTransform.TransformData = value;
            }
        }

        [TabGroup("Data", "Input"), ShowInInspector, HideInEditorMode]
        private PlayerInputData _inputData = null;
        public PlayerInputData InputData
        {
            get => _inputData;
            set
            {
                _inputData = value;
                PlayerUtilsKinematic.InputData = value;
            }
        }


        [TabGroup("Spacial handlers","Kinematic"), ShowInInspector, HideInEditorMode]
        public IKinematicMotorHandler MotorHandler = null;
        [TabGroup("Spacial handlers", "Kinematic"), ShowInInspector, HideInEditorMode]
        private IKinematicData _kinematicData = null;
        public IKinematicData KinematicData
        {
            get => _kinematicData;
            set
            {
                _kinematicData = value;
                PlayerUtilsKinematic.KinematicData = value;
            }
        }


        [TabGroup("Spacial handlers","Path"), ShowInInspector, HideInEditorMode]
        private IPathCalculator _mainPathHelper = null;
        public IPathCalculator MainPathHelper
        {
            get => _mainPathHelper;
            set
            {
                _mainPathHelper = value;
                PlayerUtilsKinematic.MainHelper = value;
            }
        }


        [TabGroup("Visual handlers","Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent BodyAnimancer = null;
        [TabGroup("Visual handlers","Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerHumanoidLayers AnimancerLayers = null;

        [TabGroup("Visual handlers", "IK"), ShowInInspector, HideInEditorMode]
        private FullHumanoidIKSolver _humanoidIkSolver = null;
        public FullHumanoidIKSolver HumanoidIkSolver
        {
            get => _humanoidIkSolver;
            set
            {
                _humanoidIkSolver = value;
                LeftArmTargetHandler = value.LeftTargetHandler;
                RightArmTargetHandler = value.RightTargetHandler;
            }
        }
        public FullArmTargetHandler LeftArmTargetHandler { get; private set; }
        public FullArmTargetHandler RightArmTargetHandler { get; private set; }

        public IIKSolver MainHandSolver => HumanoidIkSolver.MainHand;


        [TabGroup("Events","Movement"), ShowInInspector, HideInEditorMode]
        //---- Events
        public MovementTrackerEvent MovementTrackerEvent = null;

        [TabGroup("Events","Ticker"), HideInEditorMode] 
        public TickerHandler TickerHandler = null;


        #region :::: ICharacterDataHolder ::::
        public IKinematicData GetKinematicData() => _kinematicData;
        public ICharacterTransformData GetTransformData() => _playerTransformData;
        public FullHumanoidIKSolver GetHumanoidSolver() => _humanoidIkSolver; 
        #endregion
    }

    /// <summary>
    /// Single player adventure > Singleton 
    /// </summary>
    public sealed class PlayerEntitySingleton
    {
        static PlayerEntitySingleton() { }

        private PlayerEntitySingleton()
        {
            Entity = new PlayerEntity();
        }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();


        //Data container
        public PlayerParametersVariable Parameters = null;
        public TransformDataVariable TransformData = null;

        [HideInEditorMode, HideInPlayMode, HideInInlineEditors, HideDuplicateReferenceBox]
        public PlayerEntity Entity;
    }
}
