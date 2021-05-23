using Animancer;
using AnimancerEssentials;
using IKEssentials;
using KinematicCharacterController;
using KinematicEssentials;
using PlayerEssentials;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerDeclarationInjector : MonoInjector
    {
        [Title("References")]
        [SerializeField, HideInPlayMode] private KinematicCharacterMotor _motor = null;
        [SerializeField, HideInPlayMode] private PlayerBodyRotationOnLimits _rotationOnLimits = null;
        [SerializeField, HideInPlayMode] private FullBodyBipedIK _biped = null;

        [Title("Transform")]
        [SerializeField, HideInPlayMode] private PlayerTransformHandler _playerTransformHandler = new PlayerTransformHandler();

        [Title("Inputs")] 
        [SerializeField] private PlayerInputHandler _inputHandler = new PlayerInputHandler();
        [SerializeField] private InputRotation _inputRotation = new InputRotation();




        public override void DoInjection()
        {
            // GETS
            PlayerEntity entity = PlayerEntitySingleton.Instance.Entity;
            PlayerParametersVariable parameters = PlayerEntitySingleton.Instance.Parameters;
            IPlayerTransformData playerTransform = _playerTransformHandler.CharacterTransform;
            TickerHandler ticker = entity.TickerHandler;

            //---- Transform
            entity.CharacterTransformData = playerTransform;

            //----  Kinematic Motor 
            KinematicMotorHandler motorHandler
                = new KinematicMotorHandler(_motor, parameters);
            KinematicData kinematicData
                = new KinematicData();
            KinematicDataHandler kinematicDataHandler
                = new KinematicDataHandler(kinematicData, motorHandler, playerTransform.MeshRoot);

            //----  Kinematic Motor; Filters
            OnStopFilterVelocity onStopFilterVelocity = new OnStopFilterVelocity(motorHandler);
            motorHandler.VelocityFilter = onStopFilterVelocity;

            //---- IKs
            FeetIKHandler feetIkHandler = new FeetIKHandler(_biped);

            //---- Inputs
            _inputHandler.Injection(playerTransform);

            PlayerInputData inputData = _inputHandler.InputData;
            _inputRotation.Injection(parameters,playerTransform,inputData);


            //---- Events
            MovementTrackerEvent movementTrackerEvent = new MovementTrackerEvent(kinematicData);


            //---- <INJECTION: Self Objects> ----
            _motor.CharacterController = motorHandler;
            _rotationOnLimits.Injection(playerTransform,inputData);

            movementTrackerEvent.AddShortListener(onStopFilterVelocity);
            movementTrackerEvent.AddListener(feetIkHandler);

            //---- <INJECTION: Entity> ----
            entity.KinematicData = kinematicData;
            entity.MotorHandler = motorHandler;
            entity.InputData = _inputHandler.InputData;
            entity.MovementTrackerEvent = movementTrackerEvent;

            //---- <INJECTION: Ticker> ----
            ticker.AddCallbackReceiver(_playerTransformHandler);
            ticker.AddCallbackReceiver(kinematicDataHandler);
            ticker.AddCallbackReceiver(_inputHandler);
            ticker.AddCallbackReceiver(_inputRotation);
            
        }
    }
}
