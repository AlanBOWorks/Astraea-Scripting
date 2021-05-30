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

    public interface IRotationControl : IRotationWeight, IAccelerationControl
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

        private Vector3 _currentCalculation;
        public Vector3 CalculateForwardEuler()
        {
            _currentCalculation = Vector3.Lerp(
                _currentCalculation, 
                _velocity.CurrentVelocity * RotationWeight,
                Time.deltaTime * Acceleration);
            return _currentCalculation;
        }

        public float Acceleration { get; set; } = 4f;
    }

    public class TargetRotationControl : IRotationControl
    {
        [ShowInInspector, PropertyRange(-10, 10)]
        public Vector3 TargetRotationForward;

        [ShowInInspector, PropertyRange(-10, 10)]
        public float RotationWeight { get; set; }

        private Vector3 _currentCalculation;
        public Vector3 CalculateForwardEuler()
        {
            _currentCalculation = Vector3.Lerp(
                _currentCalculation,
                TargetRotationForward * RotationWeight,
                Time.deltaTime * Acceleration);
            return _currentCalculation;
        }

        public float Acceleration { get; set; } = 4f;
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
        private Vector3 _currentCalculation;
        public Vector3 CalculateForwardEuler()
        {
            _currentCalculation = Vector3.Lerp(
                _currentCalculation,
                _copyData.MeshForward * RotationWeight,
                Time.deltaTime * Acceleration);
            return _currentCalculation;
        }

        public float Acceleration { get; set; } = 2f;
    }
}
