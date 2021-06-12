using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    public class FingersIKControl : IFingersIKSolver
    {
        private readonly FingersControls _controls;
        public MonoBehaviour NullFallback;
        public float Weight = 0;

        public float FingersWeight
        {
            get => Weight;
            set => Weight = value;
        }

        [ShowInInspector]
        public float Index
        {
            get => _controls.Index.Closeness;
            set => _controls.Index.Closeness = value;
        }
        [ShowInInspector]
        public float Middle
        {
            get => _controls.Middle.Closeness;
            set => _controls.Middle.Closeness = value;
        }
        [ShowInInspector]
        public float Ring
        {
            get => _controls.Ring.Closeness;
            set => _controls.Ring.Closeness = value;
        }
        [ShowInInspector]
        public float Little
        {
            get => _controls.Little.Closeness;
            set => _controls.Little.Closeness = value;
        }
        [ShowInInspector]
        public float Thumb
        {
            get => _controls.Thumb.Closeness;
            set => _controls.Thumb.Closeness = value;
        }

        [ShowInInspector]
        public float ThumbTorsion
        {
            get => _controls.ThumbRoot.Closeness;
            set => _controls.ThumbRoot.Closeness = value;
        }
        [ShowInInspector]
        public float ThumbWide
        {
            get => _controls.ThumbRoot.WideAmount;
            set => _controls.ThumbRoot.WideAmount = value;
        }

        public void HandleWeights(IFingerStructure<float> targetWeights, float lerpAmount)
        {
            Index = Mathf.Lerp(Index,targetWeights.Index,lerpAmount); 
            Middle = Mathf.Lerp(Middle, targetWeights.Middle, lerpAmount);
            Ring = Mathf.Lerp(Ring, targetWeights.Ring, lerpAmount);
            Little = Mathf.Lerp(Little, targetWeights.Little, lerpAmount);
            Thumb = Mathf.Lerp(Thumb, targetWeights.Thumb, lerpAmount);
        }

        public void HandleWeights(IFingerStructure<float> targetWeights)
        {
            Index = targetWeights.Index;
            Middle = targetWeights.Middle;
            Ring = targetWeights.Ring;
            Little = targetWeights.Little;
            Thumb = targetWeights.Thumb;
        }

        public void HandleWeights(IFingerWeights targetWeights, float lerpAmount)
        {
            Weight = Mathf.Lerp(Weight, targetWeights.FingersWeight, lerpAmount);
            HandleWeights(targetWeights as IFingerStructure<float>,lerpAmount);

            ThumbTorsion = Mathf.Lerp(ThumbTorsion, targetWeights.ThumbTorsion, lerpAmount);
            ThumbWide = Mathf.Lerp(ThumbWide, targetWeights.ThumbWide, lerpAmount);
        }

        public void HandleWeights(IFingerWeights targetWeights)
        {
            Weight = targetWeights.FingersWeight;
            HandleWeights(targetWeights as IFingerStructure<float>);

            ThumbTorsion = targetWeights.ThumbTorsion;
            ThumbWide = targetWeights.ThumbWide;
        }
        public void HandleWeights(FingersWeight targetWeights)
        {
            Weight = targetWeights.MasterWeight;
            Index = targetWeights.Index;
            Middle = targetWeights.Middle;
            Ring = targetWeights.Ring;
            Little = targetWeights.Little;
            Thumb = targetWeights.Thumb;

            ThumbTorsion = targetWeights.ThumbTorsion;
            ThumbWide = targetWeights.ThumbWide;
        }


        public FingersIKControl (FingersHolder holder, MonoBehaviour nullFallback)
        {
            NullFallback = nullFallback;
            _controls = new FingersControls(holder,holder);
            Timing.RunCoroutine(LateUpdate(), Segment.LateUpdate);
        }

        private IEnumerator<float> LateUpdate()
        {
            while (NullFallback != null)
            {
                if (Weight <= 0) yield return Timing.WaitForOneFrame;
                foreach (IFingerClosenessControl control in _controls.Fingers)
                {
                    control.HandleIK(Weight);
                }
                

                yield return Timing.WaitForOneFrame;
            }
        }


        internal class FingersControls : IFingerStructure<IFingerClosenessControl>
        {
            public IFingerClosenessControl Index { get; }
            public IFingerClosenessControl Middle { get; }
            public IFingerClosenessControl Ring { get; }
            public IFingerClosenessControl Little { get; }
            public IFingerClosenessControl Thumb { get; }
            public ThumbControl ThumbRoot { get; private set; }

            public IFingerClosenessControl[] Fingers { get; }

            public FingersControls(IFingerStructure<Transform[]> holder, IFingerLimits limits)
            {
                Vector3 fingersAngleLimits = limits.MainFingerAngleLimits;
                Vector3 thumbAngleLimit = limits.ThumbAngleLimits;
                Vector3 thumbTorsionLimits = limits.ThumbTorsionLimits;
                Vector3 thumbWideLimits = limits.ThumbWideLimits;

                Index = InstantiateMain(holder.Index);
                Middle = InstantiateMain(holder.Middle);
                Ring = InstantiateMain(holder.Ring);
                Little = InstantiateMain(holder.Little);
                Thumb = InstantiateThumb();
                ThumbRoot = new ThumbControl(holder.Thumb[0],thumbTorsionLimits, thumbWideLimits);


                Fingers = new[]
                {
                    Index,
                    Middle,
                    Ring,
                    Little,
                    Thumb,
                    ThumbRoot
                };


                IFingerClosenessControl InstantiateMain(Transform[] bones)
                {
                    int length = bones.Length;
                    IFingerClosenessControl[] generated = new IFingerClosenessControl[length];
                    for (int i = 0; i < length; i++)
                    {
                        generated[i] = new FingerControl(bones[i],fingersAngleLimits);
                    }

                    return new FingersControl(generated);
                }

                IFingerClosenessControl InstantiateThumb()
                {
                    //The thumb root is removed
                    IFingerClosenessControl[] generated = new IFingerClosenessControl[holder.Thumb.Length-1];
                    for (int i = 0; i < generated.Length; i++)
                    {
                        generated[i] = new FingerControl(holder.Thumb[i+1], thumbAngleLimit); 
                    }

                    return new FingersControl(generated);
                }
            }
        }

        internal class FingersControl : IFingerClosenessControl
        {

            public readonly IFingerClosenessControl[] Fingers;
            public FingersControl(IFingerClosenessControl[] fingers)
            {
                Fingers = fingers;
                _closeness = 0;
            }

            private float _closeness;
            public float Closeness
            {
                get => _closeness;
                set
                {
                    _closeness = value;
                    foreach (IFingerClosenessControl control in Fingers)
                    {
                        control.Closeness = value;
                    }
                }
            }
            public void HandleIK(float ikWeight)
            {
                foreach (IFingerClosenessControl control in Fingers)
                {
                    control.HandleIK(ikWeight);
                }
            }
        }


        internal class FingerControl : IFingerClosenessControl
        {
            private readonly Transform _finger;
            protected readonly Quaternion InitialRotation;
            protected readonly Quaternion TargetRotation;
            public float Closeness { get; set; } = 0;
        
            /// <param name="angleLimits">Generally 80º are a good limit</param>
            public FingerControl(Transform finger, Vector3 angleLimits)
            {
                _finger = finger;
                InitialRotation = finger.localRotation;
                TargetRotation = InitialRotation * Quaternion.Euler(angleLimits);
            }

            public void HandleIK(float ikWeight)
            {
                _finger.localRotation = CalculateTargetLocalRotation(ikWeight);
            }

            protected virtual Quaternion CalculateTargetLocalRotation(float ikWeight)
            {
                Quaternion closenessRotation =
                    Quaternion.SlerpUnclamped(InitialRotation, TargetRotation, Closeness);
                return Quaternion.SlerpUnclamped(
                    _finger.localRotation,
                    closenessRotation,
                    ikWeight);;
            }
        }

        internal class ThumbControl : FingerControl
        {
            protected readonly Quaternion WideRotation;
            public float WideAmount { get; set; }

            public ThumbControl(Transform finger, Vector3 angleLimits, Vector3 wideLimits) : base(finger, angleLimits)
            {
                WideRotation = Quaternion.Euler(wideLimits);
            }

            protected override Quaternion CalculateTargetLocalRotation(float ikWeight)
            {
                Quaternion wideAddition = Quaternion.SlerpUnclamped(Quaternion.identity, WideRotation, WideAmount);
                return base.CalculateTargetLocalRotation(ikWeight) * wideAddition;
            }
        }

    }

    [Serializable]
    public class FingersHolder : IFingerStructure<Transform[]>, IFingerLimits
    {
        [SerializeField]
        private Transform[] index;
        [SerializeField]
        private Transform[] middle;
        [SerializeField]
        private Transform[] ring;
        [SerializeField]
        private Transform[] little;
        [SerializeField, InfoBox("[0]: Thumb root for angle control")]
        private Transform[] thumb;


        public Transform[] Index => index;
        public Transform[] Middle => middle;
        public Transform[] Ring => ring;
        public Transform[] Little => little;
        public Transform[] Thumb => thumb;


        [SerializeField] private Vector3 mainFingersAngleLimit = new Vector3(0,0,80);
        public Vector3 MainFingerAngleLimits => mainFingersAngleLimit;
        [SerializeField] private Vector3 thumbAngleLimit = new Vector3(0,0,80);
        public Vector3 ThumbAngleLimits => thumbAngleLimit;
        [SerializeField] private Vector3 thumbTorsionLimit = new Vector3(0, 40, 00);
        public Vector3 ThumbTorsionLimits => thumbTorsionLimit;
        [SerializeField] private Vector3 thumbWideLimits = new Vector3(-40,0,00);
        public Vector3 ThumbWideLimits => thumbWideLimits;
    }

    /// <summary>
    /// Used mainly for static values
    /// </summary>
    public struct FingersWeight 
    {
        public static readonly FingersWeight Zero 
            = new FingersWeight(0,0,0,0,0,0,0,0);
        public static readonly FingersWeight One
            = new FingersWeight(1, 1, 1, 1, 1, 1, 1, 1);
        public static readonly FingersWeight Half
            = new FingersWeight(.5f, .5f, .5f, .5f, .5f, .5f, .5f, .5f);
        /// <summary>
        /// Value = .95f
        /// </summary>
        public static readonly FingersWeight SemiOne
            = new FingersWeight(.95f, .95f, .95f, .95f, .95f, .95f, .95f, .95f);
        
        public float MasterWeight { get; set; }
        public float Index { get; set; }
        public float Middle { get; set; }
        public float Ring { get; set; }
        public float Little { get; set; }
        public float Thumb { get; set; }
        public float ThumbWide { get; }
        public float ThumbTorsion { get; }

        public FingersWeight(float masterWeight, float index,
            float middle,
            float ring,
            float little,
            float thumb,
            float thumbWide,
            float thumbTorsion)
        {
            MasterWeight = masterWeight;

            Index = index;
            Middle = middle;
            Ring = ring;
            Little = little;
            Thumb = thumb;
            ThumbWide = thumbWide;
            ThumbTorsion = thumbTorsion;
        }

    }

    public interface IFingerLimits
    {
        Vector3 MainFingerAngleLimits { get; }
        Vector3 ThumbAngleLimits { get; }
        Vector3 ThumbTorsionLimits { get; }
        Vector3 ThumbWideLimits { get; }
    }

    public interface IFingerStructure<out T>
    {
        T Index { get; }
        T Middle { get; }
        T Ring { get; }
        T Little { get; }
        T Thumb { get; }
    }
    public interface IFingerSetStructure<in T>
    {
        T Index { set; }
        T Middle { set; }
        T Ring { set; }
        T Little { set; }
        T Thumb { set; }
    }

    public interface IFingersIKSolver : IFingerWeights
    {
        void HandleWeights(IFingerStructure<float> targetWeights, float lerpAmount);
        void HandleWeights(IFingerStructure<float> targetWeights);
        void HandleWeights(IFingerWeights targetWeights, float lerpAmount);
        void HandleWeights(IFingerWeights targetWeights);
        void HandleWeights(FingersWeight targetWeights);

    }

    public interface IFingerWeights : IFingerStructure<float>, IFingerSetStructure<float>
    {
        float FingersWeight { get; set; }
        float ThumbWide { get; }
        float ThumbTorsion { get; }
    }

    public interface IFingerClosenessControl
    {
        float Closeness { get; set; }
        void HandleIK(float ikWeight);
    }

    [Serializable]
    public class FingersSolverConstructor
    {
        public FingersHolder leftFingers = new FingersHolder();
        public FingersHolder rightFingers = new FingersHolder();

        private IFingersIKSolver _leftSolver = null;
        private IFingersIKSolver _rightSolver = null;
        public IFingersIKSolver GetOrCreateLeftSolver(MonoBehaviour nullCallback)
        {
            return _leftSolver ??
                   (_leftSolver = new FingersIKControl(leftFingers, nullCallback));
        }
        public IFingersIKSolver GetOrCreateRightSolver(MonoBehaviour nullCallback)
        {
            return _rightSolver ??
                   (_rightSolver = new FingersIKControl(rightFingers, nullCallback));
        }
    }
}
