using System.Collections.Generic;
using CharacterCreator;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;

using UnityEngine;

namespace CharacterCreator
{
    [CreateAssetMenu(fileName = "FacialCreator",
        menuName = "CharacterCreator/FacialCreator")]
    public class ScriptableFacialCreator : ScriptableObject
    {
        [SerializeReference]
        private FacialDataHolderReferencer _referencer = null;

        public FacialCreationStructure FacialData()
        {
            return _referencer.Reference.GetStructure();
        }

#if UNITY_EDITOR


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _headMidHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _headBottomHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _headBottomAngle = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyesSeparation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyesHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyesScale = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _irisWide = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _irisHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _irisPosition = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyesRotation = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeOpenTop = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeTop = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeTop_IN = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeTop_OUT = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeOpenBottom = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeBottom = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeBottom_IN = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeShapeBottom_OUT = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeWideness_IN = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeWideness_OUT = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeSlashScale = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeSlashScale_OUT = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeSlashPosition_OUT = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeSlashBottom = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeBrowHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeBrowSeparation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeBrowRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeBrownArc_IN = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _eyeBrownArc_OUT = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseBridgeScale = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseBridgeHeight = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseScale = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseShape = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseDepth = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _noseHeight = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _nosePoint = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _earSize = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _earPointRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _earRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _earPointSize = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _earPointiness = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekBoneSize = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekBoneRotationX = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekBoneRotationY = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekBoneRotationZ = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekSize = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekRotationX = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _cheekRotationY = 0;



        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _jawShapeX = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _jawShapeY = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _jawShapeZ = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _jawBackHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _jawBackRotation = 0;


        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _chinPointiness = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _chinRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _chinShapeX = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _chinShapeY = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _chinShapeZ = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipHeight = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipTopScale = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipTopRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipTopDepth = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipBottomScale = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipBottomRotation = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipBottomDepth = 0;

        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipShapeMid_Z = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipShapeOut_Z = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipShapeMid_Y = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipShapeOut_Y = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipWideness = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipCornerLength = 0;
        [Range(-2f, 2f), SerializeField, ShowIf("_runningTest")] private float _lipCornerShape = 0;


        private bool _runningTest = false;

        [Button("Test controller",ButtonSizes.Large), HideIf("_runningTest")]
        private void DoTest()
        {
            Timing.RunCoroutine(_DoTest());
            IEnumerator<float> _DoTest()
            {
                ResetValues();

                _runningTest = true;
                while (_referencer != null && _runningTest)
                {
                    Update();
                    yield return Timing.WaitForOneFrame;
                }
            }
        }
        [Button,ShowIf("_runningTest")]
        private void CancelTest()
        {
            ResetValues();
            _runningTest = false;
        }

