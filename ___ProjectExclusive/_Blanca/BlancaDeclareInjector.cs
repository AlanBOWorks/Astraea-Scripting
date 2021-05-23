using AIEssentials;
using Blanca.Actions;
using Blanca.AIEssentials;
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
            entity.CharacterTransformData = _characterTransformHandler.CharacterTransform;

            //----  Kinematic Motor 
            KinematicMotorHandler motorHandler 
                = new KinematicMotorHandler(_motor, parameters);
            KinematicData kinematicData 
                = new KinematicData();
            KinematicDataHandler kinematicDataHandler 
                = new KinematicDataHandler(kinematicData,motorHandler, _motor.transform);

            //----  Kinematic Motor; Filters
            OnStopFilterVelocity onStopFilterVelocity = new OnStopFilterVelocity(motorHandler);
            OnStopFilterRotation onStopFilterRotation = new OnStopFilterRotation(kinematicData, 30f);
            motorHandler.VelocityFilter = onStopFilterVelocity;
            motorHandler.RotationFilter = onStopFilterRotation;

            //---- Brain
            SimpleRequestManager mainTaskManager = new SimpleRequestManager(
                new BlancaActionStopMoving());
            SingleTaskManager lookAtTaskManager = new SingleTaskManager();
            lookAtTaskManager.InjectOnEmptyAction(
                new BlancaActionDefaultLookAt(lookAtTaskManager));
            SingleTaskManager handsTaskManager = new SingleTaskManager(); 
            
            //---- IK: Handlers
            HumanoidIKSolver humanoidIkSolver = new HumanoidIKSolver(_bipedIk,_lookAtAnimator,_irisLookAt);
            humanoidIkSolver.SetUpMainHand(parameters.IsLeftMainHand);
            FeetIKHandler feetIkHandler = new FeetIKHandler(_bipedIk);


            //---- Events
            MovementTrackerEvent movementTrackerEvent = new MovementTrackerEvent(kinematicData);

            //---- <INJECTION: Self Objects> ----
            _motor.CharacterController = motorHandler;

            movementTrackerEvent.AddShortListener(onStopFilterVelocity);
            movementTrackerEvent.AddListener(feetIkHandler);


            //---- <INJECTION: Entity> ----
            entity.KinematicData = kinematicData;
            entity.MotorHandler = motorHandler;


            entity.MainTaskManager = mainTaskManager;
            entity.LookAtTaskManager = lookAtTaskManager;
            entity.HandsTaskManager = handsTaskManager;

            entity.HumanoidIkSolver = humanoidIkSolver;

            entity.MovementTrackerEvent = movementTrackerEvent;

            //---- <INJECTION: Ticker> ----
            ticker.AddCallbackReceiver(_characterTransformHandler);
            ticker.AddCallbackReceiver(kinematicDataHandler);

            //---- etc
            humanoidIkSolver.Head.SetWeight(0); //To avoid the ugly: LookAt World(0,0,0)
        }
        


        private void Start()
        {
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;
            BlancaEntity entity = singleton.Entity;
            entity.MainTaskManager.MoveNext();
            Destroy(this);//Free memory since this no longer is needed
        }

    }
}
