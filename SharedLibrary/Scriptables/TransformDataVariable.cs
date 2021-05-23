using UnityEngine;

namespace SharedLibrary
{

    [CreateAssetMenu(fileName = "N - Transform Data [Variable]",
        menuName = "Variable/Transform Data")]
    public class TransformDataVariable : NonSerializedScriptableVariable<ICharacterTransformData>
    {

    }
}
