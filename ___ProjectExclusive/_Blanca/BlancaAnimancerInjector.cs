using Animancer;
using Animators;
using Blanca;
using IKEssentials;
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
        [BoxGroup("Body"), SerializeField, HideInPlayMode] private AnimationClip _idleClip = null;
        [BoxGroup("Body"), SerializeField, HideInPlayMode] private MixerTransition2D _defaultMovement = null;
        [BoxGroup("Body"), SerializeField, HideInPlayMode] private AnimationClip _breathing = null;
        [BoxGroup("Body"), SerializeField, HideInPlayMode] private AnimationClip _torchIdle = null;

        [BoxGroup("Facial"), SerializeField, HideInPlayMode] private AnimationClip _idleExpression = null;
        [BoxGroup("Facial"), SerializeField, HideInPlayMode] private LinearMixerTransition _blinkExpressions = null;


        //Persistent
        [TabGroup("Body"), SerializeField]
        private AnimancerMovementInjector animancerMovementInjector = new AnimancerMovementInjector();
        [TabGroup("Facial"), SerializeField]
        private EyesBlinkerHandler _blinkerHandler = new EyesBlinkerHandler();

        [TabGroup("Facial"), SerializeField] 
        private EyesLidHandler lidHandler = null;


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

            humanoidLayers.Addition.Play(_breathing);
            humanoidLayers.UpperHalf.GetOrCreateState(_torchIdle);

            facialLayers.Base.GetOrCreateState(_idleExpression).Weight = 1; //TODO make an expression handler
            facialLayers.Override.GetOrCreateState(_blinkExpressions).Weight = 1;



            //---- Animancer: Handlers
            animancerMovementInjector.Injection(kinematicData);
            animancerMovementInjector.AddMixer(_defaultMovement.Transition);

            BlancaEmotionAnimationsHolder emotionAnimationsHolder 
                = new BlancaEmotionAnimationsHolder(facialLayers);


            //---- Blinker
            AnimancerLayerBlinker blinker = new AnimancerLayerBlinker(facialLayers.Override);
            _blinkerHandler.InjectHolder(blinker);
            _blinkerHandler.Listeners.Add(lidHandler);

            //---- ENTITY (Injections)
            entity.AnimancerHumanoidLayers = humanoidLayers;
            entity.AnimancerFacialLayers = facialLayers;
            entity.BlinkerHandler = _blinkerHandler;
            entity.EmotionAnimations = emotionAnimationsHolder; 

            //---- ENTITY (projections)
            entity.MovementTrackerEvent.AddListener(movementStatesHandler);

            BlinkerEmotionListener blinkerEmotionListener = new BlinkerEmotionListener(_blinkExpressions);
            entity.EmotionsData.Listeners.Add(blinkerEmotionListener);

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
            _blinkExpressions = null;
        }

    }
}
