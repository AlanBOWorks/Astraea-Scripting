using System;
using UnityEngine;

namespace SharedLibrary
{
    /// <summary>
    /// Common structure with <seealso cref="Vector3"/> and <seealso cref="Quaternion"/><br></br>
    /// Easy constructor by <seealso cref="Transform"/>
    /// </summary>
    [Serializable]
    public struct LocalPositionRotation
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public LocalPositionRotation(Transform bone)
        {
            Position = bone.localPosition;
            Rotation = bone.localRotation;
        }

        public LocalPositionRotation(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        /// <summary>
        /// Cast the (<seealso cref="Vector3"/> <paramref name="eulerRotation"/>) by <seealso cref="Quaternion.Euler"/>.
        /// <br></br>
        /// _For better constructor use: <see cref="LocalPositionRotation(Vector3, Quaternion)"/>
        /// </summary>
        public LocalPositionRotation(Vector3 position, Vector3 eulerRotation)
        {
            Position = position;
            Rotation = Quaternion.Euler(eulerRotation);
        }
    }
    /// <summary>
    /// <inheritdoc cref="LocalPositionRotation"/>
    /// <br></br>
    /// Also includes LocalScale
    /// </summary>
    [Serializable]
    public struct TransformLocalValues
    {
        public readonly Vector3 LocalPosition;
        public readonly Quaternion LocalRotation;
        public readonly Vector3 LocalScale;

        public TransformLocalValues(Transform bone) : this(bone.localPosition, bone.localRotation, bone.localScale)
        { }

        public TransformLocalValues(Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = localScale;
        }

        public TransformLocalValues(Vector3 localPosition, Quaternion localRotation) : this(localPosition, localRotation, Vector3.one)
        { }

        public void LerpTransform(Transform bone, float lerpAmount)
        {
            bone.localPosition = Vector3.LerpUnclamped(bone.localPosition, LocalPosition, lerpAmount);
            bone.localRotation = Quaternion.LerpUnclamped(bone.localRotation, LocalRotation, lerpAmount);
            bone.localScale = Vector3.LerpUnclamped(bone.localScale, LocalScale, lerpAmount);
        }

        public void LoadTransform(Transform bone)
        {
            bone.localPosition = LocalPosition;
            bone.localRotation = LocalRotation;
            bone.localScale = LocalScale;
        }
    }
}