        private void ResetValues()
        {
            _headMidHeight = 0;
            _headBottomHeight = 0;
            _headBottomAngle = 0;

            _eyesSeparation = 0;
            _eyesHeight = 0;
            _eyesScale = 0;

            _irisWide = 0;
            _irisHeight = 0;
            _irisPosition = 0;

            _eyesRotation = 0;

            _eyeShapeTop = 0;
            _eyeShapeTop_IN = 0;
            _eyeShapeTop_OUT = 0;
            _eyeShapeBottom = 0;

            _eyeShapeBottom_IN = 0;
            _eyeShapeBottom_OUT = 0;

            _eyeOpenBottom = 0;
            _eyeOpenTop = 0;

            _eyeWideness_IN = 0;
            _eyeWideness_OUT = 0;

            _eyeBrowHeight = 0;
            _eyeBrowSeparation = 0;
            _eyeBrowRotation = 0;
            _eyeBrownArc_IN = 0;
            _eyeBrownArc_OUT = 0;

            _eyeSlashScale = 0;
            _eyeSlashScale_OUT = 0;
            _eyeSlashPosition_OUT = 0;
            _eyeSlashBottom = 0;

            _noseBridgeScale = 0;
            _noseBridgeHeight = 0;
            _noseScale = 0;
            _noseShape = 0;
            _noseDepth = 0;
            _noseHeight = 0;
            _nosePoint = 0;

            _earSize = 0;
            _earRotation = 0;
            _earPointSize = 0;
            _earPointRotation = 0;
            _earPointiness = 0;

            _cheekBoneSize = 0;
            _cheekBoneRotationX = 0;
            _cheekBoneRotationY = 0;
            _cheekBoneRotationZ = 0;

            _cheekSize = 0;
            _cheekHeight = 0;
            _cheekRotationX = 0;
            _cheekRotationY = 0;

            _jawShapeX = 0;
            _jawShapeY = 0;
            _jawShapeZ = 0;
            _jawBackHeight = 0;
            _jawBackRotation = 0;

            _chinPointiness = 0;
            _chinRotation = 0;
            _chinShapeX = 0;
            _chinShapeY = 0;
            _chinShapeZ = 0;

            _lipHeight = 0;
            _lipTopScale = 0;
            _lipTopRotation = 0;
            _lipTopDepth = 0;

            _lipBottomScale = 0;
            _lipBottomRotation = 0;
            _lipBottomDepth = 0;

            _lipShapeMid_Z = 0;
            _lipShapeOut_Z = 0;

            _lipShapeMid_Y = 0;
            _lipShapeOut_Y = 0;

            _lipWideness = 0;
            _lipCornerLength = 0;

            _lipCornerShape = 0;
        }


        private void Update()
        {
            HeadMidHeight(_headMidHeight);
            HeadBottomHeight(_headBottomHeight);
            HeadBottomRotation(_headBottomAngle);

            EyesSeparation(_eyesSeparation);
            EyesHeight(_eyesHeight);
            EyesScale(_eyesScale);

            IrisWide(_irisWide);
            IrisHeight(_irisHeight);
            IrisPosition(_irisPosition);

            EyesRotation(_eyesRotation);

            EyeShapeTop(_eyeShapeTop);
            EyeShapeTop_IN(_eyeShapeTop_IN);
            EyeShapeTop_OUT(_eyeShapeTop_OUT);

            EyeShapeBottom(_eyeShapeBottom);
            EyeBottomShape_IN(_eyeShapeBottom_IN);
            EyeBottomShape_OUT(_eyeShapeBottom_OUT);

            EyeBottomOpenness(_eyeOpenBottom);
            EyeTopOpenness(_eyeOpenTop);

            EyeWideness_IN(_eyeWideness_IN);
            EyeWideness_OUT(_eyeWideness_OUT);

            EyeBrownHeight(_eyeBrowHeight);
            EyeBrownSeparation(_eyeBrowSeparation);
            EyeBrowRotation(_eyeBrowRotation);
            EyeBrownArc_IN(_eyeBrownArc_IN);
            EyeBrowArc_OUT(_eyeBrownArc_OUT);

            EyeMidSlashScale(_eyeSlashScale);
            EyeOutSlashScale(_eyeSlashScale_OUT);
            EyeOutSlashPosition(_eyeSlashPosition_OUT);
            EyeBottomSlashScale(_eyeSlashBottom);

            NoseBridgeScale(_noseBridgeScale);
            NoseBridgeHeight(_noseBridgeHeight);
            NoseScale(_noseScale);
            NoseShape(_noseShape);
            NoseDepth(_noseDepth);
            NoseHeight(_noseHeight);
            NoseTipWide(_nosePoint);

            EarScale(_earSize);
            EarPointRotation(_earRotation);
            EarHorizontalRotation(_earPointRotation);
            EarPointScale(_earPointSize);
            EarPointiness(_earPointiness);

            CheekBoneScale(_cheekBoneSize);
            CheekBoneRotationX(_cheekBoneRotationX);
            CheekBoneRotationY(_cheekBoneRotationY);
            CheekBoneRotationZ(_cheekBoneRotationZ);

            CheekSize(_cheekSize);
            CheekHeightPosition(_cheekHeight);
            CheekRotationX(_cheekRotationX);
            CheekRotationY(_cheekRotationY);

            JawShapeX(_jawShapeX);
            JawShapeY(_jawShapeY);
            JawShapeZ(_jawShapeZ);
            BackJawHeight(_jawBackHeight);
            BackJawRotation(_jawBackRotation);

            ChinPoint(_chinPointiness);
            ChinPointRotation(_chinRotation);
            ChinShapeX(_chinShapeX);
            ChinShapeY(_chinShapeY);
            ChinShapeZ(_chinShapeZ);

            LipHeight(_lipHeight);
            LipTopScale(_lipTopScale);
            LipTopRotation(_lipTopRotation);
            LipTopDepth(_lipTopDepth);

            LipBottomScale(_lipBottomScale);
            LipBottomRotation(_lipBottomRotation);
            LipBottomDepth(_lipBottomDepth);

            LipShapeMid_Z(_lipShapeMid_Z);
            LipShapeOut_Z(_lipShapeOut_Z);

            LipShapeMid_Y(_lipShapeMid_Y);
            LipShapeOut_Y(_lipShapeOut_Y);

            LipWideness(_lipWideness);
            LipCornerLength(_lipCornerLength);

            LipCornerShape(_lipCornerShape);
        }

