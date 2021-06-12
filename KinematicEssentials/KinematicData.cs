using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KinematicEssentials
{
    public interface IKinematicData : IKinematicVelocity, IKinematicRotation { }

    public class ComposedKinematicData : IKinematicData
    {
        public readonly IKinematicVelocity Velocity;
        public readonly IKinematicRotation Rotation;

        public ComposedKinematicData(IKinematicVelocity velocity, IKinematicRotation rotation)
        {
            Velocity = velocity;
            Rotation = rotation;
        }


        public Vector3 CurrentVelocity => Velocity.CurrentVelocity;
        public Vector3 DesiredVelocity => Velocity.DesiredVelocity;
        public Vector2 CurrentLocalPlanarVelocity => Velocity.CurrentLocalPlanarVelocity;
        public float DesiredSpeed => Velocity.DesiredSpeed;
        public float CurrentSpeed => Velocity.CurrentSpeed;

        public Vector3 NormalizedCurrentRotationForward => Rotation.NormalizedCurrentRotationForward;
        public Vector3 NormalizedCurrentRotationRight => Rotation.NormalizedCurrentRotationRight;
        public Quaternion CurrentRotation => Rotation.CurrentRotation;
        public Vector3 NormalizedDesiredRotationForward => Rotation.NormalizedDesiredRotationForward;
    }

    public class KinematicData : IKinematicData, IKinematicRotation
    {
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public Vector3 CurrentVelocity { get; private set; }
        public Vector3 DesiredVelocity { get; private set; }

        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        private Vector3 _localVelocity;
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public Vector2 CurrentLocalPlanarVelocity { get; private set; }

        public float DesiredSpeed { get; private set; }

        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public float CurrentSpeed { get; private set; }
        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public Quaternion CurrentRotation { get; private set; }

        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        private Vector3 CurrentRotationForward { get; set; }

        public Vector3 NormalizedCurrentRotationForward { get; private set; }
        public Vector3 NormalizedCurrentRotationRight { get; private set; }

        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        private Vector3 DesiredRotationForward { get; set; }

        public Vector3 NormalizedDesiredRotationForward { get; private set; }

        public Vector3 NormalizedGlobalVelocity { get; private set; }


        /// <summary>
        /// The target point in the world which the agent is moving towards
        /// </summary>
        public Vector3 PointOfDesiredVelocity { get; private set; }

        public void InjectData(KinematicMotorHandler handler, Transform referenceTransform)
        {
            KinematicMotorHandler motorHandler = handler;

            CurrentVelocity = motorHandler.CurrentVelocity;
            CurrentRotation = motorHandler.CurrentRotation;
            CurrentRotationForward = motorHandler.CurrentRotationForward;
            NormalizedCurrentRotationForward = CurrentRotationForward.normalized;
            NormalizedCurrentRotationRight = referenceTransform.right;

            DesiredVelocity = motorHandler.DesiredVelocity;
            DesiredRotationForward = motorHandler.DesiredRotationForward;
            NormalizedDesiredRotationForward = DesiredRotationForward.normalized;

            _localVelocity = referenceTransform.InverseTransformDirection(CurrentVelocity);
            CurrentLocalPlanarVelocity = new Vector2(_localVelocity.x,_localVelocity.z);

            DesiredSpeed = DesiredVelocity.magnitude;
            CurrentSpeed = CurrentLocalPlanarVelocity.magnitude;


            NormalizedGlobalVelocity = CurrentVelocity.normalized;
            PointOfDesiredVelocity = referenceTransform.position + CurrentVelocity;
        }
    }

    public class KinematicDataHandler : ITicker
    {
        public readonly KinematicData Data;
        public readonly KinematicMotorHandler Handler;
        public readonly Transform DirectionReference;

        public KinematicDataHandler(KinematicData data, KinematicMotorHandler handler, Transform directionReference)
        {
            Data = data;
            Handler = handler;
            DirectionReference = directionReference;
        }

        public bool Disabled { get; set; }
        public void Tick()
        {
            Data.InjectData(Handler,DirectionReference);
        }
    }

    public interface IKinematicVelocity
    {
        Vector3 CurrentVelocity { get; }
        Vector3 DesiredVelocity { get; }
        /// <summary>
        /// Velocity in (x,y) emulating Planar Movement;<br></br>
        /// Used mainly for animation purposes
        /// </summary>
        Vector2 CurrentLocalPlanarVelocity { get; }

        float DesiredSpeed { get; }
        float CurrentSpeed { get; }
    }

    public interface IKinematicRotation : IKinematicNormalizedRotation
    {
        Quaternion CurrentRotation { get; }
        Vector3 NormalizedDesiredRotationForward { get; }
    }

    public interface IKinematicNormalizedRotation
    {
        Vector3 NormalizedCurrentRotationForward { get; }
        Vector3 NormalizedCurrentRotationRight { get; }

    }
}
