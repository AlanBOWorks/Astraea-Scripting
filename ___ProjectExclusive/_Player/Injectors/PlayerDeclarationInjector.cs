using Animancer;
using AnimancerEssentials;
using FIMSpace.FLook;
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
        [SerializeField] private KinematicCharacterMotor _motor = null;
        [SerializeField] private PlayerBodyRotationHandler rotationHandler = null;

        [Title("IK")]
        [SerializeField] private FullBodyBipedIK _biped = null;
        [SerializeField] private FLookAnimator _lookAt = null;
        [SerializeField] private IrisLookAt _irisLookAt = null;
        [SerializeField] private IKFeetHandler _ikFeetHandler = null;
        [SerializeField] 
        private FingersSolverConstructor _fingersSolverConstructor = new FingersSolverConstructor();

        [Title("Transform")]
        [SerializeField] private PlayerTransformHandler _playerTransformHandler = new PlayerTransformHandler();

        [Title("Inputs")] 
        [SerializeField] private PlayerInputHandler _inputHandler = new PlayerInputHandler();
        [SerializeField] private InputRotation _inputRotation = new InputRotation(); //this rotates the camera


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
            KinematicData rawKinematicData
                = new KinematicData();
            KinematicDataHandler kinematicDataHandler
                = new KinematicDataHandler(rawKinematicData, motorHandler, playerTransform.MeshRoot);
            // This is because the player's rotation is based in the camera instead of the player's transform.root
            ComposedKinematicData playerKinematicData = new ComposedKinematicData(rawKinematicData,rotationHandler);

            //----  Kinematic Motor; Filters
            OnStopFilterVelocity onStopFilterVelocity = new OnStopFilterVelocity(4);
            motorHandler.VelocityFilters.Enqueue(onStopFilterVelocity);

            //---- IKs
            FullHumanoidIKSolver humanoidIk =
                new FullHumanoidIKSolver(_biped, _lookAt, _irisLookAt, _fingersSolverConstructor);

            //---- Inputs
            _inputHandler.Injection(playerTransform);

            PlayerInputData inputData = _inputHandler.InputData;
            _inputRotation.Injection(parameters,playerTransform,inputData);


            //---- Events
            //Uses the raw version because it's 'faster', but it could use the composed one as well
            MovementTrackerEvent movementTrackerEvent = new MovementTrackerEvent(rawKinematicData);

            //---- <INJECTION: Self Objects> ----
            _motor.CharacterController = motorHandler;
            rotationHandler.Injection(playerTransform,inputData);

            movementTrackerEvent.AddShortListener(onStopFilterVelocity);
            movementTrackerEvent.AddListener(_ikFeetHandler);

            rotationHandler.AddListener(_ikFeetHandler);

            //---- <INJECTION: Entity> ----
            entity.KinematicData = playerKinematicData;
            entity.MotorHandler = motorHandler;
            entity.InputData = _inputHandler.InputData;
            entity.MovementTrackerEvent = movementTrackerEvent;
            entity.HumanoidIkSolver = humanoidIk;

            //---- <INJECTION: Ticker> ----
            ticker.AddCallbackReceiver(_playerTransformHandler);
            ticker.AddCallbackReceiver(kinematicDataHandler);
            ticker.AddCallbackReceiver(_inputHandler);
            ticker.AddCallbackReceiver(_inputRotation);



            _fingersSolverConstructor = null; //To avoid generating new solvers on accident
        }
    }
}
