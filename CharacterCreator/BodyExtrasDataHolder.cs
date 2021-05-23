using System;
using UnityEngine;

namespace CharacterCreator
{
    public class BodyExtrasDataHolder : MonoBehaviour
    {
        public BodyExtraTransforms ExtraTransforms = new BodyExtraTransforms();
    }

    [Serializable]
    public class BodyExtraTransforms : BodyExtrasStructure<Transform>{}
}
