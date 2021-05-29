using AIEssentials;
using Animancer;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    
    public class BlancaComponentsInjector : MonoInjector, ISerializationCallbackReceiver
    {
        [Title("Components")] 
        [SerializeField, HideInPlayMode] private AnimancerComponent _bodyAnimancer = null;
        [SerializeField, HideInPlayMode] private AnimancerComponent _facialAnimancer = null;
        [SerializeField, HideInPlayMode] private TickerHandler _ticker = null;


        [Title("Path Calculators")]
        [SerializeField, HideInPlayMode] 
        private SerializedBlancaPathControls _pathControls = new SerializedBlancaPathControls();


        [Title("Variables (For Singleton)")]
        [SerializeField] private BlancaParametersVariable _parametersVariable = null;
        [SerializeField] private TransformDataVariable _transformDataVariable = null;

        public override void DoInjection()
        {
            BlancaEntity entity = BlancaEntitySingleton.Instance.Entity;


            //---- PathCalculators
            entity.PathControls = _pathControls;
           

            //---- <Section> ---- Animancer
            entity.BodyAnimancer = _bodyAnimancer;
            entity.FacialAnimancer = _facialAnimancer;

            entity.TickerHandler = _ticker;


        }


        private void Start()
        {
            Destroy(this); //Free memory since this no longer is needed
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            BlancaEntitySingleton singleton = BlancaEntitySingleton.Instance;;
            singleton.Parameters = _parametersVariable;
            singleton.TransformData = _transformDataVariable;
        }
    }
}