        private void OnEnable()
        {
            CancelTest();
        }
        private void OnDisable()
        {
            CancelTest();
        }
#endif

        #region << GENERAL >>

        public void HeadMidHeight(float weight)
        {
            DoPositionY(
                FacialData().HeadMid, weight);
        }
        public void HeadBottomHeight(float weight)
        {
            DoPositionY(
                FacialData().HeadBottom, weight);
        }
        public void HeadBottomRotation(float weight)
        {
            DoRotationZ(
                FacialData().HeadBottom, weight);
        }
        #endregion

        #region << EYEBROW >>
        public void EyeBrownHeight(float weight)
        {
            DoPositionX(
                FacialData().LeftEyeBrow[0], weight);
            DoPositionX(
                FacialData().RightEyeBrow[0], weight);
        }

        public void EyeBrownSeparation(float weight)
        {
            DoPositionZ(
                FacialData().LeftEyeBrow[0], weight);
            DoPositionZ(
                FacialData().RightEyeBrow[0], -weight);

            DoPositionY(
                FacialData().LeftEyeBrow[0], weight * .5f);
            DoPositionY(
                FacialData().RightEyeBrow[0], weight * .5f);
        }

        public void EyeBrowRotation(float weight)
        {
            DoRotationY(
                FacialData().LeftEyeBrow[0], weight);
            DoRotationY(
                FacialData().RightEyeBrow[0], -weight);
        }

        public void EyeBrownArc_IN(float weight)
        {
            DoRotationZ(
                FacialData().LeftEyeBrow_IN(), weight * 2);
            DoRotationZ(
                FacialData().RightEyeBrow_IN(), -weight * 2);
        }

        public void EyeBrowArc_OUT(float weight)
        {
            DoRotationZ(
                FacialData().LeftEyeBrow_OUT(), weight * 2);
            DoRotationZ(
                FacialData().RightEyeBrow_OUT(), -weight * 2);
        }

        #endregion

        #region << EYES >>
        public void EyesScale(float weight)
        {
            DoScale(
                FacialData().LeftEyeRoot, weight * 8);
            DoScale(
                FacialData().RightEyeRoot, weight * 8);

            DoPositionY(
                FacialData().LeftEyeRoot, -weight * 1.5f);
            DoPositionY(
                FacialData().RightEyeRoot, -weight * 1.5f);
        }

        public void IrisWide(float weight)
        {
            DoScaleX(
                FacialData().LeftIris,weight);
            DoScaleX(
                FacialData().RightIris, weight);
        }
        public void IrisHeight(float weight)
        {
            DoScaleY(
                FacialData().LeftIris, weight);
            DoScaleY(
                FacialData().RightIris, weight);
        }
        public void IrisPosition(float weight)
        {
            DoPositionY(
                FacialData().LeftIris, weight);
            DoPositionY(
                FacialData().RightIris, weight);

            DoPositionZ(
                FacialData().LeftIris, weight *.3f);
            DoPositionZ(
                FacialData().RightIris, weight *.3f);
        }
        


