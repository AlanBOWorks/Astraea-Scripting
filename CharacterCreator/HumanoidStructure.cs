using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CharacterCreator
{
    public abstract class SerializerHumanoidStructure<T> : HumanoidStructure<T> where T : Component
    {

        /// <summary>
        /// Biped for the skeletonBiped (normally the root of the 3d Mesh's skeleton)
        /// </summary>
        [SerializeField] protected T BipedRoot;
        [SerializeField] protected T Pelvis;

        [SerializeField] protected T Head;
        [SerializeField] protected T Neck;

        [SerializeField] protected T LeftShoulder;
        [SerializeField] protected T LeftUpperArm;
        [SerializeField] protected T LeftLowerArm;
        [SerializeField] protected T LeftHand;

        [ReadOnly]
        public T[] LeftIndex = new T[SingleFingerLength];
        [ReadOnly]
        public T[] LeftLittle = new T[SingleFingerLength];
        [ReadOnly]
        public T[] LeftMiddle = new T[SingleFingerLength];
        [ReadOnly]
        public T[] LeftRing = new T[SingleFingerLength];
        [ReadOnly]
        public T[] LeftThumbs = new T[SingleFingerLength];

        [SerializeField] protected T RightShoulder;
        [SerializeField] protected T RightUpperArm;
        [SerializeField] protected T RightLowerArm;
        [SerializeField] protected T RightHand;

        [ReadOnly]
        public T[] RightIndex = new T[SingleFingerLength];
        [ReadOnly]
        public T[] RightLittle = new T[SingleFingerLength];
        [ReadOnly]
        public T[] RightMiddle = new T[SingleFingerLength];
        [ReadOnly]
        public T[] RightRing = new T[SingleFingerLength];
        [ReadOnly]
        public T[] RightThumbs = new T[SingleFingerLength];

        [SerializeField] protected T UpperChest;
        [SerializeField] protected T Optional_Chest;
        [SerializeField] protected T SpineBase;

        [SerializeField] protected T LeftThigh;
        [SerializeField] protected T LeftCalf;
        [SerializeField] protected T LeftFoot;
        [SerializeField] protected T LeftToe;

        [SerializeField] protected T RightThigh;
        [SerializeField] protected T RightCalf;
        [SerializeField] protected T RightFoot;
        [SerializeField] protected T RightToe;

        protected T[] GetFingers(T hand)
        {
            T[] gets = hand.GetComponentsInChildren<T>();
            T[] serializated = new T[gets.Length - 1];
            for (int i = 0; i < serializated.Length; i++)
            {
                serializated[i] = gets[i + 1];
            }

            return serializated;
        }
        private void SerializeRightFingers(T[] serializedFingers)
        {
            RightFingers = serializedFingers;
            int j = 0;
            for (int i = 0; i < SingleFingerLength; i++, j++)
            {
                RightIndex[i] = serializedFingers[j];
            }
            for (int i = 0; i < SingleFingerLength; i++, j++)
            {
                RightLittle[i] = serializedFingers[j];
            }
            for (int i = 0; i < SingleFingerLength; i++, j++)
            {
                RightMiddle[i] = serializedFingers[j];
            }
            for (int i = 0; i < SingleFingerLength; i++, j++)
            {
                RightRing[i] = serializedFingers[j];
            }
            for (int i = 0; i < SingleFingerLength; i++, j++)
            {
                RightThumbs[i] = serializedFingers[j];
            }
        }
        private void SerializeLeftFingers(T[] serializedFingers)
        {
            LeftFingers = serializedFingers;
            int j = 0;
            for (int i = 0; i < SingleFingerLength; i++)
            {
                LeftIndex[i] = serializedFingers[j];
                j++;
            }
            for (int i = 0; i < SingleFingerLength; i++)
            {
                LeftLittle[i] = serializedFingers[j];
                j++;
            }
            for (int i = 0; i < SingleFingerLength; i++)
            {
                LeftMiddle[i] = serializedFingers[j];
                j++;
            }
            for (int i = 0; i < SingleFingerLength; i++)
            {
                LeftRing[i] = serializedFingers[j];
                j++;
            }
            for (int i = 0; i < SingleFingerLength; i++)
            {
                LeftThumbs[i] = serializedFingers[j];
                j++;
            }
        }

        [Button("Serialize",ButtonSizes.Gigantic)]
        public void Serialize()
        {
            Roots[0] = BipedRoot;
            Roots[1] = Pelvis;

            NeckHead[0] = Neck;
            NeckHead[1] = Head;



            LeftArm[0] = LeftShoulder;
            LeftArm[1] = LeftUpperArm;
            LeftArm[2] = LeftLowerArm;
            LeftArm[3] = LeftHand;
            SerializeLeftFingers(GetFingers(LeftHand));

            RightArm[0] = RightShoulder;
            RightArm[1] = RightUpperArm;
            RightArm[2] = RightLowerArm;
            RightArm[3] = RightHand;
            SerializeRightFingers(GetFingers(RightHand));

            LeftLeg[0] = LeftThigh;
            LeftLeg[1] = LeftCalf;
            LeftLeg[2] = LeftFoot;
            LeftLeg[3] = LeftToe;

            RightLeg[0] = RightThigh;
            RightLeg[1] = RightCalf;
            RightLeg[2] = RightFoot;
            RightLeg[3] = RightToe;



            Spines[0] = SpineBase;
            Spines[1] = UpperChest;
            Spines[2] = Optional_Chest;

        }
    }

    [Serializable]
    public abstract class HumanoidStructure<T> : HumanStructureBase<T>
    {

        public const int TotalLength = RootLength + NeckHeadLength + ArmLength * 2 + SingleFingerLength * 2 + SpineLength +
                                       LegLength * 2;

        protected HumanoidStructure()
        {
            
        }

        protected HumanoidStructure(T uniformValue) : base(uniformValue)
        {
            
        }

        public const int RootLength = 2;
        [DisableInEditorMode]
        public T[] Roots = new T[RootLength];

        public const int NeckHeadLength = 2;
        [DisableInEditorMode]
        public T[] NeckHead = new T[NeckHeadLength];


        public const int ArmLength = 4;
        public const int SingleFingerLength = 4;
        public const int TotalFingersLength = 5 * SingleFingerLength;
        [DisableInEditorMode]
        public T[] LeftArm = new T[ArmLength];
        [DisableInEditorMode]
        public T[] LeftFingers = new T[TotalFingersLength];
        [DisableInEditorMode]
        public T[] RightArm = new T[ArmLength];
        [DisableInEditorMode]
        public T[] RightFingers = new T[TotalFingersLength * SingleFingerLength];

        public const int SpineLength = 3;
        [DisableInEditorMode]
        public T[] Spines = new T[SpineLength];

        public const int LegLength = 4;
        [DisableInEditorMode]
        public T[] LeftLeg = new T[LegLength];

        [DisableInEditorMode]
        public T[] RightLeg = new T[LegLength];



        #region << INJECTORS >>
       
        /// <summary>
        /// Initially created for <seealso cref="Vector3.Lerp"/>
        /// so it overrides the <seealso cref="Transform"/>'s values
        /// <br></br>
        /// <br></br>
        /// - !!! NOT for Runtime (it's designed for initialization/Serialize/DeSerialize)
        /// </summary>
        public void DoParse<TPrimary, TSecondaryParse, TThird>
        (HumanoidStructure<TPrimary> primaryHumanoid, HumanoidStructure<TSecondaryParse> secondaryHumanoid,HumanoidStructure<TThird> thirdHumanoid,
            HumanAction<TPrimary, TSecondaryParse, TThird> parseAction)
        {
            for (int j = 0; j < RootLength; j++)
            {
                parseAction(Roots[j],primaryHumanoid.Roots[j], secondaryHumanoid.Roots[j], thirdHumanoid.Roots[j]);
            }
            for (int j = 0; j < NeckHeadLength; j++)
            {
                parseAction(NeckHead[j],primaryHumanoid.NeckHead[j], secondaryHumanoid.NeckHead[j],thirdHumanoid.NeckHead[j]);
            }
            for (int j = 0; j < ArmLength; j++)
            {
                parseAction(LeftArm[j], primaryHumanoid.LeftArm[j], secondaryHumanoid.LeftArm[j],thirdHumanoid.LeftArm[j]);
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                 parseAction(LeftFingers[j],primaryHumanoid.LeftFingers[j], secondaryHumanoid.LeftFingers[j],thirdHumanoid.LeftFingers[j]);
            }
            for (int j = 0; j < ArmLength; j++)
            {
                parseAction(RightArm[j],primaryHumanoid.RightArm[j], secondaryHumanoid.RightArm[j],thirdHumanoid.RightArm[j]);
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                parseAction(RightFingers[j],primaryHumanoid.RightFingers[j], secondaryHumanoid.RightFingers[j],thirdHumanoid.RightFingers[j]);
            }
            for (int j = 0; j < SpineLength; j++)
            {
                parseAction(Spines[j],primaryHumanoid.Spines[j], secondaryHumanoid.Spines[j],thirdHumanoid.RightFingers[j]);
            }
            for (int j = 0; j < LegLength; j++)
            {
                parseAction(LeftLeg[j],primaryHumanoid.LeftLeg[j], secondaryHumanoid.LeftLeg[j],thirdHumanoid.LeftLeg[j]);
            }
            for (int j = 0; j < LegLength; j++)
            {
                parseAction(RightLeg[j],primaryHumanoid.RightLeg[j], secondaryHumanoid.RightLeg[j],thirdHumanoid.RightLeg[j]);
            }
        }
        /// <summary>
        /// Used for inject a certain type value into this by <see cref="HumanFunc{TParse}"/>
        /// <br></br>
        /// Initially created for inject the <seealso cref="Transform"/>'s values to another <seealso cref="HumanoidStructure{T}"/> where <see cref="T"/> :
        /// <seealso cref="Vector3"/>, <seealso cref="Quaternion"/>
        /// <br></br>
        /// <br></br>
        /// - !!! NOT for Runtime (it's designed for initialization/Serialize/DeSerialize)
        /// </summary>
        public void InjectParse<TParse>(HumanoidStructure<TParse> humanoid, HumanFunc<TParse> parseFunc)
        {
            for (int j = 0; j < RootLength; j++)
            {
                Roots[j] = parseFunc(humanoid.Roots[j]);
            }
            for (int j = 0; j < NeckHeadLength; j++)
            {
                NeckHead[j] = parseFunc(humanoid.NeckHead[j]);
            }
            for (int j = 0; j < ArmLength; j++)
            {
                LeftArm[j] = parseFunc(humanoid.LeftArm[j]);
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                LeftFingers[j] = parseFunc(humanoid.LeftFingers[j]);
            }
            for (int j = 0; j < ArmLength; j++)
            {
                RightArm[j] = parseFunc(humanoid.RightArm[j]);
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                RightFingers[j] = parseFunc(humanoid.RightFingers[j]);
            }
            for (int j = 0; j < SpineLength; j++)
            {
                Spines[j] = parseFunc(humanoid.Spines[j]);
            }
            for (int j = 0; j < LegLength; j++)
            {
                LeftLeg[j] = parseFunc(humanoid.LeftLeg[j]);
            }
            for (int j = 0; j < LegLength; j++)
            {
                RightLeg[j] = parseFunc(humanoid.RightLeg[j]);
            }
        }

        public override T[] GenerateArray()
        {
            T[] generated = new T[TotalLength];

            int i = 0;
            for (int j = 0; j < RootLength; i++, j++)
            {
                generated[i] = Roots[j];
            }
            for (int j = 0; j < NeckHeadLength; i++, j++)
            {
                generated[i] = NeckHead[j];
            }
            for (int j = 0; j < ArmLength; i++, j++)
            {
                generated[i] = LeftArm[j];
            }
            for (int j = 0; j < TotalFingersLength; i++, j++)
            {
                generated[i] = LeftFingers[j];
            }
            for (int j = 0; j < ArmLength; i++, j++)
            {
                generated[i] = RightArm[j];
            }
            for (int j = 0; j < TotalFingersLength; i++, j++)
            {
                generated[i] = RightFingers[j];
            }

            for (int j = 0; j < SpineLength; i++, j++)
            {
                generated[i] = Spines[j];
            }

            for (int j = 0; j < LegLength; i++, j++)
            {
                generated[i] = LeftLeg[j];
            }
            for (int j = 0; j < LegLength; i++, j++)
            {
                generated[i] = RightLeg[j];
            }

            return generated;
        }

        /// <summary>
        /// Used to inject one unique <seealso cref="dataValue"/> in the whole <see cref="HumanoidStructure{T}"/>
        /// <br></br>
        /// Useful for initialization or whole reset in Editor
        /// <br></br>
        /// <br></br>
        /// - !!! Not recommendable for Runtime (it's designed for initialization/Serialize/DeSerialize)
        /// </summary>
        public override void InjectUniformValue(T dataValue)
        {
            for (int j = 0; j < RootLength; j++)
            {
                Roots[j] = dataValue;
            }
            for (int j = 0; j < NeckHeadLength; j++)
            {
                NeckHead[j] = dataValue;
            }
            for (int j = 0; j < ArmLength; j++)
            {
                LeftArm[j] = dataValue;
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                LeftFingers[j] = dataValue;
            }
            for (int j = 0; j < ArmLength; j++)
            {
                RightArm[j] = dataValue;
            }
            for (int j = 0; j < TotalFingersLength; j++)
            {
                RightFingers[j] = dataValue;
            }

            for (int j = 0; j < SpineLength; j++)
            {
                Spines[j] = dataValue;
            }

            for (int j = 0; j < LegLength; j++)
            {
                LeftLeg[j] = dataValue;
            }
            for (int j = 0; j < LegLength; j++)
            {
                RightLeg[j] = dataValue;
            }
        } 
        #endregion

        #region << GETS >>

        public T GetBipedRoot()
        {
            return Roots[0];
        }
        public T GetPelvis()
        {
            return Roots[1];
        }

        public T GetHead()
        {
            return NeckHead[0];
        }
        public T GetNeck()
        {
            return NeckHead[1];
        }

        public T GetLeftShoulder()
        {
            return LeftArm[0];
        }
        public T GetLeftUpperArm()
        {
            return LeftArm[1];
        }
        public T GetLeftLowerArm()
        {
            return LeftArm[2];
        }
        public T GetLeftHand()
        {
            return LeftArm[3];
        }
        public T GetRightShoulder()
        {
            return RightArm[0];
        }
        public T GetRightUpperArm()
        {
            return RightArm[1];
        }
        public T GetRightLowerArm()
        {
            return RightArm[2];
        }
        public T GetRightHand()
        {
            return RightArm[3];
        }

        public T GetUpperChest()
        {
            return Spines[0];
        }
        public T GetOptional_Chest()
        {
            return Spines[2];
        }
        public T GetSpineBase()
        {
            return Spines[1];
        }

        public T GetLeftThGetThigh()
        {
            return LeftLeg[0];
        }
        public T GetLeftCalf()
        {
            return LeftLeg[1];
        }
        public T GetLeftFoot()
        {
            return LeftLeg[2];
        }
        public T GetLeftToe()
        {
            return LeftLeg[3];
        }

        public T GetRightThGetThigh()
        {
            return RightLeg[0];
        }
        public T GetRightCalf()
        {
            return RightLeg[1];
        }
        public T GetRightFoot()
        {
            return RightLeg[2];
        }
        public T GetRightToe()
        {
            return RightLeg[3];
        } 
        #endregion
    }


    public class MinimalHumanoidStructure<T>
    {
        public readonly T[] Humanoid;

        public MinimalHumanoidStructure(HumanoidStructure<T> humanoid)
        {
            Humanoid = humanoid.GenerateArray();
        }
    }


    public abstract class HumanStructureBase<T>
    {
        protected HumanStructureBase() { }

        protected HumanStructureBase(T uniformValue)
        {
            InjectUniformValue(uniformValue);
        }

        public delegate T HumanFunc<in TParse>(TParse onParse);
        public delegate void HumanAction<in TPrimary, in TSecondary, in TThird>(T element, TPrimary primary, TSecondary secondary, TThird third);

        public abstract T[] GenerateArray();
        public abstract void InjectUniformValue(T dataValue);
    }
}
