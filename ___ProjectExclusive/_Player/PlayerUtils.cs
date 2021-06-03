using AIEssentials;
using KinematicEssentials;
using PlayerEssentials;
using UnityEngine;

namespace Player
{
    public static class PlayerUtilsTransform
    {
        private static IPlayerTransformData _transformData = null;
        public static IPlayerTransformData TransformData
        {
            get => _transformData;
            set
            {
                _transformData = value;
                HeadTransform = _transformData.Head;
                BodyTransform = _transformData.MeshRoot;
            }
        }
        public static Transform HeadTransform { get; private set; }
        public static Transform BodyTransform { get; private set; }

        public static Vector3 GetCurrentPosition() => TransformData.MeshWorldPosition;
        public static Vector3 CalculateFormationPosition(Vector2 planeLocalPosition)
        {
            Vector3 offSet = BodyTransform.TransformDirection(planeLocalPosition.x, 0, planeLocalPosition.y);
            return GetCurrentPosition() + offSet;
        }
    }

    public static class PlayerUtilsKinematic
    {
        public static IPathCalculator MainHelper;

        public static PlayerInputData InputData = null;
        public static IKinematicData KinematicData = null;

        public static float CurrentSpeed => KinematicData.CurrentSpeed;
        public static bool IsPlayerMoving => InputData.IsMoving;
    }

    
}
