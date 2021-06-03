using System;
using SMaths;
using UnityEngine;

namespace Blanca
{

    [Serializable]
    public class BlancaPersonality : IBlancaEmotions<SRange>
    {
        [SerializeField] private SRange happinessSpectrum = new SRange(-.5f, .3f);
        [SerializeField] private SRange courageSpectrum = new SRange(-.5f, .6f);
        [SerializeField] private SRange curiositySpectrum = new SRange(-.8f, .5f);


        public SRange Happiness => happinessSpectrum;
        public SRange Courage => courageSpectrum;
        public SRange Curiosity => curiositySpectrum;
    }
}
