using KinematicEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    public interface IRotationWeight
    {
        float RotationWeight { get; set; }
    }

    public interface IRotationControl : IRotationWeight
    {
        Vector3 CalculateForwardEuler();
    }

    public class MovementRotationControl : IRotationControl
    {
        private readonly IKinematicVelocity _velocity;

        public MovementRotationControl(IKinematicVelocity velocity)
        {
            _velocity = velocity;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float RotationWeight { get; set; } = 0;
        public Vector3 CalculateForwardEuler()
        {
            return _velocity.CurrentVelocity * RotationWeight;
        }
    }

    public class TargetRotationControl : IRotationControl
    {
        [ShowInInspector, PropertyRange(-10, 10)]
        public Vector3 TargetRotationForward;

        [ShowInInspector, PropertyRange(-10, 10)]
        public float RotationWeight { get; set; }
        public Vector3 CalculateForwardEuler()
        {
            return TargetRotationForward * RotationWeight;
        }
    }

    public class CopyCharacterRotationControl : IRotationControl
    {
        private readonly ICharacterTransformData _copyData;
        public CopyCharacterRotationControl(ICharacterTransformData copyData)
        {
            _copyData = copyData;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float RotationWeight { get; set; }
        public Vector3 CalculateForwardEuler()
        {
            return _copyData.MeshForward * RotationWeight;
        }
    }
}