        public void EyesSeparation(float weight)
        {
            DoPositionX(
                FacialData().LeftEyeRoot, weight);
            //-weight because mirror
            DoPositionX(
                FacialData().RightEyeRoot, -weight);
        }
        public float WeightEyeSeparation()
        {
            return WeightPositionX(FacialData().LeftEyeRoot);
        }

        public void EyesHeight(float weight)
        {
            DoPositionZ(
                FacialData().LeftEyeRoot, weight);
            DoPositionZ(
                FacialData().RightEyeRoot, weight);
        }

        public void EyesRotation(float weight)
        {
            DoRotationY(
                FacialData().LeftEyeRoot, weight);
            DoRotationY(
                FacialData().RightEyeRoot, -weight);
        }
        public float WeightEyeRotation()
        {
            return WeightRotationY(FacialData().LeftEyeRoot);
        }

        public void EyeShapeTop(float weight)
        {
            for (int i = 1; i < FacialData().LeftEyeTop.Length; i++)
            {
                DoRotationY(
                    FacialData().LeftEyeTop[i], weight);
                DoRotationY(
                    FacialData().RightEyeTop[i], -weight);
            }
        }
        public void EyeShapeTop_IN(float weight)
        {
            DoRotationX(
                FacialData().FirstLeftEyeTop_IN(), weight);
            DoRotationX(
                FacialData().SecondLeftEyeTop_IN(), -weight);
            DoPositionZ(
                FacialData().FirstLeftEyeTop_IN(), weight *.5f);
            DoPositionZ(
                FacialData().SecondLeftEyeTop_IN(), weight * .5f);

            DoRotationX(
                FacialData().FirstRightEyeTop_IN(), weight);
            DoRotationX(
                FacialData().SecondRightEyeTop_IN(), -weight);
            DoPositionZ(
                FacialData().FirstRightEyeTop_IN(), weight * .5f);
            DoPositionZ(
                FacialData().SecondRightEyeTop_IN(), weight * .5f);
        }
        public void EyeShapeTop_OUT(float weight)
        {
            DoRotationY(
                FacialData().FirstLeftEyeTop_OUT(), -weight);
            DoRotationY(
                FacialData().SecondLeftEyeTop_OUT(), .5f*weight);
            

            DoRotationY(
                FacialData().FirstRightEyeTop_OUT(), weight);
            DoRotationY(
                FacialData().SecondRightEyeTop_OUT(), -.5f * weight);
            
        }

        public void EyeShapeBottom(float weight)
        {
            for (int i = 1; i < FacialData().LeftEyeBottom.Length; i++)
            {
                DoRotationX(
                    FacialData().LeftEyeBottom[i], weight*2.5f);
                DoRotationX(
                    FacialData().RightEyeBottom[i], weight*2.5f);
               
            }
        }

        public void EyeBottomShape_IN(float weight)
        {
            DoRotationY(
                FacialData().FirstLeftEyeBottom_IN(), weight);
            DoRotationY(
                FacialData().FirstRightEyeBottom_IN(), -weight);
        }

        public void EyeBottomShape_OUT(float weight)
        {
            DoRotationY(
                FacialData().FirstLeftEyeBottom_OUT(), weight);
            DoRotationY(
                FacialData().FirstRightEyeBottom_OUT(), -weight);
        }
        

        public void EyeTopOpenness(float weight)
        {
            DoPositionZ(
                FacialData().LeftEyeTopRoot(), weight);
            DoPositionZ(
                FacialData().RightEyeTopRoot(), weight);
        }
        public void EyeBottomOpenness(float weight)
        {
            DoPositionZ(
                FacialData().LeftEyeBottomRoot(), weight);
            DoPositionZ(
                FacialData().RightEyeBottomRoot(), weight);
        }

