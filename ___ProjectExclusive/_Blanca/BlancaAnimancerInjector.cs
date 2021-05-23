using Animancer;
using Animators;
using Blanca;
using KinematicEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnimancerEssentials
{
    public class BlancaAnimancerInjector : MonoInjector
    {
        [Title("Animancer Settings")]
        [SerializeField, HideInPlayMode]
        private AnimancerHumanoidLayersSetter _bodyLayersSetter = new AnimancerHumanoidLayersSetter();
        [SerializeField, HideInPlayMode]
        private AnimancerFacialLayersSetter _facialLayerSetters = new AnimancerFacialLayersSetter();


        [Title("Animation Settings")]
        [TabGroup("Body"), SerializeField, HideInPlayMode] private AnimationClip _idleClip = null;
        [TabGroup("Body"), SerializeField, HideInPlayMode] private MixerTransition2D _defaultMovement = null;

        [TabGroup("Facial"), SerializeField, HideInPlayMode] private AnimationClip _idleExpression = null;
        [TabGroup("Facial"), SerializeField, HideInPlayMode] private AnimationClip _blinkExpression = null;


        //Persistent
        [TabGroup("Body"), SerializeField]
        private AnimancerMovementInjector animancerMovementInjector = new AnimancerMovementInjector();
        [TabGroup("Facial"), SerializeField]
        private EyesBlinkerHandler _blinkerHandler = new EyesBlinkerHandler();



        public override void DoInjection()
        {
            // GETS
            BlancaEntity entity = BlancaEntitySingleton.Instance.Entity;
            KinematicData kinematicData = entity.KinematicData;

            AnimancerComponent bodyAnimancer = entity.BodyAnimancer;
            AnimancerComponent facialAnimancer = entity.FacialAnimancer;

            TickerHandler ticker = entity.TickerHandler;

            //---- Animancer
            AnimancerHumanoidLayers humanoidLayers
                = _bodyLayersSetter.GenerateLayers(bodyAnimancer);
            AnimancerFacialLayers facialLayers
                = _facialLayerSetters.GenerateLayers(facialAnimancer);

            //---- Animancer: states
            AnimancerMovementStates animancerMovementStates = new AnimancerMovementStates(
                _idleClip,_defaultMovement.Transition, humanoidLayers.Base);
            AnimancerBaseStatesHandler animancerBaseStates 
                = new AnimancerBaseStatesHandler(animancerMovementStates);
            MovementStatesHandler movementStatesHandler = new MovementStatesHandler(animancerBaseStates);

            facialLayers.Base.GetOrCreateState(_idleExpression).Weight = 1; //TODO make an expression handler
            facialLayers.Override.GetOrCreateState(_blinkExpression).Weight = 1;

            //---- Animancer: Handlers
            animancerMovementInjector.Injection(kinematicData);
            animancerMovementInjector.AddMixer(_defaultMovement.Transition);


            //---- Blinker
            AnimancerLayerBlinker blinker = new AnimancerLayerBlinker(facialLayers.Override);
            _blinkerHandler.InjectHolder(blinker);


            //---- ENTITY (Injections)
            entity.AnimancerHumanoidLayers = humanoidLayers;
            entity.AnimancerFacialLayers = facialLayers;
            entity.BlinkerHandler = _blinkerHandler;

            //---- ENTITY (projections)
            entity.MovementTrackerEvent.AddListener(movementStatesHandler);

            //---- TICKER
            ticker.AddCallbackReceiver(animancerMovementInjector);



            //---- etc.
            _blinkerHandler.StartBlink();
        }

        private void Start()
        {
            _bodyLayersSetter = null;
            _facialLayerSetters = null;

            _idleClip = null;
            _defaultMovement = null;
            _idleExpression = null;
            _blinkExpression = null;
        }

    }
}
