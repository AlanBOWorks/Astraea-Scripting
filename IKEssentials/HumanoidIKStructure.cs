using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    

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
