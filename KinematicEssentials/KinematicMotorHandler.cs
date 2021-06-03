using System.Collections.Generic;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace KinematicEssentials
{
    public class KinematicMotorHandler : ICharacterController, IKinematicMotorHandler
    {
        public readonly KinematicCharacterMotor Motor;
        public ICharacterGravity CharacterGravity;
        /// <summary>
        /// The order of the Queue is the order of priority of execution
        /// </summary>
        public readonly Queue<IKinematicVelocityFilter> VelocityFilters;
        public readonly Queue<IKinematicRotationFilter> RotationFilters;

        private const int PredictedAmountOfFilters = 4;
        public Vector3 CurrentVelocity => Motor.Velocity;

        [ShowInInspector] 
        public Vector3 DesiredVelocity { get; set; }
        private Vector3 _targetVelocity;

        [field: ShowInInspector]
        public Vector3 DesiredRotationForward { get; set; }
        private Quaternion _targetRotation;

        public Quaternion CurrentRotation { get; private set; }
        public Vector3 CurrentRotationForward { get; private set; }

        public KinematicMotorHandler(KinematicCharacterMotor motor, ICharacterGravity characterGravity)
        {
            Motor = motor;
            CharacterGravity = characterGravity;

            CurrentRotationForward = motor.transform.forward;
            DesiredVelocity = Vector3.zero;

            VelocityFilters = new Queue<IKinematicVelocityFilter>(PredictedAmountOfFilters);
            RotationFilters = new Queue<IKinematicRotationFilter>(PredictedAmountOfFilters);
        }


        public void DoVelocityLerp(Vector3 target, float changeSpeed)
        {
            DesiredVelocity = Vector3.Lerp(DesiredVelocity,target, Time.deltaTime * changeSpeed);
        }


        public void DoRotationSlerp(Vector3 target, float changeSpeed)
        {
            DesiredRotationForward = Vector3.Slerp(DesiredRotationForward,target,Time.deltaTime * changeSpeed);
        }


        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            _targetVelocity = DesiredVelocity;
            foreach (IKinematicVelocityFilter filter in VelocityFilters)
            {
                _targetVelocity = filter.FilterVelocity(currentVelocity, _targetVelocity);
            }

            if (Motor.GetState().GroundingStatus.IsStableOnGround)
            {
                currentVelocity = _targetVelocity;
            }
            else
            {
                _currentGravity += CharacterGravity.Gravity * deltaTime;
                currentVelocity = _currentGravity;
                if (CharacterGravity.CanMoveOnAir) currentVelocity += _targetVelocity;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {

            Transform motorTransform = Motor.transform;

            Vector3 transformUp = motorTransform.up;
            // to avoid rotate out of Character's Y
            Vector3 onPlaneForward = Vector3.ProjectOnPlane(DesiredRotationForward, transformUp);
            //Just to avoid Quaternion(zero)
            Vector3 smallForward = motorTransform.forward * 0.001f;

            CurrentRotationForward = onPlaneForward + smallForward;
            Quaternion targetRotation = Quaternion.LookRotation(CurrentRotationForward, transformUp);
            
            _targetRotation = targetRotation;
            foreach (IKinematicRotationFilter filter in RotationFilters)
            {
                _targetRotation = filter.FilterRotation(currentRotation, _targetRotation);
            }


            CurrentRotation = _targetRotation;
            currentRotation = CurrentRotation;
        }


        private Vector3 _currentGravity;
        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            _currentGravity = Vector3.zero;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }


        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        } 
    }

    public interface IKinematicMotorHandler
    {
        Vector3 DesiredVelocity { get; set; }
        Vector3 CurrentVelocity { get; }
        void DoVelocityLerp(Vector3 target, float changeSpeed);

        /// <summary>
        /// The proccess for this <see cref="Vector3"/> will be <seealso cref="Vector3.ProjectOnPlane"/>
        /// and <seealso cref="Quaternion.LookRotation"/> for the rotation be handled
        /// </summary>
        Vector3 CurrentRotationForward { get; }
        Vector3 DesiredRotationForward { get; set; }
        void DoRotationSlerp(Vector3 target, float changeSpeed);

    }

    public interface IKinematicMotorFilter : IKinematicVelocityFilter, IKinematicRotationFilter
    {
       
    }

    public interface IKinematicVelocityFilter
    {
        /// <returns><seealso cref="KinematicMotorHandler._targetVelocity"/></returns>
        Vector3 FilterVelocity(Vector3 currentVelocity, Vector3 desiredVelocity);
    }

    public interface IKinematicRotationFilter
    {

        /// <returns><seealso cref="KinematicMotorHandler._targetRotationForward"/></returns>
        Quaternion FilterRotation(Quaternion currentRotation, Quaternion desiredRotation);
    }

    public interface IKinematicDeltaVariation
    {
        BreakableAcceleration DeltaAcceleration { get; set; }
        float AngularSpeed { get; set; }
    }

    public interface ICharacterGravity
    {
        Vector3 Gravity { get; set; }
        bool CanMoveOnAir { get; set; }
    }
}
