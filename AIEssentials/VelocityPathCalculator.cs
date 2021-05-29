using System.Collections.Generic;
using MEC;
using Pathfinding;
using UnityEngine;

namespace AIEssentials
{
    public class VelocityPathCalculator : RichAI, IPathCalculator
    {
        protected override void Start()
        {
            base.Start();
            destination = transform.position;
        }

        protected override void Update()
        {
            base.Update();
            _remainingDistance = remainingDistance;
        }

        public override void FinalizeMovement(Vector3 nextPosition, Quaternion nextRotation)
        {
            // Removed the base.FinalizeMovement in order to prevent to moving the transform
            // but allowing calculations of desiredVelocity in CalculatePathVelocity
        }

        public void SetDestination(Vector3 targetPoint)
        {
            destination = targetPoint;
        }

        public Vector3 GetDestination()
        {
            return destination;
        }

        public Vector3 SteeringPoint()
        {
            return steeringTarget;
        }


        public bool HasPath()
        {
            return hasPath;
        }

        public bool IsPathPending()
        {
            return pathPending;
        }

        public Vector3 DesiredVelocity()
        {
            return desiredVelocity;
        }

        public float GetRemainingDistance()
        {
            return _remainingDistance;
        }

        /// <summary>
        /// <seealso cref="IAstarAI.remainingDistance"/> is calculated on call
        /// and uses <seealso cref="Vector3.Distance"/>; this is used to avoid
        /// additional calls since the distance is multiple times asked in
        /// various places
        /// </summary>
        private float _remainingDistance;
        public bool IsCloseEnough(float distanceThreshold)
        {
            return _remainingDistance < distanceThreshold;
        }

        public void SetReachDestinationDistance(float distance)
        {
            endReachedDistance = distance;
        }
        public bool HasReachedDestination()
        {
            return reachedEndOfPath;
        }

        public void DoClearPath()
        {
            base.ClearPath();
        }

        public IEnumerator<float> RefreshSearch(Vector3 targetPoint)
        {
            SetDestination(targetPoint);
            base.SearchPath();

            yield return Timing.WaitUntilTrue(seeker.IsDone);
        }
    }
}
