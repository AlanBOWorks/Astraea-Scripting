using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Blanca
{
    public interface IBlancaPathStructure <out T>
    {
        T Base { get; }
        T Lead { get; }
        T ToPlayer { get; }
    }
    public interface IBlancaMovementStructure<out T> : IBlancaPathStructure<T>
    {
        T Copy { get; }
    }

    public static class BlancaPathStructure
    {
        public enum Types
        {
            Base = BaseIndex,
            Lead = LeadIndex,
            ToPlayer = ToPlayerIndex,
        }

        public const int BaseIndex = 0;
        public const int LeadIndex = BaseIndex + 1;
        public const int ToPlayerIndex = LeadIndex + 1;
    }

    public static class BlancaMovementStructure
    {
        

        public const int BaseIndex = BlancaPathStructure.BaseIndex;
        public const int LeadIndex = BlancaPathStructure.LeadIndex;
        public const int ToPlayerIndex = BlancaPathStructure.ToPlayerIndex;
        public const int CopyIndex = ToPlayerIndex + 1;

        public const int MovementTypesAmount = CopyIndex + 1;
    }

    public struct BlancaVelocityWeight
    {
        public float Base;
        public float Lead;
        public float ToPlayer;
        public float Copy;

        public BlancaVelocityWeight(float bBase, float lead, float toPlayer, float copy)
        {
            Base = bBase;
            Lead = lead;
            ToPlayer = toPlayer;
            Copy = copy;
        }
    }

    public class BlancaMovementStructure<T> : IBlancaMovementStructure<T>
    {
        [ShowInInspector]
        public T Base
        {
            get => Elements[BlancaMovementStructure.BaseIndex];
            protected set => Elements[BlancaMovementStructure.BaseIndex] = value;
        }
        [ShowInInspector]
        public T Lead
        {
            get => Elements[BlancaMovementStructure.LeadIndex];
            protected set => Elements[BlancaMovementStructure.LeadIndex] = value;
        }
        [ShowInInspector]
        public T ToPlayer
        {
            get => Elements[BlancaMovementStructure.ToPlayerIndex];
            protected set => Elements[BlancaMovementStructure.ToPlayerIndex] = value;
        }


        [ShowInInspector]
        public T Copy
        {
            get => Elements[BlancaMovementStructure.CopyIndex];
            protected set => Elements[BlancaMovementStructure.CopyIndex] = value;
        }

        public List<T> Elements { get; protected set; }

        public BlancaMovementStructure()
        {
            Elements = new List<T>(BlancaMovementStructure.MovementTypesAmount);
        }

        public BlancaMovementStructure(IBlancaMovementStructure<T> wrapper)
        : this()
        {
            Elements.Add(wrapper.Base);
            Elements.Add(wrapper.Lead);
            Elements.Add(wrapper.ToPlayer);
            Elements.Add(wrapper.Copy);
        }

        

        public void AddBasicSetup(T basePath, T leadPath, T toPlayerPath)
        {
            if (Elements.Count >= BlancaMovementStructure.MovementTypesAmount -1)
            {
                Base = basePath;
                Lead = leadPath;
                ToPlayer = toPlayerPath;
                return;
            }

            Elements.Add(basePath);
            Elements.Add(leadPath);
            Elements.Add(toPlayerPath);
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

        public T GetElement(BlancaPathStructure.Types targetTypes)
        {
            switch (targetTypes)
            {
                default:
                case BlancaPathStructure.Types.Base:
                    return _base;
                case BlancaPathStructure.Types.Lead:
                    return _lead;
                case BlancaPathStructure.Types.ToPlayer:
                    return _toPlayer;
            }
        }
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

        public void AddBasicSetup(T movement, T target)
        {
            if (Elements.Count >= RotationTypesAmount - 1)
            {
                Movement = movement;
                Target = target;
                return;
            }
            Elements.Add(movement);
            Elements.Add(target);
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
        T SecondaryTarget { get; }
    }

    public interface IBlancaLookAtModifiable<in T>
    {
        T Target { set; }
        T Movement { set; }
        T Random { set; }
        T AtPlayer { set; }
        T SecondaryTarget { set; }

    }

    public class BlancaLookWeights : BlancaLookAtStructure<float>
    {
        public BlancaLookWeights() : base()
        {
            for (int i = 0; i < Elements.Capacity; i++)
            {
                Elements.Add(0);
            }
        }
    }

    public class BlancaLookNormalizedDirections : BlancaLookAtStructure<Vector3>
    {
        public BlancaLookNormalizedDirections() : base()
        {
            for (int i = 0; i < Elements.Capacity; i++)
            {
                Elements.Add(Vector3.zero);
            }
        }

        public static List<Vector3> GenerateList()
        {
            List<Vector3> list = new List<Vector3>(LookAtTypesAmount);
            for (int i = 0; i < list.Capacity; i++)
            {
                list.Add(Vector3.zero);
            }

            return list;
        }
    }


    public class BlancaLookAtStructure<T> : IBlancaLookAtStructure<T>,IBlancaLookAtModifiable<T>
    {
        [ShowInInspector]
        public virtual T Target
        {
            get => Elements[TargetIndex];
            set => Elements[TargetIndex] = value;
        }

        [ShowInInspector]
        public T Movement
        {
            get => Elements[MovementIndex];
            set => Elements[MovementIndex] = value;
        }

        [ShowInInspector]
        public T Random
        {
            get => Elements[RandomIndex];
            set => Elements[RandomIndex] = value;
        }

        [ShowInInspector]
        public T AtPlayer
        {
            get => Elements[AtPlayerIndex];
            set => Elements[AtPlayerIndex] = value;
        }
        [ShowInInspector]
        public T SecondaryTarget
        {
            get => Elements[SecondaryTargetIndex];
            set => Elements[SecondaryTargetIndex] = value;
        }

        public List<T> Elements { get; }


        public const int TargetIndex = 0;
        public const int MovementIndex = TargetIndex + 1;
        public const int RandomIndex = MovementIndex + 1;
        public const int AtPlayerIndex = RandomIndex + 1;
        public const int SecondaryTargetIndex = AtPlayerIndex + 1;

        public const int LookAtTypesAmount = SecondaryTargetIndex + 1;

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

        public BlancaLookAtStructure(T target, T movement, T random) : this()
        {
            Elements.Add(target);
            Elements.Add(movement);
            Elements.Add(random);
        }

        public void AddBasicSetup(T target, T movement, T random)
        {
            if(Elements.Count >= LookAtTypesAmount -1)
            {
                Target = target;
                Movement = movement;
                Random = random;
                return;
            }
            Elements.Add(target);
            Elements.Add(movement);
            Elements.Add(random);
        }
    }

    public abstract class SerializableBlancaLookAtStructure<T> : MonoBehaviour, IBlancaLookAtStructure<T>
    {
        [SerializeField] private T _target;
        [SerializeField] private T _movement;
        [SerializeField] private T _random;
        [SerializeField] private T _atPlayer;
        [SerializeField] private T _secondaryTarget;

        public T Target => _target;
        public T Movement => _movement;
        public T Random => _random;
        public T AtPlayer => _atPlayer;
        public T SecondaryTarget => _secondaryTarget;
    }
}
