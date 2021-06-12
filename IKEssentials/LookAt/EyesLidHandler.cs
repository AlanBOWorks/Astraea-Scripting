using System;
using Animators;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace IKEssentials
{
    public class EyesLidHandler : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private IrisLookAt irisLookAt = null;

        [SerializeField] private Transform leftLid = null;
        [SerializeField] private Transform rightLid = null;

        [Title("Parameters")]
        [SerializeField,DisableInPlayMode] private EyesLidParameters parameters = new EyesLidParameters();

        [SuffixLabel("%/normalized")]
        public SRange upRange = new SRange(.02f,.1f);
        [NonSerialized]
        public Quaternion LidUpRotationAddition;

        [SuffixLabel("%/%")]
        public SRange downRange = new SRange(-.08f, -.1f);
        [NonSerialized]
        public Quaternion LidDownRotationAddition;

        private void Awake()
        {
            LidUpRotationAddition = Quaternion.Euler(parameters.lidUpLimits);
            LidDownRotationAddition = Quaternion.Euler(parameters.lidDownLimits);

            parameters = null;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            float yLookAt = irisLookAt.ClampedLocalDirection.y;
            float upPercentage = upRange.PercentageUnitInterval(yLookAt);
            float downPercentage = downRange.PercentageUnitInterval(yLookAt);


            Quaternion rotationOffset = Quaternion.LerpUnclamped(
                Quaternion.identity, 
                LidUpRotationAddition,
                upPercentage);
            rotationOffset *= Quaternion.LerpUnclamped(
                Quaternion.identity, 
                LidDownRotationAddition,
                downPercentage);

            leftLid.localRotation *= rotationOffset;
            rightLid.localRotation *= rotationOffset;
        }

        [Serializable]
        internal class EyesLidParameters
        {
            public Vector3 lidUpLimits;
            public Vector3 lidDownLimits;
        }
    }
}
