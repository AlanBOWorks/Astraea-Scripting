
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
    public class EyesBlinkerHandler : ISerializationCallbackReceiver
    {
        [Title("References")]
        [SerializeField] private SkinnedMeshRenderer _leftIris = null;
        [SerializeField] private SkinnedMeshRenderer _rightIris = null;
        private IBlinkHolder _facialExpressions;
        public void InjectHolder(IBlinkHolder holder)
        {
            _facialExpressions = holder;
        }

        private List<IBlinkListener> _listeners;
        public List<IBlinkListener> Listeners => _listeners;


        private const int IrisFocusBlendIndex = 4;
        private int _blinkedCount;

        [Title("Params")]
        public SRange CloseSpeed = new SRange(40,60);
        public SRange OpenSpeed = new SRange(10,24);

        [SerializeField]
        private AnimationCurve _blinkClosenessCurve = new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
        [SerializeField]
        private AnimationCurve _irisFocusCurve = new AnimationCurve(new Keyframe(0,0), new Keyframe(.7f,1));
        private const float ComparisionThreshold = .01f;
        private int _blinkConsecutiveThreshold = 3;


        public bool Enabled
        {
            set
            {
                if (value)
                    Timing.ResumeCoroutines(_blinkingHandle);
                else
                    Timing.PauseCoroutines(_blinkingHandle);
            }
        }

        private CoroutineHandle _blinkingHandle;


        public void StartBlink()
        {
            Timing.KillCoroutines(_blinkingHandle);
            _blinkingHandle = Timing.RunCoroutine(_StartBlink());
        }

        public float BlinkWeight { get; private set; }
        private IEnumerator<float> _StartBlink()
        {
            while (_facialExpressions != null)
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
                    InvokeListeners();

                    yield return Timing.WaitForOneFrame;
                }

                eyesWeight = 1;
                _facialExpressions.AnimateCloseExpression(1);
                InvokeListeners();

                yield return Timing.WaitForSeconds(Random.Range(0.1f, .4f));

                //Opening
                _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, 100); //Force the iris to shape in the "focus" state
                _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex, 100);

                BlinkWeight = _leftIris.GetBlendShapeWeight(IrisFocusBlendIndex);
                float openSpeed = OpenSpeed.RandomInRange();

                while (BlinkWeight > 0 + ComparisionThreshold)
                {
                    UpdateWithCurveExpression(0,openSpeed);
                    InvokeListeners();

                    BlinkWeight = _irisFocusCurve.Evaluate(eyesWeight) * 100;
                    _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, BlinkWeight);
                    _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex,BlinkWeight);

                    yield return Timing.WaitForOneFrame;
                }

                eyesWeight = 0;
                _facialExpressions.AnimateCloseExpression(eyesWeight);
                _leftIris.SetBlendShapeWeight(IrisFocusBlendIndex, eyesWeight);
                _rightIris.SetBlendShapeWeight(IrisFocusBlendIndex, eyesWeight);
                InvokeListeners();

                void UpdateWithCurveExpression(float targetWeight, float deltaSpeed)
                {
                    eyesWeight = Mathf.Lerp(eyesWeight, targetWeight, Time.deltaTime * deltaSpeed);

                    _facialExpressions.AnimateCloseExpression(_blinkClosenessCurve.Evaluate(eyesWeight));
                }

                void InvokeListeners()
                {
                    foreach (IBlinkListener listener in _listeners)
                    {
                        listener.DoBlink(eyesWeight);
                    }
                }
            }

            
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _listeners = new List<IBlinkListener>(4);
        }
    }

    public interface IBlinkHolder
    {
        void AnimateCloseExpression(float eyesWeight);
    }

    public interface IBlinkListener
    {
        void DoBlink(float blinkPercentage);
    }
}
