using Animancer;
using Animators;
using UnityEngine;

namespace AnimancerEssentials
{
    public class AnimancerLayerBlinker : IBlinkHolder
    {

        private readonly AnimancerLayer _layer;

        public AnimancerLayerBlinker(AnimancerLayer layer)
        {
            _layer = layer;
        }

        public AnimancerLayerBlinker(AnimancerComponent facialAnimancer, int layerIndex)
        {
            _layer = facialAnimancer.Layers[layerIndex];
        }

        public void AnimateCloseExpression(float eyesWeight)
        {
            _layer.Weight = eyesWeight;
        }
    }
}
