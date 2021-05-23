using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    [CreateAssetMenu(fileName = "N_CommonExpressions [Variable]",
        menuName = "CharacterCreator/Expression/CommonExpressions [Variable]")]
    public class CommonFacialExpressionVariable : ScriptableObject
    {
        public FullEyeLidExpression CloseEyeExpression = new FullEyeLidExpression(false);
    }
}
