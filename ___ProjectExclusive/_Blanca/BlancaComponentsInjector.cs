﻿using AIEssentials;
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
        [SerializeField, HideInPlayMode] private VelocityPathCalculator[] _pathCalculators = new VelocityPathCalculator[0];


        [Title("Variables (For Singleton)")]
        [SerializeField] private BlancaParametersVariable _parametersVariable = null;
        [SerializeField] private TransformDataVariable _transformDataVariable = null;

        public override void DoInjection()
        {
            BlancaEntity entity = BlancaEntitySingleton.Instance.Entity;


            //---- PathCalculators
            IPathCalculator[] helpers
                = PathCalculatorsConstructor.FragmentPathCalculators(_pathCalculators, out IPathCalculator mainPathCalculator);

            entity.MainPathCalculator = mainPathCalculator;
            entity.PathCalculatorsHelpers = helpers;

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
