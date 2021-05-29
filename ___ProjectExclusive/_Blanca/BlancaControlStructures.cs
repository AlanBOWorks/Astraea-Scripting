using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blanca
{
    public interface IBlancaMovementStructure<out T> : IBlancaPathStructure<T>
    {
        T Copy { get; }
    }

    public interface IBlancaPathStructure <out T>
    {
        T Base { get; }
        T Lead { get; }
        T ToPlayer { get; }
    }

    public class BlancaMovementStructure<T> : IBlancaMovementStructure<T>
    {
        [ShowInInspector]
        public T Base
        {
            get => Elements[BaseIndex];
            protected set => Elements[BaseIndex] = value;
        }
        [ShowInInspector]
        public T Lead
        {
            get => Elements[LeadIndex];
            protected set => Elements[LeadIndex] = value;
        }
        [ShowInInspector]
        public T ToPlayer
        {
            get => Elements[ToPlayerIndex];
            protected set => Elements[ToPlayerIndex] = value;
        }
        [ShowInInspector]
        public T Copy
        {
            get => Elements[CopyIndex];
            protected set => Elements[CopyIndex] = value;
        }

        public List<T> Elements { get; protected set; }

        public BlancaMovementStructure()
        {
            Elements = new List<T>(MovementTypesAmount);
        }

        public BlancaMovementStructure(IBlancaMovementStructure<T> wrapper)
        : this()
        {
            Elements.Add(wrapper.Base);
            Elements.Add(wrapper.Lead);
            Elements.Add(wrapper.ToPlayer);
            Elements.Add(wrapper.Copy);
        }

        public const int BaseIndex = 0;
        public const int LeadIndex = BaseIndex + 1;
        public const int ToPlayerIndex = LeadIndex + 1;
        public const int CopyIndex = ToPlayerIndex + 1;

        public const int MovementTypesAmount = CopyIndex+1;

        /// <summary>
        /// Used when it's not sure if the <see cref="Copy"/> element is added;
        /// </summary>
        public void SafeAddElement(T element)
        {
            if(Elements.Count < MovementTypesAmount) return;
            Elements.Add(element);
        }
    }

    public abstract class SerializableBlancaPathStructure<T> : IBlancaPathStructure<T>
    {
        [SerializeField] private T _base;
        [SerializeField] private T _lead;
        [SerializeField] private T _toPlayer;

        public T Base => _base;
        public T Lead => _lead;
        public T ToPlayer => _toPlayer;
    }

    public interface IBlancaRotationStructure<out T>
    {
        T Movement { get; }
        T Target { get; }
        T Copy { get; }
    }

    public class BlancaRotationStructure<T> : IBlancaRotationStructure<T>
    {
        [ShowInInspector]
        public T Movement
        {
            get => Elements[MovementIndex];
            protected set => Elements[MovementIndex] = value;
        }
        [ShowInInspector]
        public T Target
        {
            get => Elements[TargetIndex];
            protected set => Elements[TargetIndex] = value;
        }
        [ShowInInspector]
        public T Copy
        {
            get => Elements[CopyIndex];
            protected set => Elements[CopyIndex] = value;
        }

        public List<T> Elements { get; }

        public BlancaRotationStructure()
        {
            Elements = new List<T>(RotationTypesAmount);
        }

        public BlancaRotationStructure(IBlancaRotationStructure<T> wrapper) : this()
        {
            Elements.Add(wrapper.Movement);
            Elements.Add(wrapper.Target);
            Elements.Add(wrapper.Copy);

        }


        public const int MovementIndex = 0;
        public const int TargetIndex = MovementIndex + 1;
        public const int CopyIndex = TargetIndex + 1;
        public const int RotationTypesAmount = CopyIndex + 1;

        /// <summary>
        /// Used when it's not sure if the <see cref="Copy"/> element is added;
        /// </summary>
        public void SafeAddElement(T element)
        {
            if (Elements.Count < RotationTypesAmount) return;
            Elements.Add(element);
        }
    }

    public abstract class SerializableBlancaRotationStructure<T> : MonoBehaviour, IBlancaRotationStructure<T>
    {
        [SerializeField] private T _movement;
        [SerializeField] private T _target;
        [SerializeField] private T _copy;

        public T Movement => _movement;
        public T Target => _target;
        public T Copy => _copy;

    }

    public interface IBlancaLookAtStructure<out T>
    {
        T Target { get; }
        T Movement { get; }
        T Random { get; }
        T AtPlayer { get; }
    }

    public class BlancaLookAtStructure<T> : IBlancaLookAtStructure<T>
    {
        [ShowInInspector]
        public T Target
        {
            get => Elements[TargetIndex];
            protected set => Elements[TargetIndex] = value;
        }
        [ShowInInspector]
        public T Movement
        {
            get => Elements[MovementIndex];
            protected set => Elements[MovementIndex] = value;
        }
        [ShowInInspector]
        public T Random
        {
            get => Elements[RandomIndex];
            protected set => Elements[RandomIndex] = value;
        }
        [ShowInInspector]
        public T AtPlayer
        {
            get => Elements[AtPlayerIndex];
            protected set => Elements[AtPlayerIndex] = value;
        } 
        public List<T> Elements { get; }


        public const int TargetIndex = 0;
        public const int MovementIndex = TargetIndex + 1;
        public const int RandomIndex = MovementIndex + 1;
        public const int AtPlayerIndex = RandomIndex + 1;

        public const int LookAtTypesAmount = AtPlayerIndex + 1;

        public BlancaLookAtStructure()
        {
            Elements = new List<T>(LookAtTypesAmount);
        }
        public BlancaLookAtStructure(IBlancaLookAtStructure<T> wrapper) : this()
        {
            Elements.Add(wrapper.Target);
            Elements.Add(wrapper.Movement);
            Elements.Add(wrapper.Random);
            Elements.Add(wrapper.AtPlayer);
        }


    }
    
    public abstract class SerializableBlancaLookAtStructure<T> : MonoBehaviour, IBlancaLookAtStructure<T>
    {
        [SerializeField] private T _target;
        [SerializeField] private T _movement;
        [SerializeField] private T _random;
        [SerializeField] private T _atPlayer;

        public T Target => _target;
        public T Movement => _movement;
        public T Random => _random;
        public T AtPlayer => _atPlayer;
    }
}
