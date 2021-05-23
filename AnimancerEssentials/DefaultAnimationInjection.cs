using System;
using Animancer;
using UnityEngine;

namespace AnimancerEssentials
{
    /// <summary>
    /// Used mainly to avoid the ugly null Animation by injecting a default animation as a Base
    /// </summary>
    [Serializable]
    public class DefaultAnimationInjection 
    {
        [SerializeField] private AnimancerComponent _animancer = null;
        public AnimancerComponent Animancer
        {
            set => _animancer = value;
        }
        [SerializeField] private AnimationClip _clip = null;

        public AnimancerState Initialize()
        {
            AnimancerLayer baseLayer = _animancer.Layers[0];
            AnimancerState state = baseLayer.GetOrCreateState(_clip);

            state.Play();
            return state;
        }

    }
}
