using UnityEngine;

namespace IKEssentials
{
    public class UHandPresetTarget : UHandTargetBase, IHandTransformsTarget
    {
        [SerializeField] private SHandTargetData data = null;
        [SerializeField] private Transform handTarget = null;
        [SerializeField] private Transform bendTarget = null;

        public Transform GetHandTarget() => handTarget;
        public Transform GetBendTarget() => bendTarget;
        public override IHandTransformsTarget GetTransformsTarget() => this;
        public override IHandPersistentTargetData GetPersistentData() => data.GetData();
    }
}
