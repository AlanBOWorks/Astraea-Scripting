using Animancer;
using AnimancerEssentials;
using UnityEngine;

namespace Blanca
{
    public class BlinkerEmotionListener : IEmotionListener
    {
        private readonly LinearMixerState _state;
        public BlinkerEmotionListener(LinearMixerState.Transition transition)
        {
            _state = transition.State;
        }

        public BlinkerEmotionListener(LinearMixerTransition transition)
        {
            _state = transition.Transition.State;
        }

        public void OnTriggerHappiness(float intensity)
        {
            intensity = Mathf.Clamp01(intensity);
            _state.Parameter = intensity;
        }

        public void OnTriggerCourage(float intensity)
        {

        }

        public void OnTriggerCuriosity(float intensity)
        {

        }
    }
}
