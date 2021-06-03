using System;
using AIEssentials;
using Animancer;
using AnimancerEssentials;
using Companion;
using KinematicEssentials;
using PlayerEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerEntity
    {
        [TabGroup("Transform"), ShowInInspector, HideInEditorMode]
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

        [TabGroup("Input"), ShowInInspector, HideInEditorMode]
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


        [TabGroup("Spacial handlers"), ShowInInspector, HideInEditorMode]
        public IKinematicMotorHandler MotorHandler = null;
        [TabGroup("Spacial handlers"), ShowInInspector, HideInEditorMode]
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


        [TabGroup("Spacial handlers"), ShowInInspector, HideInEditorMode]
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


        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent BodyAnimancer = null;
        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerHumanoidLayers AnimancerLayers = null;


        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        //---- Events
        public MovementTrackerEvent MovementTrackerEvent = null;

        [Title("Ticker"), HideInEditorMode] 
        public TickerHandler TickerHandler = null;

    }

    /// <summary>
    /// Single player adventure > Singleton 
    /// </summary>
    public sealed class PlayerEntitySingleton
    {
        static PlayerEntitySingleton() { }
        private PlayerEntitySingleton() { }
        public static PlayerEntitySingleton Instance { get; } = new PlayerEntitySingleton();


        //Data container
        public PlayerParametersVariable Parameters;
        public TransformDataVariable TransformData;

        [SerializeField, HideInEditorMode, HideInPlayMode, HideInInlineEditors, HideDuplicateReferenceBox]
        public PlayerEntity Entity = new PlayerEntity();
    }
}
