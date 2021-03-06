using System.Collections.Generic;
using AIEssentials;
using Blanca;
using KinematicEssentials;
using MEC;
using Player;
using PlayerEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{

    public interface IVelocityWeight
    {
        float VelocityWeight { get; set; }

    }

    public interface IVelocityControl : IVelocityWeight, IAccelerationControl
    {
        Vector3 CalculateTargetVelocity();
    }
    public class CopyVelocityControl : IVelocityControl
    {
        [ShowInInspector, DisableInPlayMode]
        public IInputMovement CopyVelocity { set; private get; }
        private Vector3 _currentCopyVelocity;

        [ShowInInspector, PropertyRange(-10, 10)]
        public float VelocityWeight { get; set; } = 0;

        public Vector3 CalculateTargetVelocity()
        {
            _currentCopyVelocity = Vector3.Lerp(
                _currentCopyVelocity, 
                CopyVelocity.GlobalDesiredVelocity * VelocityWeight,
                Time.deltaTime * Acceleration);
            return _currentCopyVelocity;
        } 

        public CopyVelocityControl(IInputMovement copyVelocity, float copyAcceleration = 2)
        {
            CopyVelocity = copyVelocity;
            Acceleration = copyAcceleration;
        }

        public float Acceleration { get; set; }
    }

    public class PathVelocityControl : IVelocityControl, IPathDestination
    {
        protected readonly VelocityPathCalculator PathCalculator;

        public PathVelocityControl(VelocityPathCalculator pathCalculator)
        {
            PathCalculator = pathCalculator;
            VelocityWeight = 0;
        }

        [ShowInInspector]
        public float Acceleration
        {
            get => PathCalculator.acceleration;
            set => PathCalculator.acceleration = value;
        }
        [ShowInInspector, PropertyRange(-10, 10)]
        public float VelocityWeight
        {
            get => PathCalculator.maxSpeed;
            set => PathCalculator.maxSpeed = value;
        }
        public Vector3 CalculateTargetVelocity()
        {
            return PathCalculator.DesiredVelocity();
        }

        public void SetDestination(Vector3 targetPoint)
        {
            PathCalculator.SetDestination(targetPoint);
        }

        public Vector3 GetDestination()
        {
            return PathCalculator.GetDestination();
        }

        public Vector3 SteeringPoint()
        {
            return PathCalculator.steeringTarget;
        }
#if UNITY_EDITOR
        [Button]
        private void DebugDestination(Transform onTarget, float targetWeight = 1)
        {
            VelocityWeight = targetWeight;
            SetDestination(onTarget.position);
        }
#endif
    }
}
