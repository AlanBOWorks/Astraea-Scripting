using UnityEngine;

namespace Companion
{
    public static class CompanionUtils
    {
        private static CompanionEntity _entity => CompanionEntitySingleton.Instance.Entity;

        public static float DirectDistanceOfSeparation => _entity.DistanceOfSeparation;
        public static Vector3 BlancaToPlayerVector => _entity.VectorTowardsPlayer;
        public static Vector3 NormalizedBlancaPlayerDirection => _entity.NormalizedVectorTowardsPlayer;
    }
}
