using System;
using SharedLibrary;
using UnityEngine;

namespace CharacterCreator
{
    public interface IFullFacialExpression : IFacialTransformValuesHolder, IVroidExpressionHolder
    {
    }

    [Serializable]
    public class EyeLidExpression 
    {
        public bool Additive;

        public LocalPositionRotation LeftRoot;
        public LocalPositionRotation LeftTop;
        public LocalPositionRotation LeftBottom;
        public LocalPositionRotation RightRoot;
        public LocalPositionRotation RightTop;
        public LocalPositionRotation RightBottom;

        public bool IsAdditive()
        {
            return Additive;
        }
        public void DoExpression(FacialTransformValues values, 
            FacialTransform transforms,
            float weight)
        {
            if (Additive)
            {
                DoAdditiveExpression(values,transforms,weight);
            }
            else
            {
                DoOverrideExpression(values, transforms, weight);
            }
        }

        protected virtual void DoAdditiveExpression(
            FacialTransformValues values,
            FacialTransform transforms,
            float weight)
        {
            DoTransformAdditive(weight,
                LeftRoot,
                values.LeftEyeRoot,
                transforms.LeftEyeRoot);
            DoTransformAdditive(weight,
                LeftTop,
                values.LeftEyeTopRoot(),
                transforms.LeftEyeTopRoot());
            DoTransformAdditive(weight,
                LeftBottom,
                values.LeftEyeBottomRoot(),
                transforms.LeftEyeBottomRoot());

            DoTransformAdditive(weight,
                RightRoot,
                values.RightEyeRoot,
                transforms.RightEyeRoot);
            DoTransformAdditive(weight,
                RightTop,
                values.RightEyeTopRoot(),
                transforms.RightEyeTopRoot());
            DoTransformAdditive(weight,
                RightBottom,
                values.RightEyeBottomRoot(),
                transforms.RightEyeBottomRoot());
        }

        protected virtual void DoOverrideExpression(
            FacialTransformValues values,
            FacialTransform transforms,
            float weight)
        {
            DoTransformOverride(weight,
                LeftRoot,
                values.LeftEyeRoot,
                transforms.LeftEyeRoot);
            DoTransformOverride(weight,
                LeftTop,
                values.LeftEyeTopRoot(),
                transforms.LeftEyeTopRoot());
            DoTransformOverride(weight,
                LeftBottom,
                values.LeftEyeBottomRoot(),
                transforms.LeftEyeBottomRoot());

            DoTransformOverride(weight,
                RightRoot,
                values.RightEyeRoot,
                transforms.RightEyeRoot);
            DoTransformOverride(weight,
                RightTop,
                values.RightEyeTopRoot(),
                transforms.RightEyeTopRoot());
            DoTransformOverride(weight,
                RightBottom,
                values.RightEyeBottomRoot(),
                transforms.RightEyeBottomRoot());
        }

        protected void DoTransformAdditive(float weight, LocalPositionRotation addition,
            FacialTransformValue data, Transform bone)
        {
            RotateTransform(weight, data.LocalRotation * addition.Rotation, data, bone);
            TranslateTransform(weight, data.LocalPosition + addition.Position, data, bone);
        }
        protected void DoTransformOverride(float weight, LocalPositionRotation addition,
            FacialTransformValue data, Transform bone)
        {
            RotateTransform(weight, addition.Rotation, data, bone);
            TranslateTransform(weight, addition.Position, data, bone);
        }

        private void RotateTransform(float weight, Quaternion target, FacialTransformValue data, Transform bone)
        {
            bone.localRotation = Quaternion.LerpUnclamped(data.GetLocalRotation(), target, weight);
        }
        private void TranslateTransform(float weight, Vector3 target, FacialTransformValue data, Transform bone)
        {
            bone.localPosition = Vector3.LerpUnclamped(data.GetLocalPosition(),target,weight);
        }

        public virtual void Serialize(FacialTransform structure)
        {
            LeftRoot = new LocalPositionRotation(structure.LeftEyeRoot);
            LeftTop = new LocalPositionRotation(structure.LeftEyeTopRoot());
            LeftBottom = new LocalPositionRotation(structure.LeftEyeBottomRoot());

            RightRoot = new LocalPositionRotation(structure.RightEyeRoot);
            RightTop = new LocalPositionRotation(structure.RightEyeTopRoot());
            RightBottom = new LocalPositionRotation(structure.RightEyeBottomRoot());
        }

