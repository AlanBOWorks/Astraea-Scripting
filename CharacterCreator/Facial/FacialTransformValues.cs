using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    public interface IFacialTransformValuesHolder
    {
        FacialTransformValues GetTransformValues();
    }

    /// <summary>
    /// Lightweight version of <seealso cref="FacialCreationData"/>. <br></br>
    /// Unlike <seealso cref="FacialTransformValues"/> which contains Value <seealso cref="TransformValues"/>,
    /// this contains the Reference Type
    /// to use in the character creator which will control the same References values.
    /// </summary>
    [Serializable]
    public class FacialTransformValues : FacialStructure<FacialTransformValue>
    {

        public FacialTransformValues()
        {}

        public FacialTransformValues(FacialTransform transforms)
        {
            InjectTransforms(transforms);
        }

        public void InjectTransforms(FacialTransform facialTransforms)
        {
            InitializeWithParse(facialTransforms, InjectTransform);
            FacialTransformValue InjectTransform(Transform bone)
            {
                return new FacialTransformValue(bone);
            }
        }

        public void ReSerialize(FacialTransform facialTransform)
        {
            DoParse(facialTransform,Serialize);

            void Serialize(FacialTransformValue values, Transform bone)
            {
                values.ReSerialize(bone);
            }
        }

        public void RoundScale(int digits)
        {
            foreach (FacialTransformValue data in GenerateList())
            {
                data.RoundScale(digits);
            }
        }
        
    }
    /// <summary>
    /// Used to serialize the optimized bone's values for Reference type
    /// <br></br>
    /// </summary>
    [Serializable]
    public class FacialTransformValue 
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;

        public FacialTransformValue(){}
        public FacialTransformValue(Transform bone)
        {
            ReSerialize(bone);
        }

        public void RoundScale(int digits)
        {
            LocalScale.x = (float)Math.Round(LocalScale.x, digits);
            LocalScale.y = (float)Math.Round(LocalScale.y, digits);
            LocalScale.z = (float)Math.Round(LocalScale.z, digits);
        }

       
        public Vector3 GetLocalPosition()
        {
            return LocalPosition;
        }

        public Quaternion GetLocalRotation()
        {
            return LocalRotation;
        }

        public Vector3 GetLocalScale()
        {
            return LocalScale;
        }

        public void ReSerialize(Transform bone)
        {
            LocalPosition = bone.localPosition;
            LocalRotation = bone.localRotation;
            LocalScale = bone.localScale;
        }
        public void InjectInTransform(Transform bone)
        {
            bone.localPosition = LocalPosition;
            bone.localRotation = LocalRotation;
            bone.localScale = LocalScale;
        }

        public void LerpTransform(Transform bone, float lerpAmount)
        {
            bone.localPosition = Vector3.LerpUnclamped(bone.localPosition, LocalPosition, lerpAmount);
            bone.localRotation = Quaternion.LerpUnclamped(bone.localRotation, LocalRotation, lerpAmount);
            bone.localScale = Vector3.LerpUnclamped(bone.localScale, LocalScale, lerpAmount);
        }

        public void LerpTransform_NoScale(Transform bone, float lerpAmount)
        {
            bone.localPosition = Vector3.LerpUnclamped(bone.localPosition, LocalPosition, lerpAmount);
            bone.localRotation = Quaternion.LerpUnclamped(bone.localRotation, LocalRotation, lerpAmount);
        }
    }
}
