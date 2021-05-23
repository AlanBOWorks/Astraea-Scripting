using System;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    /// <summary>
    /// Used for holding the default state of the <seealso cref="FacialCreationData"/>
    /// of the Facial Mesh's Bones.<br></br>
    /// Usually used for the facial's CharacterCreator tool to manipulate the bones.<br></br>
    /// <br></br>
    /// _ For the final results use the <seealso cref="GeneratedFacialDataHolder"/> instead which holds
    /// the final data after the modifications
    /// </summary>
    public class FacialDataHolder : MonoBehaviour
    {
        [Title("Variable")]
        [SerializeReference, GUIColor(.2f, .5f, 8f)] 
        private FacialTransformValuesVariable _defaultFacial = null;
        [Title("Structure")]
        public FacialTransform FacialTransform = new FacialTransform();
        public FacialTransformValues Data = new FacialTransformValues();

        public FacialTransformValues DefaultValues()
        {
            return _defaultFacial.Data;
        }

        [Button("Inject in SO",ButtonSizes.Large), GUIColor(.2f,.5f,8f)]
        public void InjectValuesInVariable(FacialTransformValuesVariable variable)
        {
            variable.Data.InitializeWithParse(FacialTransform,GenerateValues);
            FacialTransformValue GenerateValues(Transform bone)
            {
                return new FacialTransformValue(bone);
            }
        }

        [Button("Inject in SO", ButtonSizes.Large), GUIColor(.2f, .5f, 8f), ShowIf("_defaultFacial")]
        public void InjectValuesInDefault()
        {
            InjectValuesInVariable(_defaultFacial);
        }


        [Button("Injection from SO",ButtonSizes.Large), GUIColor(.2f,.5f,8f)]
        public void TransformFaceInjection(FacialTransformValuesVariable variable)
        {
            variable.Data.DoParse(FacialTransform,DoInject);
            void DoInject(FacialTransformValue values, Transform bone)
            {
                values.InjectInTransform(bone);
            }
        }

        [Button("Injection from SO", ButtonSizes.Large), GUIColor(.2f, .5f, 8f), ShowIf("_defaultFacial")]
        public void TransformFaceInjectionFromDefault()
        {
            TransformFaceInjection(_defaultFacial);
        }


        //TODO make a DeSerialization from save files for the player
        [Button]
        public void GenerateTransformValues()
        {
            Data.InjectTransforms(FacialTransform);
        }

        [Button("Serialize current Facial State", ButtonSizes.Large), GUIColor(.2f, .7f, .4f)]
        public void SerializeFacialState()
        {
            Data.ReSerialize(FacialTransform);
        }
        [Button, GUIColor(.7f, .7f, .4f)]
        public void RoundScale()
        {
            Data.RoundScale(3);
        }

        [Button, GUIColor(.6f, .4f, .4f)]
        public void LoadTransformsValues()
        {
            Data.DoParse(FacialTransform, DoLoad);
            void DoLoad(FacialTransformValue generatedData, Transform bone)
            {
                generatedData.InjectInTransform(bone);
            }
        }
    }

    [Serializable]
    public class FacialTransform : FacialStructure<Transform>
    {
       
    }
}

