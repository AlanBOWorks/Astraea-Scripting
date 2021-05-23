using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KinematicEssentials
{
    public class KinematicData : IKinematicVelocity, IKinematicRotation
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
        public Vector3 CurrentRotationForward { get; private set; }

        [ShowInInspector,DisableInPlayMode,DisableInEditorMode]
        public Vector3 DesiredGlobalRotationForward { get; private set; }

        public Vector3 NormalizedGlobalVelocity { get; private set; }


        /// <summary>
        /// The target point in the world which the agent is moving towards
        /// </summary>
        public Vector3 PointOfDesiredVelocity { get; private set; }

        public void InjectData(KinematicMotorHandler handler, Transform referenceTransform)
        {
            KinematicMotorHandler motorHandler = handler;

            CurrentVelocity = motorHandler.DesiredVelocity;
            CurrentRotation = motorHandler.CurrentRotation;
            CurrentRotationForward = motorHandler.CurrentRotationForward;

            DesiredVelocity = motorHandler.DesiredVelocity;
            DesiredGlobalRotationForward = motorHandler.DesiredRotationForward;

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

    public interface IKinematicRotation
    {
        Quaternion CurrentRotation { get; }
        Vector3 CurrentRotationForward { get; }

        Vector3 DesiredGlobalRotationForward { get; }
    }
}
