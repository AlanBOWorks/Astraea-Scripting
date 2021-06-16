using System;
using Animators;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace IKEssentials
{
    public class EyesLidHandler : MonoBehaviour, IBlinkListener
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
            _blinkModifier = 1;
        }

        [ShowInInspector,Range(0,1),DisableInEditorMode]
        private float _blinkModifier;
        private void LateUpdate()
        {
            float yLookAt = irisLookAt.ClampedLocalDirection.y;
            float upPercentage = upRange.PercentageUnitInterval(yLookAt);
            float downPercentage = downRange.PercentageUnitInterval(yLookAt);
            upPercentage *= _blinkModifier;
            downPercentage *= _blinkModifier;

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

        public void DoBlink(float blinkPercentage)
        {
            _blinkModifier = 1 - blinkPercentage;
        }
    }
}
