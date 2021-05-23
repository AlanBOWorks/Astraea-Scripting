using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    public class HumanoidDataHolder : MonoBehaviour
    {
        [TabGroup("Transforms")] public HumanoidTransform HumanoidTransform = new HumanoidTransform();

        [TabGroup("Positions")] public HumanoidVector InitialLocalPositions = new HumanoidVector();
        [TabGroup("Positions")] public HumanoidVector AddedPositions = new HumanoidVector();

        [TabGroup("Local Rotations")] public HumanoidQuaternion InitialLocalRotations = new HumanoidQuaternion();
        [TabGroup("Local Rotations")] public HumanoidQuaternion AddedRotations = new HumanoidQuaternion();

        [TabGroup("Weights")] public HumanoidVector WeightsPosition = new HumanoidVector(Vector3.zero);
        [TabGroup("Weights")] public HumanoidVector WeightsRotation = new HumanoidVector(Vector3.zero);

        [Button]
        public void SerializeLocals()
        {
            InitialLocalPositions.InjectParse(HumanoidTransform,LocalPosition);
            Vector3 LocalPosition(Transform bone)
            {
                return bone.localPosition;
            }

            InitialLocalRotations.InjectParse(HumanoidTransform, LocalRotation);
            Quaternion LocalRotation(Transform bone)
            {
                return bone.localRotation;
            }
        }

        [Button]
        public void SerializeAddedValues(float positionModifier = 0.1f,float rotationModifier = 20)
        {
            Vector3 addition = Vector3.one * positionModifier;

            AddedPositions.InjectParse(InitialLocalPositions,AddPosition);
            Vector3 AddPosition(Vector3 original)
            {
                return original + addition;
            }

            Quaternion additionRotation = Quaternion.Euler(Vector3.one * rotationModifier);
            AddedRotations.InjectParse(InitialLocalRotations,AddRotation);

            Quaternion AddRotation(Quaternion original)
            {
                return original * additionRotation;
            }
        }
        

        [Button("Do Transform Position",ButtonSizes.Large)]
        public void DoTransformPosition()
        {
            HumanoidTransform.DoParse(InitialLocalPositions,AddedPositions,WeightsPosition,DoLerpTransform);

            void DoLerpTransform(Transform myTransform, Vector3 originalPosition, Vector3 targetPosition, Vector3 targetWeight)
            {
                Vector3 finalPosition = originalPosition;
                finalPosition.x = Mathf.LerpUnclamped(originalPosition.x, targetPosition.x, targetWeight.x);
                finalPosition.y = Mathf.LerpUnclamped(originalPosition.y, targetPosition.y, targetWeight.y);
                finalPosition.z = Mathf.LerpUnclamped(originalPosition.z, targetPosition.z, targetWeight.z);

                myTransform.localPosition = finalPosition;
            }
        }
    }

    [Serializable]
    public class HumanoidTransform : SerializerHumanoidStructure<Transform>
    {
        
    }

    [Serializable]
    public class HumanoidVector : HumanoidStructure<Vector3>
    {
        public HumanoidVector() : base()
        {}
        public HumanoidVector(Vector3 uniformValue) : base(uniformValue)
        {
            
        }
    }

    [Serializable]
    public class HumanoidQuaternion : HumanoidStructure<Quaternion>
    {

    }

}
