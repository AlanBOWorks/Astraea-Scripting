using Animancer;
using AnimancerEssentials;
using UnityEngine;

namespace Blanca
{
    public class BlancaEmotionAnimationsHolder : IBlancaEmotions<LinearMixerState>, IBlancaEmotionsHandler<LinearMixerState>,
        IEmotionListener
    {
        public BlancaEmotionAnimationsHolder(AnimancerFacialLayers layers)
        {
            _facialLayers = layers;
            NeutralState = layers.Base.CurrentState;
        }

        private readonly AnimancerFacialLayers _facialLayers;
        public AnimancerState NeutralState;

        private AnimancerLayer GetTargetLayer() => _facialLayers.Base;

        public LinearMixerState Happiness { get; set; }
        public LinearMixerState Courage { get; set; }
        public LinearMixerState Curiosity { get; set; }

        public void OnTriggerHappiness(float intensity)
        {
            HandleState(Happiness,intensity);
        }

        public void OnTriggerCourage(float intensity)
        {
            HandleState(Courage,intensity);
        }

        public void OnTriggerCuriosity(float intensity)
        {
            HandleState(Curiosity,intensity);
        }

        private static void HandleState(LinearMixerState state, float intensity)
        {
            if(state is null) return;
            float stateWeight = Mathf.Abs(intensity);
            state.TargetWeight = stateWeight;
            state.Parameter = intensity;
        }

    }
}