        public EyeLidExpression(bool isAdditive)
        {
            Additive = isAdditive;
        }
    }

    [Serializable]
    public class FullEyeLidExpression : EyeLidExpression
    {
        public LocalPositionRotation[] ExtraLeftTop;
        public LocalPositionRotation[] ExtraLeftBottom;
        public LocalPositionRotation[] ExtraRightTop;
        public LocalPositionRotation[] ExtraRightBottom;

        protected override void DoAdditiveExpression(FacialTransformValues values,
            FacialTransform transforms,
            float weight)
        {
            base.DoAdditiveExpression(values,transforms, weight);

            for (int i = 0; i < ExtraLeftTop.Length; i++)
            {
                //This could throw IndexOut???? >
                // > The game's design was made in such way that all have the same
                //amount of eye bones;
                //This could be a reminder if there's a problem related to that 
                //it should be changed with a Min(A.length,B.length) before the loop
                DoTransformAdditive(weight,
                    ExtraLeftTop[i], 
                    values.LeftEyeTop[i+1],
                    transforms.LeftEyeTop[i+1]
                    ); //+1 because index[0] is root and it was calculated base
                DoTransformAdditive(weight,
                    ExtraRightTop[i], 
                    values.RightEyeTop[i + 1],
                    transforms.RightEyeTop[i + 1]

                    );
            }
            //Same that the loop up there
            for (int i = 0; i < ExtraLeftBottom.Length; i++)
            {
                DoTransformAdditive(weight,
                    ExtraLeftBottom[i], 
                    values.LeftEyeBottom[i + 1],
                    transforms.LeftEyeBottom[i+1]);
                DoTransformAdditive(weight,
                    ExtraRightBottom[i],
                    values.RightEyeBottom[i + 1],
                    transforms.RightEyeBottom[i + 1]);
            }
        }

        protected override void DoOverrideExpression(FacialTransformValues values,
            FacialTransform transforms,
            float weight)
        {
            base.DoOverrideExpression(values, transforms, weight);

            for (int i = 0; i < ExtraLeftTop.Length; i++)
            {
                //This could throw IndexOut???? >
                // > The game's design was made in such way that all have the same
                //amount of eye bones;
                //This could be a reminder if there's a problem related to that 
                //it should be changed with a Min(A.length,B.length) before the loop
                DoTransformOverride(weight,
                    ExtraLeftTop[i],
                    values.LeftEyeTop[i + 1],
                    transforms.LeftEyeTop[i + 1]
                ); //+1 because index[0] is root and it was calculated base
                DoTransformOverride(weight,
                    ExtraRightTop[i],
                    values.RightEyeTop[i + 1],
                    transforms.RightEyeTop[i + 1]

                );
            }
            //Same that the loop up there
            for (int i = 0; i < ExtraLeftBottom.Length; i++)
            {
                DoTransformOverride(weight,
                    ExtraLeftBottom[i],
                    values.LeftEyeBottom[i + 1],
                    transforms.LeftEyeBottom[i + 1]);
                DoTransformOverride(weight,
                    ExtraRightBottom[i],
                    values.RightEyeBottom[i + 1],
                    transforms.RightEyeBottom[i + 1]);
            }
        }

        public override void Serialize(FacialTransform structure)
        {
            base.Serialize(structure);
            ExtraLeftTop = new LocalPositionRotation[structure.LeftEyeTop.Length - 1];
            ExtraLeftBottom = new LocalPositionRotation[structure.LeftEyeBottom.Length - 1];
            ExtraRightTop = new LocalPositionRotation[structure.RightEyeTop.Length - 1];
            ExtraRightBottom = new LocalPositionRotation[structure.RightEyeBottom.Length - 1];

            //Same that DoAdditiveExpression
            for (int i = 0; i < ExtraLeftTop.Length; i++)
            {
                ExtraLeftTop[i] = new LocalPositionRotation(structure.LeftEyeTop[i + 1]);
                ExtraRightTop[i] = new LocalPositionRotation(structure.RightEyeTop[i + 1]);
            }
            for (int i = 0; i < ExtraLeftBottom.Length; i++)
            {
                ExtraLeftBottom[i] = new LocalPositionRotation(structure.LeftEyeBottom[i + 1]);
                ExtraRightBottom[i] = new LocalPositionRotation(structure.RightEyeBottom[i + 1]);
            }
        }

        public FullEyeLidExpression(bool isAdditive) : base(isAdditive)
        {
        }
    }

    
}
