using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace Blanca
{
    public interface IBlancaEmotions<out T>
    {
        T Happiness { get; }
        T Courage { get; }
        T Curiosity { get; }
    }

    public interface IBlancaEmotionsHandler<in T>
    {
        T Happiness { set; }
        T Courage { set; }
        T Curiosity { set; }
    }
    
    public class BlancaEmotionsData : IBlancaEmotions<float>, IBlancaEmotionsHandler<float>
    {
        [Title("Values")]
        public float Happiness
        {
            get => _happiness;
            set => _happiness = value;
        }

        [ShowInInspector]
        public float Courage
        {
            get => _courage;
            set => _courage = value;
        }

        [ShowInInspector]
        public float Curiosity
        {
            get => _curiosity;
            set => _curiosity = value;
        }

        private const int PredictedListeners = 4;

        [Title("Listeners")]
        [ShowInInspector]
        public readonly List<IEmotionListener> Listeners;

        [ShowInInspector]
        private float _happiness;

        [ShowInInspector]
        private float _courage;
        [ShowInInspector]
        private float _curiosity;

        public enum EmotionType
        {
            Happiness,
            Courage,
            Curiosity
        }

        public BlancaEmotionsData(int predictedAmountOfListeners = PredictedListeners)
        {
            Listeners = new List<IEmotionListener>(predictedAmountOfListeners);

#if UNITY_EDITOR
            Listeners.Add(new DebugListener());
#endif

        }
        /// <summary>
        /// Used on load saved-game 
        /// </summary>
        public BlancaEmotionsData(IBlancaEmotions<float> emotionValues) : this()
        {
            _happiness = emotionValues.Happiness;
            _courage = emotionValues.Courage;
            _curiosity = emotionValues.Curiosity;
        }


        /// <summary>
        /// Used in emotion checks.
        /// <example><br></br>
        /// (eg: checking if the character has courage enough;<br></br>
        /// checking if the character has enough curiosity; etc...)
        /// </example> </summary>
        public void InvokeListeners(EmotionType targetType)
        {
            InvokeListeners(targetType, 0, false);
        }
        
        public void InvokeListenersWithOverride(EmotionType targetType, float targetValue)
        {
            InvokeListeners(targetType,targetValue, true);
        }
        
        [Button]
        private void InvokeListeners(EmotionType targetType, float targetValue, bool changeValue)
        {
            // Instant
            float intensity;
            switch (targetType)
            {
                case EmotionType.Happiness:
                    HandleValue(ref _happiness);
                    foreach (IEmotionListener listener in Listeners)
                    {
                        listener.OnTriggerHappiness(intensity);
                    }
                    break;;
                default:
                case EmotionType.Courage:
                    HandleValue(ref _courage);
                    foreach (IEmotionListener listener in Listeners)
                    {
                        listener.OnTriggerCourage(intensity);
                    }
                    break;
                case EmotionType.Curiosity:
                    HandleValue(ref _curiosity);
                    foreach (IEmotionListener listener in Listeners)
                    {
                        listener.OnTriggerCuriosity(intensity);
                    }
                    break;
            }

            void HandleValue(ref float dataHolder)
            {
                if (changeValue)
                {
                    intensity = targetValue;
                    dataHolder = targetValue;
                }
                else
                {
                    intensity = dataHolder;
                }
            }
        }



#if UNITY_EDITOR
        internal class DebugListener : IEmotionListener
        {
            public void OnTriggerHappiness(float intensity)
            {
                Debug.Log($"Happiness trigger: {intensity}");
            }

            public void OnTriggerCuriosity(float intensity)
            {
                Debug.Log($"Curiosity trigger: {intensity}");
            }

            public void OnTriggerCourage(float intensity)
            {
                Debug.Log($"Courage trigger: {intensity}");
            }
        } 
#endif

    }


    public interface IEmotionListener
    {
        void OnTriggerHappiness(float intensity);
        void OnTriggerCourage(float intensity);
        void OnTriggerCuriosity(float intensity);

    }


}
