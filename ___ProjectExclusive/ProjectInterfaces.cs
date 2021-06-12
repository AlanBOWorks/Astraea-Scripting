using IKEssentials;
using KinematicEssentials;
using SharedLibrary;
using UnityEngine;


    //Non specified interfaces yet exclusive to the project
namespace ___ProjectExclusive
{
    public interface ICharacterDataHolder
    {
        IKinematicData GetKinematicData();
        ICharacterTransformData GetTransformData();
        FullHumanoidIKSolver GetHumanoidSolver();
    }
}
