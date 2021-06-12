using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class UHandTargetData : UHandTargetBase
    {
        [SerializeField] private HandTargetData data = new HandTargetData();
        public override IHandTransformsTarget GetTransformsTarget() => data;
        public override IHandPersistentTargetData GetPersistentData() => data;
    }


    public abstract class UHandTargetBase : MonoBehaviour
    {
        public abstract IHandTransformsTarget GetTransformsTarget();
        public abstract IHandPersistentTargetData GetPersistentData();
    }

    [Serializable]
    public class HandTargetData : HandPersistentTargetData, IHandTargetData
    {
        [SerializeField] private Transform handTarget = null;
        [SerializeField] private Transform bendTarget = null;

        public Transform GetHandTarget()
        {
            return handTarget;
        }
        public Transform GetBendTarget()
        {
            return bendTarget;
        }
    }


    /// <summary>
    /// <inheritdoc cref="IHandPersistentTargetData"/>
    /// </summary>
    [Serializable]
    public class HandPersistentTargetData : IHandPersistentTargetData
    {
        [BoxGroup("Hand")]
        [Title("Basic")]
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float positionWeight = 1;
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float rotationWeight = 1;
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float bendWeight = 1;

        [Title("Chain effects")]
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float reachWeight = .1f;
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float push = .5f;
        [SerializeField, Range(0, 1), BoxGroup("Hand")] private float pushParent = .2f;

        [BoxGroup("Fingers")]
        [Title("Master")]
        [SerializeField, Range(0, 1)] private float fingersWeight = 0;

        [Title("Singles")]
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float index;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float middle;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float ring;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float little;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float thumb;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float thumbWide;
        [SerializeField, Range(-4, 4), BoxGroup("Fingers")] private float thumbTorsion;


        public float Index
        {
            get => index;
            set => index = value;
        }

        public float Middle
        {
            get => middle;
            set => middle = value;
        }

        public float Ring
        {
            get => ring;
            set => ring = value;
        }

        public float Little
        {
            get => little;
            set => little = value;
        }

        public float Thumb
        {
            get => thumb;
            set => thumb = value;
        }

        public float FingersWeight
        {
            get => fingersWeight;
            set => fingersWeight = value;
        }

        public float ThumbWide => thumbWide;

        public float ThumbTorsion => thumbTorsion;


        public float PositionWeight => positionWeight;

        public float RotationWeight => rotationWeight;


        public float BendWeight => bendWeight;


        public float ReachWeight => reachWeight;

        public float PushWeight => push;

        public float PushParent => pushParent;
    }


    public interface IHandTargetData : IHandPersistentTargetData, IHandTransformsTarget
    {}
    /// <summary>
    /// Contains data that persist within scenes
    /// </summary>
    public interface IHandPersistentTargetData : IHandTransformWeights, IHandPushWeights, IFingerWeights 
    {}

    public interface IHandTransformsTarget
    {
        Transform GetHandTarget();
        Transform GetBendTarget();
    }

    public interface IHandTransformWeights
    {
        float PositionWeight { get; }
        float RotationWeight { get; }
        float BendWeight { get; }
    }

    public interface IHandPushWeights
    {
        float ReachWeight { get; }
        float PushWeight { get; }
        float PushParent { get; }
    }
}
