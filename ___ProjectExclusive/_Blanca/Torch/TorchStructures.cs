using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public interface ITorchStructure<T>
    {
        T Original { get; set; }
        T Procedural { get; set; }
        T Animation { get; set; }
        T UnEquip { get; set; }
    }

    public interface ITorchTargets<T>
    {
        T Holder { get; set; }

        T LeftHandRoot { get; set; }
        T LeftBendTarget { get; set; }
        T RightHandRoot { get; set; }
        T RightBendTarget { get; set; }

        T[] Elements { get; }
    }

    public static class TorchTargetsBase
    {
        public static T[] GenerateElements<T>(ITorchTargets<T> targets)
        {
            var generated = new[]
            {
                targets.Holder,
                targets.LeftHandRoot,
                targets.LeftBendTarget,
                targets.RightHandRoot,
                targets.RightBendTarget
            };
            return generated;
        }


        public const int HolderIndex = 0;
        public const int LeftHandIndex = HolderIndex + 1;
        public const int LeftBendIndex = LeftHandIndex + 1;
        public const int RightHandIndex = LeftBendIndex + 1;
        public const int RightBendIndex = RightHandIndex + 1;
        public const int ElementsAmount = RightBendIndex + 1;
    }

    public class TorchTargetsBase<T> : ITorchTargets<T>
    {
        public T Holder
        {
            get => Elements[TorchTargetsBase.HolderIndex];
            set => Elements[TorchTargetsBase.HolderIndex] = value;
        }
        public T LeftHandRoot
        {
            get => Elements[TorchTargetsBase.LeftHandIndex];
            set => Elements[TorchTargetsBase.LeftHandIndex] = value;
        }
        public T LeftBendTarget
        {
            get => Elements[TorchTargetsBase.LeftBendIndex];
            set => Elements[TorchTargetsBase.LeftBendIndex] = value;
        }
        public T RightHandRoot
        {
            get => Elements[TorchTargetsBase.RightHandIndex];
            set => Elements[TorchTargetsBase.RightHandIndex] = value;
        }
        public T RightBendTarget
        {
            get => Elements[TorchTargetsBase.RightBendIndex];
            set => Elements[TorchTargetsBase.RightBendIndex] = value;
        }
        [ShowInInspector]
        public T[] Elements { get; }


        public TorchTargetsBase()
        {
            Elements = new T[TorchTargetsBase.ElementsAmount];
        }

    }


}
