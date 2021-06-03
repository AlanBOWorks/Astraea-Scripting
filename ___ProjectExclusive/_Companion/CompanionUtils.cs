using AIEssentials;
using Blanca;
using KinematicEssentials;
using PlayerEssentials;
using SharedLibrary;
using UnityEngine;

namespace Companion
{
    public static class CompanionUtils
    {
        private static CompanionEntity Entity => CompanionEntitySingleton.Instance.Entity;

        public static ICharacterTransformData BlancaTransformData = null;
        public static IPlayerTransformData PlayerTransformData = null;

        public static KinematicData PlayerKinematicData = null;

        public static float DirectDistanceOfSeparation => Entity.DistanceOfSeparation;
        public static Vector3 BlancaToPlayerVector => Entity.VectorTowardsPlayer;
        public static Vector3 NormalizedBlancaPlayerDirection => Entity.NormalizedVectorTowardsPlayer;


        public static bool PlayerMovementInLeadDirection(float distanceThreshold = .2f)
        {
            Vector3 playerDesiredPoint = PlayerKinematicData.PointOfDesiredVelocity;
            IPathCalculator leadPathCalculator = BlancaUtilsKinematic.PathControls.Lead;
            Vector3 leadPoint = BlancaTransformData.MeshWorldPosition + leadPathCalculator.DesiredVelocity();
            return (playerDesiredPoint - leadPoint).sqrMagnitude < distanceThreshold * distanceThreshold;
        }
    }
}
