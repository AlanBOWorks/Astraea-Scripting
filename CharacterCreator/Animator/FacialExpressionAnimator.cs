using System;
using System.Collections.Generic;
using Animators;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    public class FacialExpressionAnimator : MonoBehaviour, IBlinkHolder
    {
        [TabGroup("References")]
        [SerializeReference] private FacialDataHolder _dataHolder = null;
        [TabGroup("References")]
        [SerializeReference] private SkinnedMeshRenderer _meshRenderer = null;
        [TabGroup("References")]
        public CommonFacialExpressionVariable CommonExpressionVariable = null;//TODO change to Interface related


        [TabGroup("Animators")] 
        [Title("Blinker")]
        public EyesBlinkerHandler Blinker = new EyesBlinkerHandler();



        private void Start()
        {
            Blinker.InjectHolder(this);
            Blinker.StartBlink();
        }

        private void OnEnable()
        {
            Blinker.Enabled = true;
        }

        private void OnDisable()
        {
            Blinker.Enabled = false;
        }

        [Button(ButtonSizes.Large),HideInPlayMode, ShowIf("IsEditingMode"), GUIColor(.6f, .6f, .2f)]
        private void SerializeCloseEyes()
        {
            CommonExpressionVariable.CloseEyeExpression.Serialize(_dataHolder.FacialTransform);
        }

        public void AnimateCloseExpression(float weight)
        {
            CommonExpressionVariable.CloseEyeExpression.DoExpression(
                _dataHolder.Data,
                _dataHolder.FacialTransform,
                weight);
        }

        private void LerpFacialTransforms(IFacialTransformValuesHolder expression, float lerpAmount)
        {
            _dataHolder.Data = expression.GetTransformValues();
            _dataHolder.FacialTransform.DoParse(expression.GetTransformValues(), LerpBone);


            void LerpBone(Transform bone, FacialTransformValue values)
            {
                values.LerpTransform_NoScale(bone, lerpAmount);
            }
        }

        private void LerpVroid(IVroidExpressionHolder expression, float lerpAmount)
        {
            expression.GetVroidExpression().LerpBlend(_meshRenderer, lerpAmount);

        }

        private CoroutineHandle _expressionHandle;

        public void AnimateToExpression(IFullFacialExpression expression, float changeSpeed =2)
        {
            Timing.KillCoroutines(_expressionHandle);

            _expressionHandle = Timing.RunCoroutine(_DoChange());
            IEnumerator<float> _DoChange()
            {
                float lerpAmount = changeSpeed * Time.deltaTime;
                while (lerpAmount < 1)
                {
                    DoLerp();
                    yield return Timing.WaitForOneFrame;
                    lerpAmount += changeSpeed * Time.deltaTime;
                }

                lerpAmount = 1;
                DoLerp();


                void DoLerp()
                {
                    LerpFacialTransforms(expression,lerpAmount);
                    LerpVroid(expression,lerpAmount);
                }
                
            }
        }

        [Button, HideIf("IsEditingMode"), GUIColor(.6f, .6f, .2f)]
        private void AnimateToExpression(FullFacialExpressionVariable variable, float changeSpeed = 2)
        {
            AnimateToExpression(variable as IFullFacialExpression,changeSpeed);
        }

        [Button, ShowIf("IsEditingMode"), GUIColor(.6f, .6f, .2f)]
        public void InjectInVariable(FullFacialExpressionVariable expressionVariable)
        {
            expressionVariable.SerializeValues(_dataHolder.FacialTransform,_meshRenderer);
        }

#if UNITY_EDITOR
        private bool _isTesting;
        [SerializeField,ShowIf("_isTesting"),Range(0,1)] private float _testWeight = 0;

        [Button,GUIColor(.4f,.4f,.4f),HideIf("_isTesting")]
        private void Test()
        {
            Timing.RunCoroutine(_DoTest(), gameObject);
        }

        [Button,GUIColor(.4f,.4f,.4f), ShowIf("_isTesting")]
        private void CancelTest()
        {
            _isTesting = false;
        }

        private IEnumerator<float> _DoTest()
        {
            _testWeight = 0;
            _isTesting = true;
            while (_isTesting)
            {
                CommonExpressionVariable.CloseEyeExpression.DoExpression(
                    _dataHolder.Data,
                    _dataHolder.FacialTransform,
                    _testWeight);
                yield return Timing.WaitForOneFrame;
            }
        }
#endif
    }

}