        public void EyeWideness_IN(float weight)
        {
            DoPositionX(
                FacialData().FirstLeftEyeTop_IN(), -weight);
            DoPositionX(
                FacialData().FirstLeftEyeBottom_IN(), -weight);
            DoPositionX(
                FacialData().FirstRightEyeTop_IN(), weight);
            DoPositionX(
                FacialData().FirstRightEyeBottom_IN(), weight);
        }

        public void EyeWideness_OUT(float weight)
        {
            DoPositionX(
                FacialData().FirstLeftEyeBottom_OUT(), weight);
            DoPositionX(
                FacialData().FirstLeftEyeTop_OUT(), weight);
            DoPositionX(
                FacialData().FirstRightEyeBottom_OUT(), -weight);
            DoPositionX(
                FacialData().FirstRightEyeTop_OUT(), -weight);
        }

        public void EyeMidSlashScale(float weight)
        {
            DoScale(
                FacialData().FirstSlashLeftEyeTop(), weight*10);
            DoScale(
                FacialData().FirstSlashRightEyeTop(), weight*10);

            DoRotationZ(
                FacialData().FirstSlashLeftEyeTop(), weight);
            DoRotationZ(
                FacialData().FirstSlashRightEyeTop(), -weight);

            DoRotationY(
                FacialData().FirstSlashLeftEyeTop(), weight);
            DoRotationY(
                FacialData().FirstSlashRightEyeTop(), -weight);
        }




        public void EyeOutSlashScale(float weight)
        {
            DoScale(
                FacialData().SecondLeftEyeTop_OUT(), weight * 15);
            DoScale(
                FacialData().SecondRightEyeTop_OUT(), weight * 15);
        }

        public void EyeOutSlashPosition(float weight)
        {
            DoPositionY(
                FacialData().SecondLeftEyeTop_OUT(), weight);
            DoPositionY(
                FacialData().SecondRightEyeTop_OUT(), weight);

            DoPositionX(
                FacialData().SecondLeftEyeTop_OUT(), -weight);
            DoPositionX(
                FacialData().SecondRightEyeTop_OUT(), weight);
        }

        public void EyeBottomSlashScale(float weight)
        {
            DoScale(
                FacialData().LeftEyeSlashBottom(), weight * 10);
            DoScale(
                FacialData().RightEyeSlashBottom(), weight * 10);

        }

        #endregion

        #region << CHEEKBONE >>
        public void CheekBoneScale(float weight)
        {
            DoScale(
                FacialData().LeftMidHead, weight);
            DoPositionX(
                FacialData().LeftMidHead, weight);
            DoScale(
                FacialData().RightMidHead, weight);
            DoPositionX(
                FacialData().RightMidHead, -weight);
        }

        public void CheekBoneRotationX(float weight)
        {
            DoRotationX(
                FacialData().LeftMidHead, weight);
            DoRotationX(
                FacialData().RightMidHead, weight);
        }
        public void CheekBoneRotationY(float weight)
        {
            DoRotationY(
                FacialData().LeftMidHead, weight);
            DoRotationY(
                FacialData().RightMidHead, -weight);
        }
        public void CheekBoneRotationZ(float weight)
        {
            DoRotationZ(
                FacialData().LeftMidHead, weight);
            DoRotationZ(
                FacialData().RightMidHead, -weight);
        } 
        #endregion

        #region << EARS >>

        public void EarScale(float weight)
        {
            DoScale(
                FacialData().LeftEar[0], weight * 5);
            DoScale(
                FacialData().RightEar[0], weight * 5);
        }

        public void EarPointRotation(float weight)
        {
            DoRotationZ(
                FacialData().LeftEar[0], -weight);
            DoRotationZ(
                FacialData().RightEar[0], weight);
        }

        public void EarHorizontalRotation(float weight)
        {
            DoRotationX(
                FacialData().LeftEarPoint(), weight);
            DoRotationX(
                FacialData().RightEarPoint(), weight);
        }


