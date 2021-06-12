using UnityEngine;

namespace IKEssentials
{
    [CreateAssetMenu(fileName = "Hand Target Data [Variable]",
        menuName = "Variable/IK/Hand Target Data")]
    public class SHandTargetData : ScriptableObject
    {
        [SerializeField] private HandPersistentTargetData data = new HandPersistentTargetData();
        public IHandPersistentTargetData GetData() => data;
    }
}
