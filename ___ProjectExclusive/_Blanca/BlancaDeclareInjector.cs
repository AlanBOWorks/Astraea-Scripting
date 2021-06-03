using AIEssentials;
using FIMSpace.FLook;
using IKEssentials;
using KinematicCharacterController;
using KinematicEssentials;
using RootMotion.FinalIK;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Blanca
{
    
    public class BlancaDeclareInjector : MonoInjector
    {
        [Title("References")]
        [SerializeField] private KinematicCharacterMotor _motor = null;

        [Title("Transforms")] 
        [SerializeField]
        private CharacterTransformHandler _characterTransformHandler = new CharacterTransformHandler();

        [Title("IK Handlers")] 
        [SerializeField] private FullBodyBipedIK _bipedIk = null;
        [SerializeField] private FLookAnimator _lookAtAnimator = null;
        [SerializeField] private IrisLookAt _irisLookAt = null;
        [SerializeField] private IKFeetHandler _ikFeetHandler = null;

        [Title("Transforms")]
        [SerializeField] 
        private BlancaPersonality personality = new BlancaPersonality();

        [Title("Parameters")] 
        [SerializeField]
        private CurveRotationFilter curveRotationFilter = new CurveRotationFilter();
        [SerializeField, Range(-1,1)] 
        private float rotationTriggerDotThreshold = .4f;

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
            OnStopFilterVelocity onStopFilterVelocity = new OnStopFilterVelocity();
            OnStopFilterRotation onStopFilterRotation = new OnStopFilterRotation(kinematicData, 30f);

            motorHandler.VelocityFilters.Enqueue(onStopFilterVelocity);

            motorHandler.RotationFilters.Enqueue(onStopFilterRotation);
            motorHandler.RotationFilters.Enqueue(curveRotationFilter);

            //---- Kinematic Motor; Final
            BlancaMotorControl motorControl 
                = new BlancaMotorControl(motorHandler,velocityControls,rotationControls);

            //---- Brain
            
            
            //---- IK: Handlers
            HumanoidIKSolver humanoidIkSolver = new HumanoidIKSolver(_bipedIk,_lookAtAnimator,_irisLookAt);
            humanoidIkSolver.SetUpMainHand(parameters.IsLeftMainHand);

            //---- IK: Controls
            BlancaLookAtControlHolder lookAtControls 
                = new BlancaLookAtControlHolder(characterTransformData,kinematicData);
            BlancaIKControl ikControls = new BlancaIKControl(
                characterTransformData,
                humanoidIkSolver.HeadIkSolver,
                lookAtControls);

            //---- Emotions
            BlancaEmotionsData emotionsData = new BlancaEmotionsData(personality);

            //---- Events
            MovementTrackerEvent movementTrackerEvent = new MovementTrackerEvent(kinematicData);
            RotationTrackerEvent rotationTrackerEvent = new RotationTrackerEvent(kinematicData, rotationTriggerDotThreshold);



            //---- <INJECTION: Self Objects> ----
            _motor.CharacterController = motorHandler;

            movementTrackerEvent.AddShortListener(onStopFilterVelocity);
            movementTrackerEvent.AddListener(_ikFeetHandler);

            rotationTrackerEvent.AddListener(_ikFeetHandler);

            //---- <INJECTION: Entity> ----
            entity.KinematicData = kinematicData;
            entity.MotorHandler = motorHandler;
            entity.VelocityControls = velocityControls;
            entity.RotationControls = rotationControls;
            entity.LookAtControls = lookAtControls;

            entity.HumanoidIkSolver = humanoidIkSolver;

            entity.EmotionsData = emotionsData;

            entity.MovementTrackerEvent = movementTrackerEvent;
            entity.RotationTrackerEvent = rotationTrackerEvent;

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
