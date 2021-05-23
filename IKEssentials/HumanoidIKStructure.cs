using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{

    public abstract class HumanoidIKStructure<T> : HumanoidIKStructureBase<T>
    {
        protected HumanoidIKStructure(IHumanoidIKStructure<T> structure, bool isLeftMain)
        {
            Head = structure.Head;
            LeftHand = structure.LeftHand;
            RightHand = structure.RightHand;

            SetUpMainHand(isLeftMain);
        }

        public sealed override T Head { get; set; }

        public sealed override T LeftHand { get; set; }

        public sealed override T RightHand { get; set; }
    }

    public class SerializedHumanoidIKStructure<T> : HumanoidIKStructureBase<T>
    {
        [SerializeField] private T _head;
        public override T Head
        {
            get => _head;
            set => _head = value;
        }

        [SerializeField] private T _rightHand;
        public override T RightHand
        {
            get => _rightHand;
            set => _rightHand = value;
        }

        [SerializeField] private T _leftHand;
        public override T LeftHand
        {
            get => _leftHand;
            set => _leftHand = value;
        }
    }

    public abstract class HumanoidIKStructureBase<T> : IHumanoidIKStructure<T>
    {
        public abstract T Head { get; set; }
        public abstract T RightHand { get; set; }
        public abstract T LeftHand { get; set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public T MainHand { get; protected set; }
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public T SecondaryHand { get; protected set; }


        public void SetUpMainHand(bool isLeftMain)
        {
            if (isLeftMain)
            {
                MainHand = LeftHand;
                SecondaryHand = RightHand;
            }
            else
            {
                MainHand = RightHand;
                SecondaryHand = LeftHand;
            }
        }
    }

    public interface IHumanoidIKStructure<out T>
    {
        T Head { get; }
        
        T RightHand { get; }
        T LeftHand { get; }

        T MainHand { get; }
        T SecondaryHand { get; }
    }

    public static class HumanoidIKUtils
    {

        public delegate void ParseAction<in T,in TParse>(T main, TParse secondary);

        public static void ParsingAction<T,TParse>(IHumanoidIKStructure<T> main,IHumanoidIKStructure<TParse> parsingHumanoid, 
            ParseAction<T,TParse> parseAction)
        {
            parseAction(main.Head, parsingHumanoid.Head);
            parseAction(main.LeftHand, parsingHumanoid.LeftHand);
            parseAction(main.RightHand, parsingHumanoid.RightHand);
            parseAction(main.MainHand, parsingHumanoid.MainHand);
            parseAction(main.SecondaryHand, parsingHumanoid.SecondaryHand);

        }
    }
}
