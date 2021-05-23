using Animancer;
using AnimancerEssentials;
using KinematicEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerAnimancerInjector : MonoInjector
    {
        [Title("Animancer Settings")]
        [SerializeField, HideInPlayMode] 
        private AnimancerHumanoidLayersSetter _animancerLayerSetters = new AnimancerHumanoidLayersSetter();
        [SerializeField, HideInPlayMode] private AnimationClip _idleClip = null;
        [SerializeField, HideInPlayMode] private MixerTransition2D _defaultMovement = null;
        [SerializeField] 
        AnimancerMovementInjector _animancerMovementInjector = new AnimancerMovementInjector();

        public override void DoInjection()
        {
            // GETS
            PlayerEntity entity = PlayerEntitySingleton.Instance.Entity;
            AnimancerComponent bodyAnimancer = entity.BodyAnimancer;
            TickerHandler ticker = entity.TickerHandler;
            KinematicData kinematicData = entity.KinematicData;


            //---- Animancer
            AnimancerHumanoidLayers humanoidLayers
                = _animancerLayerSetters.GenerateLayers(bodyAnimancer);


            //---- Animancer: states
            AnimancerMovementStates animancerMovementStates = new AnimancerMovementStates(
                _idleClip, _defaultMovement.Transition, humanoidLayers.Base);
            AnimancerBaseStatesHandler animancerBaseStates
                = new AnimancerBaseStatesHandler(animancerMovementStates);
            MovementStatesHandler movementStatesHandler = new MovementStatesHandler(animancerBaseStates);

            //---- Animancer: Handlers
            _animancerMovementInjector.Injection(kinematicData);
            _animancerMovementInjector.AddMixer(_defaultMovement.Transition);

            //---- ENTITY (projections)
            entity.MovementTrackerEvent.AddListener(movementStatesHandler);


            //---- <INJECTION: Ticker> ----
            ticker.AddCallbackReceiver(_animancerMovementInjector);

            Destroy(this);
        }
    }
}
