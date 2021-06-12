using ___ProjectExclusive;
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
        [SerializeField] private BlancaTransformHandler blancaTransform = new BlancaTransformHandler();

        [Title("IK Handlers")] 
        [SerializeField] private FullBodyBipedIK _bipedIk = null;
        [SerializeField] private FLookAnimator _lookAtAnimator = null;
        [SerializeField] private IrisLookAt _irisLookAt = null;
        [SerializeField] private IKFeetHandler _ikFeetHandler = null;
        [Title("IK - Fingers")] 
        [SerializeField]
        private FingersSolverConstructor _fingersSolverConstructor = new FingersSolverConstructor();

        [Title("Transforms")]
        [SerializeField] 
        private BlancaPersonality personality = new BlancaPersonality();

        [Title("Parameters - Kinematic")] 
        [SerializeField]
        private CurveRotationFilter curveRotationFilter = new CurveRotationFilter();
        [SerializeField] 
        private CurveVelocityFilter curveVelocityFilter = new CurveVelocityFilter();
        [SerializeField, Range(-1,1)] 
        private float rotationTriggerDotThreshold = .4f;
        [Title("Parameters - IK")]
        [SerializeField, SuffixLabel("%/m^2")]
        private AnimationCurve sqrLookAtTargetModifier = new AnimationCurve(
            new Keyframe(.1f,4f),new Keyframe(1,0));

        public override void DoInjection()
        {
            //GETS
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
            BlancaEntity entity = singleton.Entity;
            BlancaParametersVariable parameters = singleton.Parameters;

            TickerHandler ticker = entity.TickerHandler;

            //---- Transforms
            ICharacterTransformData characterTransformData = blancaTransform.CharacterTransform;
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

            motorHandler.VelocityFilters.Enqueue(onStopFilterVelocity);
            motorHandler.VelocityFilters.Enqueue(curveVelocityFilter);
            motorHandler.VelocityFilters.Enqueue(new SmoothVelocityFilter(8));

            motorHandler.RotationFilters.Enqueue(curveRotationFilter);
            motorHandler.RotationFilters.Enqueue(new SmoothRotationFilter());

            //---- Kinematic Motor; Final
            BlancaMotorControl motorControl 
                = new BlancaMotorControl(motorHandler,velocityControls,rotationControls);

            //---- Brain
            
            
            //---- IK: Handlers
            FullHumanoidIKSolver humanoidIkSolver 
                = new FullHumanoidIKSolver(_bipedIk,
                    _lookAtAnimator, _irisLookAt,
                    _fingersSolverConstructor, true);

            //---- IK: Controls
            BlancaLookAtControlHolder lookAtControls 
                = new BlancaLookAtControlHolder(characterTransformData,kinematicData);
            BlancaIKControl ikControls = new BlancaIKControl(
                characterTransformData,
                humanoidIkSolver.HeadIkSolver,
                lookAtControls);

            lookAtControls.UpdateLookAtTargetCurve(sqrLookAtTargetModifier);

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
            ticker.AddCallbackReceiver(blancaTransform);
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
