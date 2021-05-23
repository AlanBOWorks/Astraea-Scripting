using AIEssentials;
using KinematicEssentials;
using PlayerEssentials;
using UnityEngine;

namespace Player
{
    public static class PlayerUtilsTransform
    {
        private static readonly PlayerEntity Entity = PlayerEntitySingleton.Instance.Entity;

        public static IPlayerTransformData GetTransformData() => Entity.CharacterTransformData;
        public static Vector3 GetCurrentPosition() => GetTransformData().MeshWorldPosition;
    }

    public static class PlayerUtilsKinematic
    {
        private static readonly PlayerEntity _entity = PlayerEntitySingleton.Instance.Entity;
        public static IPathCalculator MainHelper;

        public static KinematicData GetPlayerKinematicData() => _entity.KinematicData;

        public static float CurrentSpeed => GetPlayerKinematicData().CurrentSpeed;
        public static bool IsPlayerMoving => _entity.InputData.IsMoving;

        public static Vector3 GetPlayerPointOfVelocity() => GetPlayerKinematicData().PointOfDesiredVelocity;
    }

    
}
