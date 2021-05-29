﻿using System.Collections.Generic;
using KinematicEssentials;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace IKEssentials
{
    public interface ILookAtWeight
    {
        float LookAtWeight { get; set; }
    }

    public interface ILookAtControl : ILookAtWeight
    {
        Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint);
    }


    public class LookAtTarget : ILookAtControl
    {
        public Vector3 TargetPoint { get; private set; }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float LookAtWeight { get; set; }
        public Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint)
        {
            Vector3 direction = TargetPoint - headReferencePoint;
            direction.Normalize();
            return direction * LookAtWeight;
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

    public class LookAtMovement : ILookAtControl
    {
        public IKinematicVelocity Velocity { set; private get; }

        public LookAtMovement(IKinematicVelocity velocity)
        {
            Velocity = velocity;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float LookAtWeight { get; set; }
        public Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint)
        {
            Vector3 direction = Velocity.DesiredVelocity;
            direction.Normalize();
            return direction * LookAtWeight;
        }
    }

    public class LookAtRandom : ILookAtControl
    {

        public Transform InverseReference { set; private get; }
        public SRange UpdateFrequency { set; private get; }

        public LookAtRandom(Transform headInverseReference,SRange updateFrequency)
        {
            InverseReference = headInverseReference;
            UpdateFrequency = updateFrequency;
            Timing.RunCoroutine(DoRandomVariation());
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float LookAtWeight { get; set; }
        private Vector3 _currentDirection;
        public Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint)
        {
            _currentDirection.Normalize();
            return _currentDirection * LookAtWeight;
        }

        private IEnumerator<float> DoRandomVariation()
        {
            while (InverseReference != null)
            {
                yield return Timing.WaitForSeconds(UpdateFrequency.RandomInRange());
                _currentDirection = InverseReference.TransformDirection(Random.insideUnitCircle);
            }
        }
    }

    public class LookAtHeadData : ILookAtControl
    {
        public ICharacterTransformData LookAtHead { set; private get; }

        public LookAtHeadData(ICharacterTransformData lookAtHead)
        {
            LookAtHead = lookAtHead;
        }

        [ShowInInspector, PropertyRange(-10, 10)]
        public float LookAtWeight { get; set; }
        public Vector3 CalculateDirectionToLookAt(Vector3 headReferencePoint)
        {
            Vector3 direction = LookAtHead.HeadPosition - headReferencePoint;
            direction = Vector3.ClampMagnitude(direction,1);
            return direction * LookAtWeight;
        }
    }

}
