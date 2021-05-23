using System;
using Animancer;
using UnityEngine;

namespace AnimancerEssentials
{
    [Serializable]
    public class AnimancerLayerSetters
    {
        private const int AnimancerDefaultLayerAmount = 4;
        public LayerSettings[] Settings;

        public AnimancerLayerSetters(int layerAmount = AnimancerDefaultLayerAmount)
        {
            Settings = new LayerSettings[layerAmount];
        }

        public AnimancerLayer[] GenerateLayers(AnimancerComponent animancer)
        {
            AnimancerLayer[] layers = new AnimancerLayer[Settings.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = Settings[i].InitializeLayer(animancer, i);
            }

            return layers;
        }
    }

    [Serializable]
    public class LayerSettings
    {
        public bool IsAdditive;
        public AvatarMask Mask;
        [Range(0, 1)]
        public float InitialWeight;

        /// <summary>
        /// Sets the animancer layer and returns it; It's made in this way for the <seealso cref="AnimancerLayerSetters"/>
        /// generates an array from it
        /// </summary>
        public AnimancerLayer InitializeLayer(AnimancerComponent animancer, int layerIndex)
        {
            AnimancerLayer layer = animancer.Layers[layerIndex];
            layer.IsAdditive = IsAdditive;
            if (Mask != null)
                layer.SetMask(Mask);

            layer.SetWeight(InitialWeight);

            return layer;
        }
    }
}