        public void EarPointScale(float weight)
        {
            DoScaleY(
                FacialData().LeftEarPoint(), weight);
            DoScaleY(
                FacialData().RightEarPoint(), weight);
        }
        public void EarPointiness(float weight)
        {
            DoScaleX(
                FacialData().LeftEarPoint(), weight);
            DoScaleX(
                FacialData().RightEarPoint(), weight);
        } 
        #endregion

        #region << NOSE >>
        public void NoseBridgeScale(float weight)
        {
            DoScale(
                FacialData().NoseBridge(), weight);
            DoPositionY(
                FacialData().NoseBridge(), weight);
        }

        public void NoseBridgeHeight(float weight)
        {
            DoPositionZ(
                FacialData().NoseBridge(), weight);
        }

        public void NoseScale(float weight)
        {
            DoScale(
                FacialData().NoseFrontRoot(), weight*10);
        }

        public void NoseShape(float weight)
        {
            DoRotationX(
                FacialData().NoseFrontRoot(), weight);
            DoRotationX(
                FacialData().NoseTip(), weight);
        }

        public void NoseDepth(float weight)
        {
            DoPositionY(
                FacialData().NoseFrontRoot(), weight);
        }
        public void NoseHeight(float weight)
        {
            DoPositionZ(
                FacialData().NoseFrontRoot(), weight);
        }

        public void NoseTipWide(float weight)
        {
            FacialData().NoseTip().ScaleWeight = weight * 15;

            Vector3 scale = FacialData().NoseTip().Bone.localScale;
            scale.x = Mathf.LerpUnclamped(
                1,
                FacialData().NoseTip().MaxScale.x,
                FacialData().NoseTip().ScaleWeight);
            FacialData().NoseTip().Bone.localScale = scale;
        }
        #endregion

        #region << CHEEKS >>
        public void CheekSize(float weight)
        {
            DoScale(
                FacialData().LeftCheek, weight * 10);
            DoRotationZ(
                FacialData().LeftCheek, -weight);
            DoPositionY(
                FacialData().LeftCheek, weight);

            DoScale(
                FacialData().RightCheek, weight * 10);
            DoRotationZ(
                FacialData().RightCheek, weight);
            DoPositionY(
                FacialData().RightCheek, weight);
        }

        public void CheekHeightPosition(float weight)
        {
            DoPositionX(
                FacialData().LeftCheek, weight);
            DoPositionX(
                FacialData().RightCheek, weight);
        }

        public void CheekRotationY(float weight)
        {
            DoRotationY(
                FacialData().LeftCheek, weight);
            DoRotationY(
                FacialData().RightCheek, -weight);
        }

        public void CheekRotationX(float weight)
        {
            DoRotationX(
                FacialData().LeftCheek, weight);
            DoRotationX(
                FacialData().RightCheek, weight);
        }
        #endregion

        #region << JAW & CHIN >>
        public void JawShapeX(float weight)
        {
            DoRotationX(
                FacialData().LeftJaw, weight * 2);
            DoRotationX(
                FacialData().RightJaw, weight * 2);
        }
        public void JawShapeY(float weight)
        {
            DoRotationY(
                FacialData().LeftJaw, weight * 2);
            DoRotationY(
                FacialData().RightJaw, -weight * 2);
        }
        public void JawShapeZ(float weight)
        {
            DoRotationZ(
                FacialData().LeftJaw, weight * 2);
            DoRotationZ(
                FacialData().RightJaw, -weight * 2);
        }

        public void BackJawHeight(float weight)
        {
            DoPositionX(
                FacialData().LeftBackJaw, weight);
            DoPositionX(
                FacialData().RightBackJaw, weight);
        }
        public void BackJawRotation(float weight)
        {
            DoRotationY(
                FacialData().LeftBackJaw, weight);
            DoRotationY(
                FacialData().RightBackJaw, -weight);
        }

        public void ChinPoint(float weight)
        {
            DoPositionX(
                FacialData().Chin[0], weight * 5);
        }

