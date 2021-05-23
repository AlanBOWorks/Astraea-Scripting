using System;
using Animancer;
using UnityEngine;

namespace AnimancerEssentials
{
    public class AnimancerHumanoid<T> : IAnimancerHumanoidStructure<T>
    {
        public const int BaseIndex = 0;
        public const int UpperHalfIndex = BaseIndex +1;
        public const int PriorityUpperHalfIndex = UpperHalfIndex +1;
        public const int OverrideIndex = PriorityUpperHalfIndex +1;

        public const int SerializationLength = OverrideIndex + 1;

        public T Base => SerializedData[BaseIndex];
        public T UpperHalf => SerializedData[UpperHalfIndex];
        public T PriorityUpperHalf => SerializedData[PriorityUpperHalfIndex];
        public T Override => SerializedData[OverrideIndex];
        public T[] SerializedData { get; protected set; }

        public bool IsValid(T[] check)
        {
            return check.Length == SerializationLength;
        }

        public AnimancerHumanoid()
        {
            SerializedData = new T[SerializationLength];
        }

        public AnimancerHumanoid(T[] elements)
        {
            if (IsValid(elements))
            {
                SerializedData = elements;
            }
            else
            {
                throw new ArgumentException(
                    $"The {elements}'s Length is not valid ({elements.Length} != {SerializationLength})");

            }
        }
    }

    public abstract class SerializedAnimancerHumanoid<T> : AnimancerHumanoid<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private T _base;
        [SerializeField] private T _upperHalf;
        [SerializeField] private T _priorityUpperHalf;
        [SerializeField] private T _override;


        public void OnBeforeSerialize()
        {
            _base = Base;
            _upperHalf = UpperHalf;
            _priorityUpperHalf = PriorityUpperHalf;
            _override = Override;
        }

        public void OnAfterDeserialize()
        {
            SerializedData = new[]
            {
                _base,
                _upperHalf,
                _priorityUpperHalf,
                _override
            };
        }
    }

    public class AnimancerFacial<T> : IAnimancerFacialStructure<T>
    {
        public const int BaseIndex = 0;
        public const int AdditionIndex = BaseIndex + 1;
        public const int OverrideIndex = AdditionIndex + 1;
        public const int SerializationLength = OverrideIndex + 1;

        public T Base => SerializedData[BaseIndex];
        public T Addition => SerializedData[AdditionIndex];
        public T Override => SerializedData[OverrideIndex];
        public T[] SerializedData { get; protected set; }

        public bool IsValid(T[] check)
        {
            return check.Length == SerializationLength;
        }


        public AnimancerFacial()
        {
            SerializedData = new T[SerializationLength];
        }
        public AnimancerFacial(T[] elements)
        {
            if (IsValid(elements))
                SerializedData = elements;
            else
            {
                throw new ArgumentException(
                    $"The {elements}'s Length is not valid ({elements.Length} != {SerializationLength})");
            }
        }
    }

    public abstract class SerializedAnimancerFacial<T> : AnimancerFacial<T>, ISerializationCallbackReceiver
    {
        [SerializeField] private T _base;
        [SerializeField] private T _addition;
        [SerializeField] private T _override;
        public void OnBeforeSerialize()
        {
            _base = Base;
            _addition = Addition;
            _override = Override;
        }

        public void OnAfterDeserialize()
        {
            SerializedData = new[]
            {
                _base,
                _addition,
                _override
            };
        }
    }

    public class AnimancerHumanoidLayers : AnimancerHumanoid<AnimancerLayer>
    {
        public AnimancerHumanoidLayers(AnimancerLayer[] elements) : base(elements)
        {}
    }

    public class AnimancerFacialLayers : AnimancerFacial<AnimancerLayer>
    {
        public AnimancerFacialLayers(AnimancerLayer[] elements) : base(elements)
        {}
    }

    [Serializable]
    public class AnimancerHumanoidLayersSetter : SerializedAnimancerHumanoid<LayerSettings>
    {
        public AnimancerHumanoidLayers GenerateLayers(AnimancerComponent animancer)
        {
            AnimancerLayer[] layers = new AnimancerLayer[SerializationLength];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = SerializedData[i].InitializeLayer(animancer, i);
            }

            return new AnimancerHumanoidLayers(layers);
        }
    }

    [Serializable]
    public class AnimancerFacialLayersSetter : SerializedAnimancerFacial<LayerSettings>
    {
        public AnimancerFacialLayers GenerateLayers(AnimancerComponent animancer)
        {
            AnimancerLayer[] layers = new AnimancerLayer[SerializationLength];
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = SerializedData[i].InitializeLayer(animancer, i);
            }

            return new AnimancerFacialLayers(layers);
        }
    }
}
