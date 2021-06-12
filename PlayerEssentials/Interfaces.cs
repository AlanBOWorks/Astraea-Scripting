using ___ProjectExclusive;
using SharedLibrary;
using UnityEngine;

namespace PlayerEssentials
{

    public interface ICameraParameters
    {
        float CalculateAngleDelta(float currentAngle);
    }

    public interface IInputLookAround
    {
        Vector2 LookDeltaAxis { get; }
    }


    public interface IInputMovement
    {
        Vector3 GlobalDesiredDirection { get; }
        Vector3 GlobalDesiredVelocity { get; }
        bool IsMoving { get; }
        bool IsSprintPress { get; }
        Vector2 PlaneMovement { get; }
    }

    public interface IPlayerTransformData : ICharacterTransformData
    {
        Camera PlayerCamera { get; }

        Vector3 CameraForward { get; }
        Vector3 CameraRight { get; }
        Vector3 CameraUp { get; }

        /// <summary>
        /// Normalized planar forward (to know where is, in plane, the player is looking; useful for
        /// track the angle difference or to guess the next possible point where the player
        /// is going to move)
        /// </summary>
        Vector3 CameraPlanarForward { get; }

        /// <summary>
        /// Unlike <see cref="CameraPlanarForward"/> (which is normalized), this vector maintains its magnitude after
        /// losing its Y local-axis value 
        /// </summary>
        Vector3 CameraProjectedForward { get; }

        Transform GetLookAtPoint();
    }
}