        public void ChinPointRotation(float weight)
        {
            DoRotationZ(
                FacialData().Chin[0], weight * 3);
            DoRotationZ(
                FacialData().Chin[FacialData().Chin.Length - 1], -weight * 3);
        }
        public void ChinShapeX(float weight)
        {
            DoPositionX(
                FacialData().LeftJaw, weight);
            DoPositionX(
                FacialData().RightJaw, weight);
        }
        public void ChinShapeY(float weight)
        {
            DoPositionY(
                FacialData().LeftJaw, weight);
            DoPositionY(
                FacialData().RightJaw, weight);
        }
        public void ChinShapeZ(float weight)
        {
            DoPositionZ(
                FacialData().LeftJaw, weight);
            DoPositionZ(
                FacialData().RightJaw, -weight);
        } 
        #endregion

        public void LipHeight(float weight)
        {
            DoPositionX(
                FacialData().LipTop, weight);
            DoPositionX(
                FacialData().LipBottom, weight);
        }

        public void LipTopScale(float weight)
        {
            DoScale(
                FacialData().LipTop, weight * 5);
        }

        public void LipTopRotation(float weight)
        {
            DoRotationZ(
                FacialData().LipTop, weight);
        }

       
        public void LipTopDepth(float weight)
        {
            DoPositionY(
                FacialData().LipTop, weight);
        }

        public void LipBottomScale(float weight)
        {
            DoScale(
                FacialData().LipBottom, weight * 5);
        }

        public void LipBottomRotation(float weight)
        {
            DoRotationZ(
                FacialData().LipBottom, weight);
        }
        

        public void LipBottomDepth(float weight)
        {
            DoPositionY(
                FacialData().LipBottom, weight);
        }

        public void LipShapeMid_Z(float weight)
        {
            DoRotationZ(
                FacialData().LeftLipTop[0], -weight);
            DoRotationZ(
                FacialData().RightLipTop[0], weight);
            DoRotationZ(
                FacialData().LeftLipBottom[0], -weight);
            DoRotationZ(
                FacialData().RightLipBottom[0], weight);

        }
        public void LipShapeOut_Z(float weight)
        {
            DoRotationZ(
                FacialData().LeftLipTop[1],-weight);
            DoRotationZ(
                FacialData().RightLipTop[1], weight);
            DoRotationZ(
                FacialData().LeftLipBottom[1], -weight);
            DoRotationZ(
                FacialData().RightLipBottom[1], weight);
        }
        public void LipShapeMid_Y(float weight)
        {
            DoRotationY(
                FacialData().LeftLipTop[0], -weight);
            DoRotationY(
                FacialData().RightLipTop[0], weight);
            DoRotationY(
                FacialData().LeftLipBottom[0], -weight);
            DoRotationY(
                FacialData().RightLipBottom[0], weight);

        }
        public void LipShapeOut_Y(float weight)
        {
            DoRotationY(
                FacialData().LeftLipTop[1], -weight);
            DoRotationY(
                FacialData().RightLipTop[1], weight);
            DoRotationY(
                FacialData().LeftLipBottom[1], weight);
            DoRotationY(
                FacialData().RightLipBottom[1], -weight);
        }

        public void LipWideness(float weight)
        {
            DoPositionY(
                FacialData().GetLeftLipTopLast(), weight);
            DoPositionY(
                FacialData().GetRightLipTopLast(), weight);

            DoPositionY(
                FacialData().GetLeftLipBottomLast(), weight);
            DoPositionY(
                FacialData().GetRightLipBottomLast(), weight);
        }

        public void LipCornerShape(float weight)
        {
            DoPositionX(
                FacialData().LeftLipCorner, weight);
            DoPositionX(
                FacialData().RightLipCorner, weight);

           
        }
        public void LipCornerLength(float weight)
        {
            DoPositionZ(
                FacialData().LeftLipCorner, -weight);
            DoPositionZ(
                FacialData().RightLipCorner, weight);
        }

        

        


