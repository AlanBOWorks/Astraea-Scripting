﻿using System;
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
        [TabGroup("Spacial handlers")]
        [ShowInInspector]
        private SerializedBlancaPathControls _pathControls;
        public SerializedBlancaPathControls PathControls
        {
            get => _pathControls;
            set
            {
                _pathControls = value;
                BlancaUtilsKinematic.MainPathCalculator = PathControls.Base;
                BlancaUtilsKinematic.PathControls = PathControls;
            }
        }
        [TabGroup("Spacial handlers")]
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

        [TabGroup("Visual handlers"), ShowInInspector, HideInEditorMode]
        private BlancaLookAtControlHolder _lookAtControls = null;
        public BlancaLookAtControlHolder LookAtControls
        {
            get => _lookAtControls;
            set
            {
                _lookAtControls = value;
                BlancaUtilsIK.LookAtControls = value;

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

        public static BlancaEntity GetEntity() => Instance.Entity;
    }
}
