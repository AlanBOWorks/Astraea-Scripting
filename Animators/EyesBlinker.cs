
using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animators
{
    [Serializable]
    public class EyesBlinkerHandler
    {
        [Title("References")]
        [SerializeField] private SkinnedMeshRenderer _leftIris = null;
        [SerializeField] private SkinnedMeshRenderer _rightIris = null;
        private IBlinkHolder _facialExpressions;
        public void InjectHolder(IBlinkHolder holder)
        {
            _facialExpressions = holder;
        }


        private const int IrisFocusBlendIndex = 4;
        private int _blinkedCount;

        [Title("Params")]
        public SRange CloseSpeed = new SRange(40,60);
        public SRange OpenSpeed = new SRange(10,24);

        [SerializeField]
        private AnimationCurve _blinkCurve = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        [SerializeField]
        private AnimationCurve _irisFocusCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(.7f,1));
        private const float ComparisionThreshold = .01f;
        private int _blinkConsecutiveThreshold = 3;



        private bool _enabled = false;
        public CoroutineHandle BlinkingHandle { get; private set; }

        /// <summary>
        /// Will stop blinking after the animations are done
        /// </summary>
        public void SafeStopBlinking()
        {
            _enabled = false;
        }

        public void StartBlink()
        {
            Timing.KillCoroutines(BlinkingHandle);
            BlinkingHandle = Timing.RunCoroutine(_StartBlink());
        }

        public float BlinkWeight { get; private set; }
        private IEnumerator<float> _StartBlink()
        {
            _enabled = true;
            while (_enabled)
            {
                //Too guarantee a wait after many consecutive Blinks
                if (_blinkedCount < _blinkConsecutiveThreshold)
                {
                    yield return Timing.WaitForSeconds(Random.Range(0, .6f));
                    _blinkedCount++;
                }
                else
                {
                    yield return Timing.WaitForSeconds(Random.Range(3f, 5f));
                    _blinkedCount = 0;
                    _blinkConsecutiveThreshold = Random.Range(1, 3);
                }

                yield return Timing.WaitUntilDone(_DoBlink());

                if (Random.Range(0, 3) == 1)
                {
                    yield return Timing.WaitUntilDone(_DoBlink());
                }
            }

            IEnumerator<float> _DoBlink()
            {
                float eyesWeight = 0;
                float closeSpeed = CloseSpeed.RandomInRange();
                //Closing
                while (eyesWeight < 1 - ComparisionThreshold)
                {
                    UpdateWithCurveExpression(1, closeSpeed);

                    yield return Timing.WaitForSeconds(Time.deltaTime);
                }
                _facialExpressions.AnimateCloseExpression(1);

                yield return Timing.WaitForSeconds(Random.Range(0.1f, .4f));

                //Opening
                _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, 100); //Force the iris to shape in the "focus" state
                _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex, 100);

                BlinkWeight = _leftIris.GetBlendShapeWeight(IrisFocusBlendIndex);
                float openSpeed = OpenSpeed.RandomInRange();

                while (BlinkWeight > 0 + ComparisionThreshold)
                {
                    UpdateWithCurveExpression(0,openSpeed);

                    BlinkWeight = _irisFocusCurve.Evaluate(eyesWeight) * 100;
                    _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, BlinkWeight);
                    _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex,BlinkWeight);

                    yield return Timing.WaitForSeconds(Time.deltaTime);
                }

                _facialExpressions.AnimateCloseExpression(0);
                _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, 0);
                _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex, 0);


                void UpdateWithCurveExpression(float targetWeight, float deltaSpeed)
                {
                    eyesWeight = Mathf.Lerp(eyesWeight, targetWeight, Time.deltaTime * deltaSpeed);

                    _facialExpressions.AnimateCloseExpression(_blinkCurve.Evaluate(eyesWeight));
                }
            }

            
        }
    }

    public interface IBlinkHolder
    {
        void AnimateCloseExpression(float eyesWeight);
    }
}
