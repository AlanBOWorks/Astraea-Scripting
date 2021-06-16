using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using AIEssentials;
using Animancer;
using AnimancerEssentials;
using Animators;
using Companion;
using IKEssentials;
using KinematicEssentials;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    [Serializable]
    public class BlancaEntity : ICharacterDataHolder
    {
        [TabGroup("Transform","Transform"), ShowInInspector, HideInEditorMode]
        private ICharacterTransformData _characterTransformData = null;
        public ICharacterTransformData CharacterTransformData
        {
            get => _characterTransformData;
            set
            {
                _characterTransformData = value;
                BlancaEntitySingleton.Instance.TransformData.Injection(value);
                BlancaUtilsTransform.CharacterTransform = value;
            }
        }
        

        [TabGroup("Transform", "Spacial handlers"), ShowInInspector, HideInEditorMode]
        private KinematicMotorHandler _motorHandler = null;
        public KinematicMotorHandler MotorHandler
        {
            get => _motorHandler;
            set
            {
                _motorHandler = value;
                BlancaUtilsKinematic.Motor = value;
            }
        }
        [TabGroup("Transform", "Spacial handlers"), ShowInInspector, HideInEditorMode]
        private KinematicData _kinematicData = null;
        public KinematicData KinematicData
        {
            get => _kinematicData;
            set
            {
                _kinematicData = value;
                BlancaUtilsKinematic.KinematicVelocity = _kinematicData;
                BlancaUtilsKinematic.KinematicRotation = _kinematicData;
            }
        }
        [TabGroup("Transform", "Spacial handlers")] 
        [ShowInInspector]
        private BlancaVelocityControlHolder _velocityControls;
        public BlancaVelocityControlHolder VelocityControls
        {
            set
            {
                _velocityControls = value;
                BlancaUtilsKinematic.VelocityControls = value;
            }
            get => _velocityControls;
        }
        [TabGroup("Transform", "Spacial handlers")]
        [ShowInInspector]
        private SerializedBlancaPathControls _pathControls;
        public SerializedBlancaPathControls PathControls
        {
            get => _pathControls;
            set
            {
                _pathControls = value;
                BlancaUtilsKinematic.PathControls = PathControls;
            }
        }
        [TabGroup("Transform", "Spacial handlers")]
        [ShowInInspector]
        private BlancaRotationControlHolder _rotationControls;
        public BlancaRotationControlHolder RotationControls
        {
            set
            {
                _rotationControls = value;
                BlancaUtilsKinematic.RotationControls = value;
            }
            get => _rotationControls;
        }







        [TabGroup("Visual handlers", "Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent BodyAnimancer = null;
        [TabGroup("Visual handlers", "Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerHumanoidLayers AnimancerHumanoidLayers = null;
       
        [TabGroup("Visual handlers", "Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent FacialAnimancer = null;
        [TabGroup("Visual handlers", "Animancer"), ShowInInspector, HideInEditorMode]
        public AnimancerFacialLayers AnimancerFacialLayers = null;
        [TabGroup("Visual handlers", "Animancer"), ShowInInspector, HideInEditorMode]
        public EyesBlinkerHandler BlinkerHandler = null;

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


        public LookAtTarget LookAtTargetCalculator { get; private set; }
        public LookAtTarget LookAtTargetSecondaryCalculator { get; private set; }
        public LookAtRandom LookAtRandom { get; private set; }
        public IBlancaLookAtModifiable<float> FullHeadWeights { get; private set; }
        public BlancaLookWeights HeadLookWeights { get; private set; }
        public BlancaLookWeights IrisLookWeights { get; private set; }

        [TabGroup("Visual handlers", "IK"), ShowInInspector, HideInEditorMode]
        private BlancaHeadIKControl _headIkControl = null;

        public BlancaHeadIKControl HeadIkControl
        {
            get => _headIkControl;
            set
            {
                _headIkControl = value;
                LookAtTargetCalculator = value.LookAtCalculators.LookAtTarget;
                LookAtTargetSecondaryCalculator = value.LookAtCalculators.LookAtSecondary;
                LookAtRandom = value.LookAtCalculators.LookAtRandom;
                FullHeadWeights = value;
                HeadLookWeights = value.HeadWeights;
                IrisLookWeights = value.IrisWeights;

                BlancaUtilsIK.LookAtTargetCalculator = LookAtTargetCalculator;
                BlancaUtilsIK.FullHeadWeights = value;
            }
        }

        [TabGroup("Events", "Emotion handlers"), ShowInInspector, HideInEditorMode]
        private BlancaEmotionsData _emotionsData = null;
        public BlancaEmotionsData EmotionsData
        {
            get => _emotionsData;
            set => _emotionsData = value;
        }
        private BlancaEmotionAnimationsHolder _emotionAnimations;
        public BlancaEmotionAnimationsHolder EmotionAnimations
        {
            get => _emotionAnimations;
            set => _emotionAnimations = value;
        }


        [TabGroup("Events","Movement"), ShowInInspector, HideInEditorMode]
        public MovementTrackerEvent MovementTrackerEvent = null;
        public RotationTrackerEvent RotationTrackerEvent = null;

        [Title("Ticker"), ShowInInspector, HideInEditorMode]
        public TickerHandler TickerHandler = null;


        #region :::: ICharacterDataHolder ::::
        public IKinematicData GetKinematicData() => _kinematicData;
        public ICharacterTransformData GetTransformData() => _characterTransformData;
        public FullHumanoidIKSolver GetHumanoidSolver() => _humanoidIkSolver; 
        #endregion
    }



    /// <summary>
    /// It uses Singleton because, well, there's only one Character "Blanca" per game
    /// </summary>
    public sealed class BlancaEntitySingleton
    {
        static BlancaEntitySingleton() { }

        private BlancaEntitySingleton()
        {
            Entity = new BlancaEntity();
            Props = new BlancaPropsEntity();
        }
        public static BlancaEntitySingleton Instance { get; } = new BlancaEntitySingleton();

        //Data container
        public BlancaParametersVariable Parameters = null;
        public TransformDataVariable TransformData = null;

        public BlancaEntity Entity = null;
        public BlancaPropsEntity Props = null;
        public CoroutineLoopHandler CoroutineLoopHandler = null;

        public static BlancaEntity GetEntity() => Instance.Entity;
    }

    [Serializable]
    public class BlancaPropsEntity
    {
        [ShowInInspector]
        public TorchTransformController TransformController = null;
        
        public UHandTargetBase TorchLeftTarget = null;
        public UHandTargetBase TorchRightTarget = null;


    }
}
