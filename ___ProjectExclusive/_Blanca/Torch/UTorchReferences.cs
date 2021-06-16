using UnityEngine;

namespace Blanca
{
    public class UTorchReferences : MonoBehaviour, ITorchTargets<Transform>
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform leftHandRoot;
        [SerializeField] private Transform leftBendTarget;
        [SerializeField] private Transform rightHandRoot;
        [SerializeField] private Transform rightBendTarget;

        public Transform Holder
        {
            get => holder;
            set => holder = value;
        }

        public Transform LeftHandRoot
        {
            get => leftHandRoot;
            set => leftHandRoot = value;
        }

        public Transform LeftBendTarget
        {
            get => leftBendTarget;
            set => leftBendTarget = value;
        }

        public Transform RightHandRoot
        {
            get => rightHandRoot;
            set => rightHandRoot = value;
        }

        public Transform RightBendTarget
        {
            get => rightBendTarget;
            set => rightBendTarget = value;
        }

        public Transform[] Elements { get; private set; }

        private void Awake()
        {
            Elements = TorchTargetsBase.GenerateElements(this);
        }
    }
}
