using System.Collections.Generic;
using AIEssentials;
using KinematicEssentials;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace IKEssentials
{
    

    public interface ILookAtCalculator
    {
        Vector3 DirectionLookAt(Vector3 headReferencePoint);
    }


    public class LookAtTarget : ILookAtCalculator
    {
        public Vector3 TargetPoint { get; private set; }
        public AnimationCurve WeightByDistance { set; private get; }

        [ShowInInspector, PropertyRange(-10, 10)]

        public LookAtTarget()
        {}

        public LookAtTarget(AnimationCurve sqrDistanceCloseModifier)
        {
            WeightByDistance = sqrDistanceCloseModifier;
        }

        public Vector3 DirectionLookAt(Vector3 headReferencePoint)
        {
            Vector3 targetDirection = TargetPoint - headReferencePoint;
            if (WeightByDistance == null) return targetDirection;

            targetDirection *= WeightByDistance.Evaluate(targetDirection.magnitude);
            return targetDirection;
        }

        private CoroutineHandle _trackHandle;

        public void TrackPoint(Vector3 fixedPoint)
        {
            Timing.KillCoroutines(_trackHandle);
            TargetPoint = fixedPoint;
        }

        public void TrackTransform(Transform target, Vector3 offset)
        {
            Timing.KillCoroutines(_trackHandle);
            _trackHandle = Timing.RunCoroutine(TrackTransform());
            IEnumerator<float> TrackTransform()
            {
                while (target != null)
                {
                    yield return Timing.WaitForOneFrame;
                    TargetPoint = target.position + offset;
                }
            }
        }
    }

    public class LookAtMovement : ILookAtCalculator
    {
        public IKinematicData Velocity { set; private get; }
        public float MagnitudeModifier;
        public LookAtMovement(IKinematicData velocity, float magnitudeModifier)
        {
            Velocity = velocity;
            MagnitudeModifier = magnitudeModifier;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public Vector3 DirectionLookAt(Vector3 headReferencePoint)
        {
            return Velocity.DesiredVelocity * MagnitudeModifier;
        }
    }

    public class LookAtRandom : ILookAtCalculator
    {

        public Transform InverseReference { set; private get; }
        public SRange UpdateFrequency { set; private get; }
        public float RandomStrength;
        public float ForwardOffset;

        public Transform RandomTargetToLook = null;

        public LookAtRandom(Transform headInverseReference,SRange updateFrequency, 
            float randomStrength = .3f, float forwardOffset = 2)
        {
            InverseReference = headInverseReference;
            UpdateFrequency = updateFrequency;
            RandomStrength = randomStrength;
            ForwardOffset = forwardOffset;

            Timing.RunCoroutine(DoRandomVariation());
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        private Vector3 _currentDirection;
        public Vector3 DirectionLookAt(Vector3 headReferencePoint)
        {
            if(RandomTargetToLook is null)
                return _currentDirection;
            else
            {
                Vector3 directionTowardsTarget = RandomTargetToLook.position - InverseReference.position;
                return _currentDirection + directionTowardsTarget;
            }
        }

        private IEnumerator<float> DoRandomVariation()
        {
            while (InverseReference != null)
            {
                yield return Timing.WaitForSeconds(UpdateFrequency.RandomInRange());
                Vector2 randomDirection = Random.insideUnitCircle * RandomStrength;

                _currentDirection = InverseReference.TransformDirection(
                    randomDirection.x,
                    randomDirection.y,
                    ForwardOffset);
            }
        }
    }

    public class LookAtHeadData : ILookAtCalculator
    {
        public ICharacterTransformData LookAtHead { set; private get; }

        public LookAtHeadData(ICharacterTransformData lookAtHead)
        {
            LookAtHead = lookAtHead;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public Vector3 DirectionLookAt(Vector3 headReferencePoint)
        {
            return LookAtHead.HeadPosition - headReferencePoint;
        }
    }

}
