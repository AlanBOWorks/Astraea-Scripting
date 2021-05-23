using SharedLibrary;
using UnityEngine;

namespace CharacterCreator
{
    [CreateAssetMenu(fileName = "N_Full Facial - N_Expression [Variable]",
        menuName = "CharacterCreator/Expression/Full Facial [Variable]")]
    public class FullFacialExpressionVariable : ScriptableObject, IFullFacialExpression
    {
        public FacialTransformValues TransformValues = new FacialTransformValues();
        public VroidExpression BlendShapes = new VroidExpression();

        public void SerializeValues(FacialTransform transforms, SkinnedMeshRenderer renderer)
        {
            TransformValues.InjectTransforms(transforms);
            TransformValues.ReSerialize(transforms);
            BlendShapes.InjectValues(renderer);
        }

        public FacialTransformValues GetTransformValues()
        {
            return TransformValues;
        }

        public VroidExpression GetVroidExpression()
        {
            return BlendShapes;
        }
    }
}
