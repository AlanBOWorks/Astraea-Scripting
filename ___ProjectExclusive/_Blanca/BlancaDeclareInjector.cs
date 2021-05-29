using AIEssentials;
using FIMSpace.FLook;
using IKEssentials;
using KinematicCharacterController;
using KinematicEssentials;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    
    public class BlancaDeclareInjector : MonoInjector
    {
        [Title("References")]
        [SerializeField, HideInPlayMode] private KinematicCharacterMotor _motor = null;

        [Title("Transforms")] 
        [SerializeField, HideInPlayMode]
        private CharacterTransformHandler _characterTransformHandler = new CharacterTransformHandler();

        [Title("IK Handlers")] 
        [SerializeField, HideInPlayMode] private FullBodyBipedIK _bipedIk = null;
        [SerializeField, HideInPlayMode] private FLookAnimator _lookAtAnimator = null;
        [SerializeField, HideInPlayMode] private IrisLookAt _irisLookAt = null;



        public override void DoInjection()
        {
            //GETS
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
            BlancaEntity entity = singleton.Entity;
            BlancaParametersVariable parameters = singleton.Parameters;

            TickerHandler ticker = entity.TickerHandler;

            //---- Transforms
            ICharacterTransformData characterTransformData = _characterTransformHandler.CharacterTransform;
            entity.CharacterTransformData = characterTransformData;


            //----  Kinematic Motor 
            KinematicMotorHandler motorHandler 
                = new KinematicMotorHandler(_motor, parameters);
            KinematicData kinematicData 
                = new KinematicData();
            KinematicDataHandler kinematicDataHandler 
                = new KinematicDataHandler(kinematicData,motorHandler, _motor.transform);

            //---- Kinematic Controls
            BlancaVelocityControlHolder velocityControls = 
                new BlancaVelocityControlHolder(entity.PathControls);
            BlancaRotationControlHolder rotationControls =
                new BlancaRotationControlHolder(kinematicData);

            //----  Kinematic Motor; Filters
            OnStopFilterVelocity onStopFilterVelocity = new OnStopFilterVelocity(motorHandler);
            OnStopFilterRotation onStopFilterRotation = new OnStopFilterRotation(kinematicData, 30f);
            motorHandler.VelocityFilter = onStopFilterVelocity;
            motorHandler.RotationFilter = onStopFilterRotation;

            //---- Kinematic Motor; Final
            BlancaMotorControl motorControl 
                = new BlancaMotorControl(motorHandler,velocityControls,rotationControls);

            //---- Brain
            
            
            //---- IK: Handlers
            HumanoidIKSolver humanoidIkSolver = new HumanoidIKSolver(_bipedIk,_lookAtAnimator,_irisLookAt);
            humanoidIkSolver.SetUpMainHand(parameters.IsLeftMainHand);
            FeetIKHandler feetIkHandler = new FeetIKHandler(_bipedIk);

            //---- IK: Controls
            BlancaLookAtControlHolder lookAtControls 
                = new BlancaLookAtControlHolder(characterTransformData,kinematicData);
            BlancaIKControl ikControls = new BlancaIKControl(
                characterTransformData,
                humanoidIkSolver.HeadIkSolver,
                lookAtControls);

            //---- Events
            MovementTrackerEvent movementTrackerEvent = new MovementTrackerEvent(kinematicData);



            //---- <INJECTION: Self Objects> ----
            _motor.CharacterController = motorHandler;

            movementTrackerEvent.AddShortListener(onStopFilterVelocity);
            movementTrackerEvent.AddListener(feetIkHandler);


            //---- <INJECTION: Entity> ----
            entity.KinematicData = kinematicData;
            entity.MotorHandler = motorHandler;
            entity.VelocityControls = velocityControls;
            entity.RotationControls = rotationControls;
            entity.LookAtControls = lookAtControls;

            entity.HumanoidIkSolver = humanoidIkSolver;

            entity.MovementTrackerEvent = movementTrackerEvent;

            //---- <INJECTION: Ticker> ----
            ticker.AddCallbackReceiver(motorControl);
            ticker.AddCallbackReceiver(_characterTransformHandler);
            ticker.AddCallbackReceiver(kinematicDataHandler);
            ticker.AddCallbackReceiver(ikControls);

        }
        


        private void Start()
        {
            /*BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
            BlancaEntity entity = singleton.Entity;*/
            Destroy(this);//Free memory since this no longer is needed
        }

    }
}
