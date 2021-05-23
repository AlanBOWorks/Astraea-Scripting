using Animancer;
using UnityEngine;

namespace AnimancerEssentials
{
    public interface IAnimancerBaseStructure<out T>
    {
        T Base { get; }
        T Override { get; }
        T[] SerializedData { get; }
    }


    public interface IAnimancerHumanoidStructure<out T> : IAnimancerBaseStructure<T>
    {
        T UpperHalf { get; }
        T PriorityUpperHalf { get; }

    }

    public interface IAnimancerFacialStructure<out T> : IAnimancerBaseStructure<T>
    {
        T Addition { get; }
    }

   
}