        #region << CALCULATIONS >>
        private static void DoPositionX(FacialCreationData data, float weight)
        {
            data.PositionWeights.x = weight;
            Vector3 target = data.Bone.localPosition;
            target.x = Mathf.LerpUnclamped(
                data.LocalPosition.x,
                data.MaxLocalPosition.x,
                weight);

            data.Bone.localPosition = target;
        }
        private static float WeightPositionX(FacialCreationData data)
        {
            return data.PositionWeights.x;
        }

        private static void DoPositionY(FacialCreationData data, float weight)
        {
            data.PositionWeights.y = weight;
            Vector3 target = data.Bone.localPosition;
            target.y = Mathf.LerpUnclamped(
                data.LocalPosition.y,
                data.MaxLocalPosition.y,
                weight);

            data.Bone.localPosition = target;
        }
        private static float WeightPositionY(FacialCreationData data)
        {
            return data.PositionWeights.y;
        }

        private static void DoPositionZ(FacialCreationData data, float weight)
        {
            data.PositionWeights.z = weight;
            Vector3 target = data.Bone.localPosition;
            target.z = Mathf.LerpUnclamped(
                data.LocalPosition.z,
                data.MaxLocalPosition.z,
                weight);

            data.Bone.localPosition = target;
        }
        private static float WeightPositionZ(FacialCreationData data)
        {
            return data.PositionWeights.z;
        }

        private static void DoRotationX(FacialCreationData data, float weight)
        {
            data.RotationWeights.x = weight;
            Vector3 target = data.Bone.localRotation.eulerAngles;
            target.x = Mathf.LerpUnclamped(
                data.LocalRotation.x,
                data.MaxLocalRotation.x,
                weight);

            data.Bone.localRotation = Quaternion.Euler(target);
        }
        private static float WeightRotationX(FacialCreationData data)
        {
            return data.RotationWeights.x;
        }

        private static void DoRotationY(FacialCreationData data, float weight)
        {
            data.RotationWeights.y = weight;
            Vector3 target = data.Bone.localRotation.eulerAngles;
            target.y = Mathf.LerpUnclamped(
                data.LocalRotation.y,
                data.MaxLocalRotation.y,
                weight);

            data.Bone.localRotation = Quaternion.Euler(target);
        }
        private static float WeightRotationY(FacialCreationData data)
        {
            return data.RotationWeights.y;
        }
        private static void DoRotationZ(FacialCreationData data, float weight)
        {
            data.RotationWeights.z = weight;
            Vector3 target = data.Bone.localRotation.eulerAngles;
            target.z = Mathf.LerpUnclamped(
                data.LocalRotation.z,
                data.MaxLocalRotation.z,
                weight);

            data.Bone.localRotation = Quaternion.Euler(target);
        }
        private static float WeightRotationZ(FacialCreationData data)
        {
            return data.RotationWeights.z;
        }

        private static void DoScale(FacialCreationData data, float weight)
        {
            data.ScaleWeight = weight;
            Vector3 minSize = Vector3.one;
            minSize.x += data.XScaleModifier;
            minSize.y += data.YScaleModifier;
            data.Bone.localScale = Vector3.LerpUnclamped(minSize, data.MaxScale, weight);
        }
        private static void DoScaleX(FacialCreationData data, float xModifier)
        {
            DoScaleX(data,data.ScaleWeight,xModifier);
        }
        private static void DoScaleY(FacialCreationData data, float yModifier)
        {
            DoScaleY(data, data.ScaleWeight, yModifier);
        }
        private static void DoScaleX(FacialCreationData data, float weight, float xModifier)
        {
            data.XScaleModifier = xModifier;
            DoScale(data,weight);
        }
        private static void DoScaleY(FacialCreationData data, float weight, float yModifier)
        {
            data.YScaleModifier = yModifier;
            DoScale(data, weight);
        }

        private static float WeightScaleX(FacialCreationData data)
        {
            return data.ScaleWeight;
        }
        #endregion
    }



}
