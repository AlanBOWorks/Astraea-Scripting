using AIEssentials;
using Animancer;
using KinematicCharacterController;
using PlayerEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerComponentsInjector : MonoInjector, ISerializationCallbackReceiver
    {
        [Title("Components")]
        [SerializeField, HideInPlayMode] private AnimancerComponent _bodyAnimancer = null;
        [SerializeField, HideInPlayMode] private VelocityPathCalculator _pathHelper = null;


        [Title("Ticker")]
        [SerializeField] private TickerHandler _ticker = null;



        [Title("Variables (For Singleton)")]
        [SerializeField] private PlayerParametersVariable _parametersVariable = null;
        [SerializeField] private TransformDataVariable _transformDataVariable = null;

        public override void DoInjection()
        {
            PlayerEntity entity = PlayerEntitySingleton.Instance.Entity;

            //---- <Section> ---- Path
            entity.MainPathHelper = _pathHelper;

            //---- <Section> ---- Animancer
            entity.BodyAnimancer = _bodyAnimancer;

            entity.TickerHandler = _ticker;


            Destroy(this); //Free memory since this no longer is needed
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            PlayerEntitySingleton singleton = PlayerEntitySingleton.Instance;
            
            singleton.Parameters = _parametersVariable;
            singleton.TransformData = _transformDataVariable;
        }
    }
}
