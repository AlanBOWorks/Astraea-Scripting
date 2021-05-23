using System;
using System.Collections.Generic;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    public class FacialStructure<T> : HumanStructureBase<T>
    {
        
        [TabGroup("Holders")]
        public T HeadTop;
        [TabGroup("Holders")]
        public T HeadMid;
        [TabGroup("Holders")]
        public T HeadBottom;


        [TabGroup("General"), Title("Left")]
        public T[] LeftEar;

        [TabGroup("General")]
        public T LeftMidHead;
        [TabGroup("General")]
        public T LeftCheek;

        [TabGroup("EyeBrow"), Title("Left")]
        [InfoBox("Post[0]: Root; Pos [1]: first IN; Post[2]: first Out",InfoMessageType.Warning)]
        public T[] LeftEyeBrow;


        [TabGroup("Eyes"), Title("Left")]
        public T LeftEyeRoot;
        [TabGroup("Eyes")]
        public T LeftIris;
        [TabGroup("Eyes")]
        [InfoBox("Post[0]: Root; Pos [1]: first IN; Post[2]: first Out; Pos[3]: second IN; Pos[4]: second Out; Pos[Last]: Slash",InfoMessageType.Warning)]
        public T[] LeftEyeTop;
        [TabGroup("Eyes")]
        [InfoBox("Post[0]: Root; Pos [1]: first IN; Post[2]: first Out; Pos[Last]: Slash",InfoMessageType.Warning)]
        public T[] LeftEyeBottom;


        [TabGroup("Lip"), Title("Left")]
        public T[] LeftLipTop;
        [TabGroup("Lip")]
        public T LeftLipCorner;
        [TabGroup("Lip")]
        public T[] LeftLipBottom;

       

        [TabGroup("General"), Title("Right")]
        public T[] RightEar;

        [TabGroup("General")]
        public T RightMidHead;
        [TabGroup("General")]
        public T RightCheek;

        [TabGroup("EyeBrow"), Title("Right")]
        public T[] RightEyeBrow;


        [TabGroup("Eyes"), Title("Right")]
        public T RightEyeRoot;
        [TabGroup("Eyes")]
        public T RightIris;
        [TabGroup("Eyes")]
        public T[] RightEyeTop;
        [TabGroup("Eyes")]
        public T[] RightEyeBottom;


        [TabGroup("Lip"), Title("Right")]
        public T[] RightLipTop;
        [TabGroup("Lip")]
        public T RightLipCorner;
        [TabGroup("Lip")]
        public T[] RightLipBottom;

        



        [TabGroup("Lip")]
        public T LipTop;
        [TabGroup("Lip")]
        public T LipBottom;


        [TabGroup("Jaw")]
        public T Jaw;
        [TabGroup("Jaw")]
        public T LeftJaw;
        [TabGroup("Jaw")]
        public T RightJaw;


        [TabGroup("Jaw")] 
        public T LeftBackJaw;
        [TabGroup("Jaw")] 
        public T RightBackJaw;


        [TabGroup("Extras")]
        [InfoBox("Post[0]: Bridge; Pos [1]: FrontRoot; Post[Last]: Tip",InfoMessageType.Warning)]
        public T[] Nose;

        [TabGroup("Extras")]
        public T[] Tounge;
        [TabGroup("Extras")]
        public T[] Chin;

        #region << GETS EYES >>

        public T LeftEyeBrowRoot()
        {
            return LeftEyeBrow[0];
        }
        public T LeftEyeBrow_IN()
        {
            return LeftEyeBrow[1];
        }
        public T LeftEyeBrow_OUT()
        {
            return LeftEyeBrow[2];
        }

        public T LeftEyeTopRoot()
        {
            return LeftEyeTop[0];
        }
        public T FirstLeftEyeTop_IN()
        {
            return LeftEyeTop[1];
        }
        public T FirstLeftEyeTop_OUT()
        {
            return LeftEyeTop[2];
        }
        public T SecondLeftEyeTop_IN()
        {
            return LeftEyeTop[3];
        }
        public T SecondLeftEyeTop_OUT()
        {
            return LeftEyeTop[4];
        }
        public T FirstSlashLeftEyeTop()
        {
            return LeftEyeTop[LeftEyeTop.Length - 1];
        }

        public T LeftEyeBottomRoot()
        {
            return LeftEyeBottom[0];
        }
        public T FirstLeftEyeBottom_IN()
        {
            return LeftEyeBottom[1];
        }
        public T FirstLeftEyeBottom_OUT()
        {
            return LeftEyeBottom[2];
        }

        public T LeftEyeSlashBottom()
        {
            return LeftEyeBottom[LeftEyeBottom.Length - 1];
        }


        public T RightEyeBrowRoot()
        {
            return RightEyeBrow[0];
        }
        public T RightEyeBrow_IN()
        {
            return RightEyeBrow[1];
        }
        public T RightEyeBrow_OUT()
        {
            return RightEyeBrow[2];
        }

        public T RightEyeTopRoot()
        {
            return RightEyeTop[0];
        }
        public T FirstRightEyeTop_IN()
        {
            return RightEyeTop[1];
        }
        public T FirstRightEyeTop_OUT()
        {
            return RightEyeTop[2];
        }
        public T SecondRightEyeTop_IN()
        {
            return RightEyeTop[3];
        }
        public T SecondRightEyeTop_OUT()
        {
            return RightEyeTop[4];
        }
        public T FirstSlashRightEyeTop()
        {
            return RightEyeTop[RightEyeTop.Length - 1];
        }

        public T RightEyeBottomRoot()
        {
            return RightEyeBottom[0];
        }
        public T FirstRightEyeBottom_IN()
        {
            return RightEyeBottom[1];
        }
        public T FirstRightEyeBottom_OUT()
        {
            return RightEyeBottom[2];
        }
        public T RightEyeSlashBottom()
        {
            return RightEyeBottom[RightEyeBottom.Length - 1];
        }
        #endregion

        #region << NOSE GETS >>
        public T NoseBridge()
        {
            return Nose[0];
        }
        public T NoseFrontRoot()
        {
            return Nose[1];
        }
        public T NoseTip()
        {
            return Nose[Nose.Length - 1];
        }
        #endregion

        #region << GETS EAR >>
        public T LeftEarPoint()
        {
            return LeftEar[LeftEar.Length - 1];
        }
        public T RightEarPoint()
        {
            return RightEar[RightEar.Length - 1];
        }
        #endregion

        #region << GETS LIPS >>

        public T GetLeftLipTopLast()
        {
            return LeftLipTop[LeftLipTop.Length - 1];
        }
        public T GetLeftLipBottomLast()
        {
            return LeftLipBottom[LeftLipBottom.Length - 1];
        }

        public T GetRightLipTopLast()
        {
            return RightLipTop[RightLipTop.Length - 1];
        }
        public T GetRightLipBottomLast()
        {
            return RightLipBottom[RightLipBottom.Length - 1];
        }

        #endregion

        public FacialStructure() : base() { }
        public FacialStructure(T uniformValue) : base(uniformValue) { }

        public FacialStructure(FacialStructure<T> copyFrom)
        {
            InjectWithInitialization(copyFrom);
        }

        public delegate T ParseAction(T parsing);
        public FacialStructure(FacialStructure<T> copyFrom, ParseAction parseAction)
        {
            InjectWithInitialization(copyFrom,parseAction);
        }

        public override T[] GenerateArray()
        {
            return GenerateList().ToArray();
        }

        public List<T> GenerateList()
        {
            List<T> generated = new List<T>
            {
                HeadTop,
                HeadMid,
                HeadBottom,
                LeftMidHead,
                LeftCheek,
                LeftEyeRoot,
                LeftIris,
                RightMidHead,
                RightCheek,
                RightEyeRoot,
                RightIris,
                LipTop,
                LipBottom,
                Jaw,
                LeftJaw,
                RightJaw,
                LeftBackJaw,
                RightBackJaw
            };


            generated.AddRange(LeftEar);
            generated.AddRange(LeftEyeBrow);
            generated.AddRange(LeftEyeTop);
            generated.AddRange(LeftEyeBottom);
            generated.AddRange(LeftLipTop);
            generated.AddRange(LeftLipBottom);
            generated.Add(LeftLipCorner);

            generated.AddRange(RightEar);
            generated.AddRange(RightEyeBrow);
            generated.AddRange(RightEyeTop);
            generated.AddRange(RightEyeBottom);
            generated.AddRange(RightLipTop);
            generated.AddRange(RightLipBottom);
            generated.Add(RightLipCorner);

            generated.AddRange(Nose);
            generated.AddRange(Tounge);
            generated.AddRange(Chin);

            return generated;
        }

        public void Inject(FacialStructure<T> copyFrom)
        {
            HeadTop = copyFrom.HeadTop;
            HeadMid = copyFrom.HeadMid;
            HeadBottom = copyFrom.HeadBottom;


            LeftMidHead = copyFrom.LeftMidHead;
            LeftCheek = copyFrom.LeftCheek;

            LeftEyeRoot = copyFrom.LeftEyeRoot;
            LeftIris = copyFrom.LeftIris;


            RightMidHead = copyFrom.RightMidHead;
            RightCheek = copyFrom.RightCheek;

            RightEyeRoot = copyFrom.RightEyeRoot;
            RightIris = copyFrom.RightIris;

            LeftLipCorner = copyFrom.LeftLipCorner;
            RightLipCorner = copyFrom.RightLipCorner;

            LipTop = copyFrom.LipTop;
            LipBottom = copyFrom.LipBottom;


            Jaw = copyFrom.Jaw;
            LeftJaw = copyFrom.LeftJaw;
            RightJaw = copyFrom.RightJaw;
            LeftBackJaw = copyFrom.LeftBackJaw;
            RightBackJaw = copyFrom.RightBackJaw;

            int i;
            for (i = 0; i < LeftEar.Length; i++)
            {
                LeftEar[i] = copyFrom.LeftEar[i];
            }

            for (i = 0; i < LeftEyeBrow.Length; i++)
            {
                LeftEyeBrow[i] = copyFrom.LeftEyeBrow[i];
            }
            for (i = 0; i < LeftEyeTop.Length; i++)
            {
                LeftEyeTop[i] = copyFrom.LeftEyeTop[i];
            }
            for (i = 0; i < LeftEyeBottom.Length; i++)
            {
                LeftEyeBottom[i] = copyFrom.LeftEyeBottom[i];
            }

            for (i = 0; i < LeftLipTop.Length; i++)
            {
                LeftLipTop[i] = copyFrom.LeftLipTop[i];
            }
            for (i = 0; i < LeftLipBottom.Length; i++)
            {
                LeftLipBottom[i] = copyFrom.LeftLipBottom[i];
            }

            for (i = 0; i < RightEar.Length; i++)
            {
                RightEar[i] = copyFrom.RightEar[i];
            }

            for (i = 0; i < RightEyeBrow.Length; i++)
            {
                RightEyeBrow[i] = copyFrom.RightEyeBrow[i];
            }
            for (i = 0; i < RightEyeTop.Length; i++)
            {
                RightEyeTop[i] = copyFrom.RightEyeTop[i];
            }
            for (i = 0; i < RightEyeBottom.Length; i++)
            {
                RightEyeBottom[i] = copyFrom.RightEyeBottom[i];
            }

            for (i = 0; i < RightLipTop.Length; i++)
            {
                RightLipTop[i] = copyFrom.RightLipTop[i];
            }
            for (i = 0; i < RightLipBottom.Length; i++)
            {
                RightLipBottom[i] = copyFrom.RightLipBottom[i];
            }

            for (i = 0; i < Nose.Length; i++)
            {
                Nose[i] = copyFrom.Nose[i];
            }
            for (i = 0; i < Tounge.Length; i++)
            {
                Tounge[i] = copyFrom.Tounge[i];
            }
            for (i = 0; i < Chin.Length; i++)
            {
                Chin[i] = copyFrom.Chin[i];
            }
        }
        public void Inject(FacialStructure<T> parsingFacial, ParseAction parseFunc)
        {
            HeadTop = parseFunc(parsingFacial.HeadTop);
            HeadMid = parseFunc(parsingFacial.HeadMid);
            HeadBottom = parseFunc(parsingFacial.HeadBottom);


            LeftMidHead = parseFunc(parsingFacial.LeftMidHead);
            LeftCheek = parseFunc(parsingFacial.LeftCheek);

            LeftEyeRoot = parseFunc(parsingFacial.LeftEyeRoot);
            LeftIris = parseFunc(parsingFacial.LeftIris);


            RightMidHead = parseFunc(parsingFacial.RightMidHead);
            RightCheek = parseFunc(parsingFacial.RightCheek);

            RightEyeRoot = parseFunc(parsingFacial.RightEyeRoot);
            RightIris = parseFunc(parsingFacial.RightIris);

            LeftLipCorner = parseFunc(parsingFacial.LeftLipCorner);
            RightLipCorner = parseFunc(parsingFacial.RightLipCorner);

            LipTop = parseFunc(parsingFacial.LipTop);
            LipBottom = parseFunc(parsingFacial.LipBottom);


            Jaw = parseFunc(parsingFacial.Jaw);
            LeftJaw = parseFunc(parsingFacial.LeftJaw);
            RightJaw = parseFunc(parsingFacial.RightJaw);
            LeftBackJaw = parseFunc(parsingFacial.LeftBackJaw);
            RightBackJaw = parseFunc(parsingFacial.RightBackJaw);

            int i;
            for (i = 0; i < LeftEar.Length; i++)
            {
                LeftEar[i] = parseFunc(parsingFacial.LeftEar[i]);
            }

            for (i = 0; i < LeftEyeBrow.Length; i++)
            {
                LeftEyeBrow[i] = parseFunc(parsingFacial.LeftEyeBrow[i]);
            }
            for (i = 0; i < LeftEyeTop.Length; i++)
            {
                LeftEyeTop[i] = parseFunc(parsingFacial.LeftEyeTop[i]);
            }
            for (i = 0; i < LeftEyeBottom.Length; i++)
            {
                LeftEyeBottom[i] = parseFunc(parsingFacial.LeftEyeBottom[i]);
            }

            for (i = 0; i < LeftLipTop.Length; i++)
            {
                LeftLipTop[i] = parseFunc(parsingFacial.LeftLipTop[i]);
            }
            for (i = 0; i < LeftLipBottom.Length; i++)
            {
                LeftLipBottom[i] = parseFunc(parsingFacial.LeftLipBottom[i]);
            }

            for (i = 0; i < RightEar.Length; i++)
            {
                RightEar[i] = parseFunc(parsingFacial.RightEar[i]);
            }

            for (i = 0; i < RightEyeBrow.Length; i++)
            {
                RightEyeBrow[i] = parseFunc(parsingFacial.RightEyeBrow[i]);
            }
            for (i = 0; i < RightEyeTop.Length; i++)
            {
                RightEyeTop[i] = parseFunc(parsingFacial.RightEyeTop[i]);
            }
            for (i = 0; i < RightEyeBottom.Length; i++)
            {
                RightEyeBottom[i] = parseFunc(parsingFacial.RightEyeBottom[i]);
            }

            for (i = 0; i < RightLipTop.Length; i++)
            {
                RightLipTop[i] = parseFunc(parsingFacial.RightLipTop[i]);
            }
            for (i = 0; i < RightLipBottom.Length; i++)
            {
                RightLipBottom[i] = parseFunc(parsingFacial.RightLipBottom[i]);
            }

            for (i = 0; i < Nose.Length; i++)
            {
                Nose[i] = parseFunc(parsingFacial.Nose[i]);
            }
            for (i = 0; i < Tounge.Length; i++)
            {
                Tounge[i] = parseFunc(parsingFacial.Tounge[i]);
            }
            for (i = 0; i < Chin.Length; i++)
            {
                Chin[i] = parseFunc(parsingFacial.Chin[i]);
            }
        }

        private void InjectWithInitialization(FacialStructure<T> copyFrom)
        {
            InitializeArrays(copyFrom);
            Inject(copyFrom);
        }

        private void InjectWithInitialization(FacialStructure<T> copyFrom, ParseAction parseFunc)
        {
            InitializeArrays(copyFrom);
            Inject(copyFrom,parseFunc);
        }

        public delegate void ParseAction<in TParse>(T selfParam, TParse parseParam);

        public void DoParse<TParse>(FacialStructure<TParse> parsingFacial, ParseAction<TParse> action)
        {
            action(HeadTop, parsingFacial.HeadTop);
            action(HeadMid, parsingFacial.HeadMid);
            action(HeadBottom, parsingFacial.HeadBottom);


            action(LeftMidHead, parsingFacial.LeftMidHead);
            action(LeftCheek, parsingFacial.LeftCheek);

            action(LeftEyeRoot, parsingFacial.LeftEyeRoot);
            action(LeftIris, parsingFacial.LeftIris);


            action(RightMidHead, parsingFacial.RightMidHead);
            action(RightCheek, parsingFacial.RightCheek);

            action(RightEyeRoot, parsingFacial.RightEyeRoot);
            action(RightIris, parsingFacial.RightIris);

            action(LeftLipCorner, parsingFacial.LeftLipCorner);
            action(RightLipCorner, parsingFacial.RightLipCorner);

            action(LipTop, parsingFacial.LipTop);
            action(LipBottom, parsingFacial.LipBottom);


            action(Jaw, parsingFacial.Jaw);
            action(LeftJaw, parsingFacial.LeftJaw);
            action(RightJaw, parsingFacial.RightJaw);
            action(LeftBackJaw, parsingFacial.LeftBackJaw);
            action(RightBackJaw, parsingFacial.RightBackJaw);


            int i;
            for (i = 0; i < LeftEar.Length; i++)
            {
                action(LeftEar[i], parsingFacial.LeftEar[i]);
            }

            for (i = 0; i < LeftEyeBrow.Length; i++)
            {
                action(LeftEyeBrow[i], parsingFacial.LeftEyeBrow[i]);
            }
            for (i = 0; i < LeftEyeTop.Length; i++)
            {
                action(LeftEyeTop[i], parsingFacial.LeftEyeTop[i]);
            }
            for (i = 0; i < LeftEyeBottom.Length; i++)
            {
                action(LeftEyeBottom[i], parsingFacial.LeftEyeBottom[i]);
            }

            for (i = 0; i < LeftLipTop.Length; i++)
            {
                action(LeftLipTop[i], parsingFacial.LeftLipTop[i]);
            }
            for (i = 0; i < LeftLipBottom.Length; i++)
            {
                action(LeftLipBottom[i], parsingFacial.LeftLipBottom[i]);
            }

            for (i = 0; i < RightEar.Length; i++)
            {
                action(RightEar[i], parsingFacial.RightEar[i]);
            }

            for (i = 0; i < RightEyeBrow.Length; i++)
            {
                action(RightEyeBrow[i], parsingFacial.RightEyeBrow[i]);
            }
            for (i = 0; i < RightEyeTop.Length; i++)
            {
                action(RightEyeTop[i], parsingFacial.RightEyeTop[i]);
            }
            for (i = 0; i < RightEyeBottom.Length; i++)
            {
                action(RightEyeBottom[i], parsingFacial.RightEyeBottom[i]);
            }

            for (i = 0; i < RightLipTop.Length; i++)
            {
                action(RightLipTop[i], parsingFacial.RightLipTop[i]);
            }
            for (i = 0; i < RightLipBottom.Length; i++)
            {
                action(RightLipBottom[i], parsingFacial.RightLipBottom[i]);
            }

            for (i = 0; i < Nose.Length; i++)
            {
                action(Nose[i], parsingFacial.Nose[i]);
            }
            for (i = 0; i < Tounge.Length; i++)
            {
                action(Tounge[i], parsingFacial.Tounge[i]);
            }
            for (i = 0; i < Chin.Length; i++)
            {
                action(Chin[i], parsingFacial.Chin[i]);
            }
        }

        public void InitializeWithParse<TParse>(FacialStructure<TParse> parsingFacial, HumanFunc<TParse> parseFunc)
        {
            InitializeArrays(parsingFacial);
            InjectParse(parsingFacial, parseFunc);
        }
        private void InjectParse<TParse>(FacialStructure<TParse> parsingFacial, HumanFunc<TParse> parseFunc)
        {
            HeadTop = parseFunc(parsingFacial.HeadTop);
            HeadMid = parseFunc(parsingFacial.HeadMid);
            HeadBottom = parseFunc(parsingFacial.HeadBottom);


            LeftMidHead = parseFunc(parsingFacial.LeftMidHead);
            LeftCheek = parseFunc(parsingFacial.LeftCheek);

            LeftEyeRoot = parseFunc(parsingFacial.LeftEyeRoot);
            LeftIris = parseFunc(parsingFacial.LeftIris);


            RightMidHead = parseFunc(parsingFacial.RightMidHead);
            RightCheek = parseFunc(parsingFacial.RightCheek);

            RightEyeRoot = parseFunc(parsingFacial.RightEyeRoot);
            RightIris = parseFunc(parsingFacial.RightIris);

            LeftLipCorner = parseFunc(parsingFacial.LeftLipCorner);
            RightLipCorner = parseFunc(parsingFacial.RightLipCorner);

            LipTop = parseFunc(parsingFacial.LipTop);
            LipBottom = parseFunc(parsingFacial.LipBottom);


            Jaw = parseFunc(parsingFacial.Jaw);
            LeftJaw = parseFunc(parsingFacial.LeftJaw);
            RightJaw = parseFunc(parsingFacial.RightJaw);
            LeftBackJaw = parseFunc(parsingFacial.LeftBackJaw);
            RightBackJaw = parseFunc(parsingFacial.RightBackJaw);

            int i;
            for (i = 0; i < LeftEar.Length; i++)
            {
                LeftEar[i] = parseFunc(parsingFacial.LeftEar[i]);
            }

            for (i = 0; i < LeftEyeBrow.Length; i++)
            {
                LeftEyeBrow[i] = parseFunc(parsingFacial.LeftEyeBrow[i]);
            }
            for (i = 0; i < LeftEyeTop.Length; i++)
            {
                LeftEyeTop[i] = parseFunc(parsingFacial.LeftEyeTop[i]);
            }
            for (i = 0; i < LeftEyeBottom.Length; i++)
            {
                LeftEyeBottom[i] = parseFunc(parsingFacial.LeftEyeBottom[i]);
            }

            for (i = 0; i < LeftLipTop.Length; i++)
            {
                LeftLipTop[i] = parseFunc(parsingFacial.LeftLipTop[i]);
            }
            for (i = 0; i < LeftLipBottom.Length; i++)
            {
                LeftLipBottom[i] = parseFunc(parsingFacial.LeftLipBottom[i]);
            }

            for (i = 0; i < RightEar.Length; i++)
            {
                RightEar[i] = parseFunc(parsingFacial.RightEar[i]);
            }

            for (i = 0; i < RightEyeBrow.Length; i++)
            {
                RightEyeBrow[i] = parseFunc(parsingFacial.RightEyeBrow[i]);
            }
            for (i = 0; i < RightEyeTop.Length; i++)
            {
                RightEyeTop[i] = parseFunc(parsingFacial.RightEyeTop[i]);
            }
            for (i = 0; i < RightEyeBottom.Length; i++)
            {
                RightEyeBottom[i] = parseFunc(parsingFacial.RightEyeBottom[i]);
            }

            for (i = 0; i < RightLipTop.Length; i++)
            {
                RightLipTop[i] = parseFunc(parsingFacial.RightLipTop[i]);
            }
            for (i = 0; i < RightLipBottom.Length; i++)
            {
                RightLipBottom[i] = parseFunc(parsingFacial.RightLipBottom[i]);
            }

            for (i = 0; i < Nose.Length; i++)
            {
                Nose[i] = parseFunc(parsingFacial.Nose[i]);
            }
            for (i = 0; i < Tounge.Length; i++)
            {
                Tounge[i] = parseFunc(parsingFacial.Tounge[i]);
            }
            for (i = 0; i < Chin.Length; i++)
            {
                Chin[i] = parseFunc(parsingFacial.Chin[i]);
            }
        }
        public void InitializeArrays<TParse>(FacialStructure<TParse> parsingFacial)
        {
            LeftEar = new T[parsingFacial.LeftEar.Length];

            LeftEyeBrow = new T[parsingFacial.LeftEyeBrow.Length];
            LeftEyeTop = new T[parsingFacial.LeftEyeTop.Length];
            LeftEyeBottom = new T[parsingFacial.LeftEyeBottom.Length];

            LeftLipTop = new T[parsingFacial.LeftLipTop.Length];
            LeftLipBottom = new T[parsingFacial.LeftLipBottom.Length];

            RightEar = new T[parsingFacial.RightEar.Length];

            RightEyeBrow = new T[parsingFacial.RightEyeBrow.Length];
            RightEyeTop = new T[parsingFacial.RightEyeTop.Length];
            RightEyeBottom = new T[parsingFacial.RightEyeBottom.Length];

            RightLipTop = new T[parsingFacial.RightLipTop.Length];
            RightLipBottom = new T[parsingFacial.RightLipBottom.Length];

            Nose = new T[parsingFacial.Nose.Length];
            Tounge = new T[parsingFacial.Tounge.Length];
            Chin = new T[parsingFacial.Chin.Length];
        }


        public override void InjectUniformValue(T dataValue)
        {
            HeadTop = dataValue;
            HeadMid = dataValue;
            HeadBottom = dataValue;


            LeftMidHead = dataValue;
            LeftCheek = dataValue;

            LeftEyeRoot = dataValue;
            LeftIris = dataValue;


            RightMidHead = dataValue;
            RightCheek = dataValue;

            RightEyeRoot = dataValue;
            RightIris = dataValue;

            LeftLipCorner = dataValue;
            RightLipCorner = dataValue;

            LipTop = dataValue;
            LipBottom = dataValue;



            Jaw = dataValue;
            LeftJaw = dataValue;
            RightJaw = dataValue;
            LeftBackJaw = dataValue;
            RightBackJaw = dataValue;

            int i;
            for (i = 0; i < LeftEar.Length; i++)
            {
                LeftEar[i] = dataValue;
            }

            for (i = 0; i < LeftEyeBrow.Length; i++)
            {
                LeftEyeBrow[i] = dataValue;
            }
            for (i = 0; i < LeftEyeTop.Length; i++)
            {
                LeftEyeTop[i] = dataValue;
            }
            for (i = 0; i < LeftEyeBottom.Length; i++)
            {
                LeftEyeBottom[i] = dataValue;
            }

            for (i = 0; i < LeftLipTop.Length; i++)
            {
                LeftLipTop[i] = dataValue;
            }
            for (i = 0; i < LeftLipBottom.Length; i++)
            {
                LeftLipBottom[i] = dataValue;
            }

            for (i = 0; i < RightEar.Length; i++)
            {
                RightEar[i] = dataValue;
            }

            for (i = 0; i < RightEyeBrow.Length; i++)
            {
                RightEyeBrow[i] = dataValue;
            }
            for (i = 0; i < RightEyeTop.Length; i++)
            {
                RightEyeTop[i] = dataValue;
            }
            for (i = 0; i < RightEyeBottom.Length; i++)
            {
                RightEyeBottom[i] = dataValue;
            }

            for (i = 0; i < RightLipTop.Length; i++)
            {
                RightLipTop[i] = dataValue;
            }
            for (i = 0; i < RightLipBottom.Length; i++)
            {
                RightLipBottom[i] = dataValue;
            }

            for (i = 0; i < Nose.Length; i++)
            {
                Nose[i] = dataValue;
            }
            for (i = 0; i < Tounge.Length; i++)
            {
                Tounge[i] = dataValue;
            }
            for (i = 0; i < Chin.Length; i++)
            {
                Chin[i] = dataValue;
            }
        }
    }



}
