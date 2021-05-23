using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    /// <summary>
    /// Used primarily for the <seealso cref="Faciacre"/>
    /// </summary>
    public class FacialCreatorDataHolder : MonoBehaviour
    {
        [Title("References")]
        [SerializeReference] private FacialDataHolderReferencer _referencer = null;
        [SerializeReference] private FacialDataHolder _facialData = null;

        public FacialCreationStructure FacialStructure = new FacialCreationStructure();

        private void Awake()
        {
            if (_referencer != null)
                _referencer.InjectReference(this);
        }

        public FacialCreationStructure GetStructure()
        {
            return FacialStructure;
        }

        [Button, GUIColor(.5f, .5f, 0)]
        public void SerializeAdditions(float addedPositionModifier = 0.01f, float addedRotationModifier = 10, float addedSizeModifier = 0.02f)
        {
            Vector3 addedPosition = Vector3.one * addedPositionModifier;
            Vector3 addedRotation = Vector3.one * addedRotationModifier;
            Vector3 maxSize = Vector3.one * (1 + addedSizeModifier);
            FacialStructure.ReSerialize(addedPosition, addedRotation, maxSize);
        }

        [Button, GUIColor(.7f, .2f, 0)]
        public void InjectFromTransforms()
        {
            FacialStructure.InjectTransforms(_facialData.FacialTransform);
        }
    }


    [Serializable]
    public class FacialCreationStructure : FacialStructure<FacialCreationData>
    {
        public void InjectTransforms(FacialTransform facialTransforms)
        {
            InitializeWithParse(facialTransforms, InjectTransform);
            FacialCreationData InjectTransform(Transform bone)
            {
                return new FacialCreationData(bone);
            }
        }

        public void ReSerialize(Vector3 addedPosition, Vector3 addedRotation, Vector3 maxSize)
        {
            foreach (FacialCreationData data in GenerateList())
            {
                data.ReSerialize(addedPosition, addedRotation, maxSize);
            }
        }


    }

    

    /// <summary>
    /// Contains all the necessary data for the Character Creator (Position, Rotation, Scales and their respective
    /// weight in relation of a max value).
    /// <br></br>
    /// It should be used mainly for the Main Character and Not recommended for "The Four" NPC 
    /// </summary>
    [Serializable]
    public class FacialCreationData
    {
        public Transform Bone;
        [TitleGroup("Positions")]
        public Vector3 LocalPosition;

        [TitleGroup("Rotations")]
        public Vector3 LocalRotation;


        /// <summary>
        /// Used for <seealso cref="Vector3.LerpUnclamped"/>
        /// </summary>
        [TitleGroup("Positions")]
        public Vector3 MaxLocalPosition;
        /// <summary>
        /// <inheritdoc cref="MaxLocalPosition"/> where the <seealso cref="Vector3"/>(x,y,z) are the
        /// weights used for the respective <seealso cref="Vector3.LerpUnclamped"/>(x,y,z) amount
        /// </summary>
        [TitleGroup("Positions")]
        public Vector3 PositionWeights;


        /// <summary>
        /// <inheritdoc cref="MaxLocalPosition"/>
        /// </summary>
        [TitleGroup("Rotations")]
        public Vector3 MaxLocalRotation;
        /// <summary>
        /// <inheritdoc cref="PositionWeights"/>
        /// </summary>
        [TitleGroup("Rotations")]
        public Vector3 RotationWeights;

        /// <summary>
        /// <inheritdoc cref="MaxLocalPosition"/>
        /// <br></br>
        /// For its minValue it will use <seealso cref="Vector3.one"/>
        /// </summary>
        [TitleGroup("Scale")]
        public Vector3 MaxScale;
        /// <summary>
        /// Global weight for the Scale
        /// </summary>
        public float ScaleWeight;

        /// <summary>
        /// Used mainly for wideness
        /// </summary>
        public float XScaleModifier;
        /// <summary>
        /// Used mainly for height
        /// </summary>
        public float YScaleModifier;

        public FacialCreationData(Transform bone)
        {
            Bone = bone;
            ReSerialize();
        }
        public void ReSerialize()
        {
            LocalPosition = Bone.localPosition;
            LocalRotation = Bone.localRotation.eulerAngles;
        }

        public void ReSerialize(Vector3 addedPosition, Vector3 addedRotation, Vector3 maxSize)
        {
            ReSerialize();
            MaxLocalPosition = LocalPosition + addedPosition;
            MaxLocalRotation = LocalRotation + addedRotation;
            MaxScale = maxSize;
        }
    }
}
