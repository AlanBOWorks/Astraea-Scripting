using System.Collections.Generic;

using MEC;
using UnityEngine;

namespace AIEssentials
{
    public interface IPathCalculator : IPathDestination, IPathDistance
    {
        IEnumerator<float> RefreshSearch(Vector3 targetPoint);

        Vector3 DesiredVelocity();
        bool HasPath();
    }


    public interface IPathDestination
    {
        void SetDestination(Vector3 targetPoint);
        Vector3 GetDestination();
        Vector3 SteeringPoint();
    }

    public interface IPathDistance
    {
        float GetRemainingDistance();
        bool IsCloseEnough(float distanceThreshold);
        bool HasReachedDestination();
        void SetReachDestinationDistance(float distance);


    }
}
