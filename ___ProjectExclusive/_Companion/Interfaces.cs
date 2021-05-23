using UnityEngine;

namespace Companion
{
    public interface ICompanionStructure<T>
    {
        T Blanca { get; set; }
        T Player { get; set; }
    }
}
