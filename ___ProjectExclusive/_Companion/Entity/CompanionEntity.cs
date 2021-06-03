using System;
using IKEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Companion
{
    /// <summary>
    /// Used mainly to keep track related to both <seealso cref="Blanca.BlancaEntity"/> and
    /// <seealso cref="Player.PlayerEntity"/> (which can't be handled by them self; <example>eg: Distance
    /// between them both; Both are in the same area; etc.</example>)
    /// </summary>
    public class CompanionEntity
    {
        public float DistanceOfSeparation = -1f; //Default negative for checks
        public Vector3 VectorTowardsPlayer;
        public Vector3 NormalizedVectorTowardsPlayer;

        public IKEssentials.HoldHandHandler HoldHandHandler = null;

    }

    /// <summary>
    /// Single entity made by the conjunction of <seealso cref="Blanca.BlancaEntity"/> & <seealso cref="Player.PlayerEntity"/>
    /// </summary>
    public sealed class CompanionEntitySingleton
    {
        static CompanionEntitySingleton() { }

        private CompanionEntitySingleton()
        {
            Entity = new CompanionEntity();
        }
        public static CompanionEntitySingleton Instance { get; } = new CompanionEntitySingleton();


        public CompanionEntityScriptableBehaviour EntityCaller = null;

        [SerializeField, HideInEditorMode, HideInPlayMode, HideInInlineEditors, HideDuplicateReferenceBox]
        public CompanionEntity Entity;
    }

}
