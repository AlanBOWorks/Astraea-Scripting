using System;
using System.Collections.Generic;
using AIEssentials;
using Animancer;
using AnimancerEssentials;
using Animators;
using IKEssentials;
using KinematicEssentials;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    [Serializable]
    public class BlancaEntity
    {
        [TabGroup("Transform"), ShowInInspector, HideInEditorMode]
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

        [TabGroup("Spacial handlers"), ShowInInspector, HideInEditorMode]
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

        [TabGroup("Spacial handlers"), ShowInInspector, HideInEditorMode]
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

        [TabGroup("Spacial handlers")] 
        [ShowInInspector, HideInEditorMode]
        private IPathCalculator _mainPathCalculator = null;
        public IPathCalculator MainPathCalculator
        {
            get => _mainPathCalculator;
            set
            {
                _mainPathCalculator = value;
                BlancaUtilsKinematic.MainPathCalculator = value;
            }
        }

        [ShowInInspector, HideInEditorMode] 
        private IPathCalculator[] _pathHelpers = null;
        public IPathCalculator[] PathCalculatorsHelpers
        {
            get => _pathHelpers;
            set
            {
                _pathHelpers = value;
                BlancaUtilsKinematic.HelperCalculators = value;
            }
        }


        [TabGroup("Brain"), ShowInInspector, HideInEditorMode]
        public ITaskManager _mainTaskManager = null;
        [TabGroup("Brain"), ShowInInspector, HideInEditorMode]
        public IRequestManagerEnumerator<CoroutineHandle> _lookAtTaskManager = null;
        [TabGroup("Brain"), ShowInInspector, HideInEditorMode]
        public IRequestManagerEnumerator<CoroutineHandle> _handsTaskManager = null;

        public ITaskManager MainTaskManager
        {
            get => _mainTaskManager;
            set
            {
                _mainTaskManager = value;
                BlancaUtilsManagers.MainManager = value;
            }
        }
        public IRequestManagerEnumerator<CoroutineHandle> LookAtTaskManager
        {
            get => _lookAtTaskManager;
            set
            {
                _lookAtTaskManager = value;
                BlancaUtilsManagers.LookAtManager = value;
            }
        }
        public IRequestManagerEnumerator<CoroutineHandle> HandsTaskManager
        {
            get => _handsTaskManager;
            set
            {
                _handsTaskManager = value;
                BlancaUtilsManagers.HandsManager = value;
            }
        }


        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent BodyAnimancer = null;
        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerHumanoidLayers AnimancerHumanoidLayers = null;
       
        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerComponent FacialAnimancer = null;
        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public AnimancerFacialLayers AnimancerFacialLayers = null;
        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        public EyesBlinkerHandler BlinkerHandler = null;


        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        private HumanoidIKSolver _humanoidIkSolver = null;

        public HumanoidIKSolver HumanoidIkSolver
        {
            get => _humanoidIkSolver;
            set
            {
                _humanoidIkSolver = value;
                BlancaUtilsIK.HumanoidIkSolver = value;
            }
        }

        [TabGroup("Events"), ShowInInspector, HideInEditorMode]
        public MovementTrackerEvent MovementTrackerEvent = null;

        [Title("Ticker"), ShowInInspector, HideInEditorMode]
        public TickerHandler TickerHandler = null;
    }



    /// <summary>
    /// It uses Singleton because, well, there's only one Character "Blanca" per game
    /// </summary>
    public sealed class BlancaEntitySingleton
    {
        static BlancaEntitySingleton() { }
        private BlancaEntitySingleton() { }
        public static BlancaEntitySingleton Instance { get; } = new BlancaEntitySingleton();

        //Data container
        public BlancaParametersVariable Parameters;
        public TransformDataVariable TransformData;

        [SerializeField, HideInEditorMode, HideInPlayMode, HideInInlineEditors,HideDuplicateReferenceBox]
        public BlancaEntity Entity = new BlancaEntity();
    }
}
