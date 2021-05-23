using System;
using UnityEngine;

namespace CharacterCreator
{
    [Serializable]
    public class VroidExpression
    {
        public float[] BlendWeights;
        

        public void InjectValues(SkinnedMeshRenderer renderer)
        {
            int amountOfShapes = renderer.sharedMesh.blendShapeCount;
            BlendWeights = new float[amountOfShapes];

            for (int i = 0; i < amountOfShapes; i++)
            {
                BlendWeights[i] = renderer.GetBlendShapeWeight(i);
            }
        }

        public void LerpBlend(SkinnedMeshRenderer renderer, float lerpAmount)
        {
            for (int i = 0; i < BlendWeights.Length; i++)
            {
                float targetWeight = Mathf.LerpUnclamped(renderer.GetBlendShapeWeight(i), BlendWeights[i], lerpAmount);
                renderer.SetBlendShapeWeight(i,targetWeight);
            }
        }
    }


    public interface IVroidExpressionHolder
    {
        VroidExpression GetVroidExpression();
    }
}
