using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Companion
{
    /// <summary>
    /// Used mainly to keep track related to both <seealso cref="Blanca.BlancaEntity"/> and
    /// <seealso cref="Player.PlayerEntity"/> (which can't be handled by them self; <example>eg: Distance
    /// between them both; Both are in the same area; etc.</example>)
    /// </summary>
    [Serializable]
    public class CompanionEntity
    {
        public float DistanceOfSeparation = -1f; //Default negative for checks
        public Vector3 VectorTowardsPlayer;
        public Vector3 NormalizedVectorTowardsPlayer;
    }

    /// <summary>
    /// Single entity made by the conjunction of <seealso cref="Blanca.BlancaEntity"/> & <seealso cref="Player.PlayerEntity"/>
    /// </summary>
    [Serializable]
    public sealed class CompanionEntitySingleton
    {
        static CompanionEntitySingleton() { }
        private CompanionEntitySingleton() { }
        public static CompanionEntitySingleton Instance { get; } = new CompanionEntitySingleton();


        public CompanionEntityScriptableBehaviour EntityCaller;

        [SerializeField, HideInEditorMode, HideInPlayMode, HideInInlineEditors, HideDuplicateReferenceBox]
        public CompanionEntity Entity = new CompanionEntity();
    }

}
