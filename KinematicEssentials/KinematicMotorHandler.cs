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
        public IKinematicVelocityFilter VelocityFilter;
        public IKinematicRotationFilter RotationFilter;

        public void ChangeAllFilters(IKinematicMotorFilter motorFilter)
        {
            VelocityFilter = motorFilter;
            RotationFilter = motorFilter;
        }

        public Vector3 CurrentVelocity => Motor.Velocity;

        [ShowInInspector] 
        public Vector3 DesiredVelocity { get; set; }
        private Vector3 _targetVelocity;

        [field: ShowInInspector]
        public Vector3 DesiredRotationForward { get; set; }
        private Vector3 _targetRotationForward;

        public Quaternion CurrentRotation { get; private set; }
        public Vector3 CurrentRotationForward { get; private set; }

        public KinematicMotorHandler(KinematicCharacterMotor motor, ICharacterGravity characterGravity)
        {
            Motor = motor;
            CharacterGravity = characterGravity;

            CurrentRotationForward = motor.transform.forward;
            DesiredVelocity = Vector3.zero;
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
            _targetVelocity = VelocityFilter?.FilterVelocity(DesiredVelocity) ?? DesiredVelocity;

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
            _targetRotationForward = RotationFilter?.FilterRotation(DesiredRotationForward) ?? DesiredRotationForward;

            Transform motorTransform = Motor.transform;

            Vector3 transformUp = motorTransform.up;
            // to avoid rotate out of Character's Y
            Vector3 onPlaneForward = Vector3.ProjectOnPlane(_targetRotationForward,transformUp);
            //Just to avoid Quaternion(zero)
            Vector3 smallForward = motorTransform.forward * 0.001f;

            CurrentRotationForward = onPlaneForward + smallForward;

            CurrentRotation = Quaternion.LookRotation(CurrentRotationForward,transformUp);
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
        /// This <see cref="Vector3"/> will be <seealso cref="Vector3.ProjectOnPlane"/>
        /// and <seealso cref="Quaternion.LookRotation"/>
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
        Vector3 FilterVelocity(Vector3 desiredVelocity);
    }

    public interface IKinematicRotationFilter
    {

        /// <returns><seealso cref="KinematicMotorHandler._targetRotationForward"/></returns>
        Vector3 FilterRotation(Vector3 desiredRotation);
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
