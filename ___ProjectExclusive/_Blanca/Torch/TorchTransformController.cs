using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca
{
    public class TorchTransformController
    {

        [ShowInInspector]
        private readonly TorchLocalPositionData _initialLocalPosition;
        [ShowInInspector]
        private readonly TorchLocalRotationData _initialLocalRotation;
        [ShowInInspector]
        private readonly ITorchTargets<Transform> _originalTargets;
        [ShowInInspector]
        private readonly ITorchTargets<Transform> _animationTargets;
        [ShowInInspector]
        private readonly ITorchStructure<Transform> _holders;

        [Range(0, 1)] public float AnimationWeight; 

        public TorchTransformController(TorchHoldersReferences references, ITorchStructure<Transform> holders, float animationWeight = 0)
        {
            _originalTargets = references.Original;
            _initialLocalPosition = new TorchLocalPositionData(_originalTargets);
            _initialLocalRotation = new TorchLocalRotationData(_originalTargets);

            _animationTargets = references.Animation;

            AnimationWeight = animationWeight;

            _holders = holders;

            _updateHandle = Timing.RunCoroutine(_DoUpdate(), Segment.LateUpdate);
        }

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Timing.ResumeCoroutines(_updateHandle);
                }
                else
                {
                    Timing.PauseCoroutines(_updateHandle);
                }
            }
        }

        private CoroutineHandle _updateHandle;
        private IEnumerator<float> _DoUpdate()
        {
            yield return Timing.WaitForOneFrame; //this is to avoid null reference on Awake Instantiation
            while (_originalTargets != null)
            {
                yield return Timing.WaitForOneFrame;
                DoHoldersLerp();
                DoLerp();
            }
        }


        private void DoHoldersLerp()
        {
            Transform bone = _originalTargets.Holder;
            Transform animatedBone = _animationTargets.Holder;
            Transform proceduralBone = _holders.Procedural;

            bone.position = Vector3.LerpUnclamped(proceduralBone.position,animatedBone.position,
                AnimationWeight);
            bone.rotation = Quaternion.SlerpUnclamped(proceduralBone.rotation,animatedBone.rotation,
                AnimationWeight);
        }
        private void DoLerp()
        {
            Transform[] bones = _originalTargets.Elements;
            Transform[] animatedTransforms = _animationTargets.Elements;
            Vector3[] originalLocals = _initialLocalPosition.Elements;
            Quaternion[] originalRotations = _initialLocalRotation.Elements;

            int length = bones.Length;
            //Skips holders
            for (int i = TorchTargetsBase.LeftHandIndex; i < length; i++)
            {
                LerpTransform(bones[i],originalLocals[i],originalRotations[i],animatedTransforms[i]);
            }
        }



        private void LerpTransform(Transform bone, Vector3 originalPosition, Quaternion originalRotation, Transform animated)
        {
            bone.localPosition = Vector3.LerpUnclamped(originalPosition, animated.localPosition, AnimationWeight);
            bone.localRotation = Quaternion.SlerpUnclamped(originalRotation, animated.rotation, AnimationWeight);
        }
    }
}
