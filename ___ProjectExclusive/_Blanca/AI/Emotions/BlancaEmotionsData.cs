using System;
using System.Collections.Generic;
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

    
    // It will be used as a reference type (normally by the director
    // or an event manager waits for an specific emotion)
    public class BlancaEmotionData
    {
        [ShowInInspector]
        private float _value;
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                _tracker.TrackValue(_value);
            }
        }

        private readonly EmotionTracker _tracker;

        public BlancaEmotionData(IEmotionHolderListener holderListener, SRange trackRange, float currentValue = 0)
        {
            _tracker = new EmotionTracker(trackRange,currentValue,holderListener);
        }


        internal class EmotionTracker
        {
            public SRange TriggerRange;
            public readonly IEmotionHolderListener Listener;
            public EmotionTracker(SRange trackRange, float currentValue, IEmotionHolderListener listener)
            {
                TriggerRange = trackRange;
                this.Listener = listener;
                SetState();
                void SetState()
                {
                    if (currentValue >= TriggerRange.maxValue)
                    {
                        _state = State.Positive;
                        return;
                    }

                    if (currentValue <= TriggerRange.minValue)
                    {
                        _state = State.Negative;
                        return;
                    }

                    _state = State.Neutral;
                }
            }

            public enum State
            {
                Positive,
                Neutral,
                Negative
            }
            private State _state;
            public void TrackValue(float target) 
            {
                switch (_state)
                {
                    case State.Positive:
                        CheckNegative(target);
                        break;
                    default:
                    case State.Neutral:
                        if (CheckPositive(target)) break;
                        CheckNegative(target);
                        break;
                    case State.Negative:
                        CheckPositive(target);
                        break;
                }
            }

            /// <returns>Just to avoid checking negative</returns>
            private bool CheckPositive(float target)
            {
                if (target <= TriggerRange.maxValue) return false;
                Listener.OnPositiveEmotion();
                _state = State.Positive;
                return true;
            }


            private void CheckNegative(float target)
            {
                if (target >= TriggerRange.minValue) return;
                Listener.OnNegativeEmotion();

                _state = State.Negative;
            }
        }
    }
    public class BlancaEmotionsData : IBlancaEmotions<BlancaEmotionData>, IBlancaEmotions<float>,
        IEmotionHolderListener
    {
        [ShowInInspector]
        public BlancaEmotionData Happiness { get; }
        [ShowInInspector]
        public BlancaEmotionData Courage { get;}
        [ShowInInspector]
        public BlancaEmotionData Curiosity { get; }

        float IBlancaEmotions<float>.Happiness => Happiness.Value;
        float IBlancaEmotions<float>.Courage => Courage.Value;
        float IBlancaEmotions<float>.Curiosity => Curiosity.Value;

        public readonly List<IEmotionsListener> EmotionsListeners;


        private const int PredictedListeners = 4;
        public BlancaEmotionsData(IBlancaEmotions<SRange> emotionParameters)
        {
            Happiness = new BlancaEmotionData(this,emotionParameters.Happiness);
            Courage = new BlancaEmotionData(this, emotionParameters.Courage);
            Curiosity = new BlancaEmotionData(this, emotionParameters.Curiosity);

            EmotionsListeners = new List<IEmotionsListener>(PredictedListeners);
        }

        /// <summary>
        /// Used on load saved-game 
        /// </summary>
        public BlancaEmotionsData(IBlancaEmotions<SRange> emotionParameters, IBlancaEmotions<float> emotionValues)
        {
            Happiness = new BlancaEmotionData(this, emotionParameters.Happiness, emotionValues.Happiness);
            Courage = new BlancaEmotionData(this, emotionParameters.Courage, emotionValues.Courage);
            Curiosity = new BlancaEmotionData(this, emotionParameters.Curiosity, emotionValues.Curiosity);
        }

        private void InvokeListeners()
        {
            foreach (IEmotionsListener listener in EmotionsListeners)
            {
                listener.OnStateSwitch(this);
            }
        }

        public void OnPositiveEmotion() => InvokeListeners();

        public void OnNegativeEmotion() => InvokeListeners();
    }


    public interface IEmotionsListener
    {
        void OnStateSwitch(IBlancaEmotions<float> currentEmotions);
    }
    public interface IEmotionHolderListener
    {
        void OnPositiveEmotion();
        void OnNegativeEmotion();
    }
}
