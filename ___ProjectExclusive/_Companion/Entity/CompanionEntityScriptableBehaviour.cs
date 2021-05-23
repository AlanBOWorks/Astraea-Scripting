using System;
using System.Collections.Generic;
using Blanca;
using MEC;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Companion
{
    /// <summary>
    /// To initialize the <see cref="Companion.CompanionEntity"/>'s values; Normally
    /// this will be invoked by <seealso cref="Blanca"/> or <seealso cref="Player"/>
    /// (mainly <seealso cref="Blanca"/> since she depends more in this than <seealso cref="Player"/>)
    /// </summary>
    [CreateAssetMenu(fileName = "CompanionEntity [Scriptable Behaviour]",
        menuName = "Entity/Companion/Scriptable Behaviour")]
    public class CompanionEntityScriptableBehaviour : ScriptableObject
    {
        [SerializeField] 
        private CompanionEntitySingleton _singleton = CompanionEntitySingleton.Instance; //Handle for configurations
        [SerializeField]
        private CompanionEntity _entity = CompanionEntitySingleton.Instance.Entity;

        [ShowInInspector,HideInEditorMode]
        [TabGroup("Blanca")]
        private ICharacterTransformData _blancaTransform;
        [ShowInInspector,HideInEditorMode]
        [TabGroup("Player")]
        private ICharacterTransformData _playerTransform;

        private CoroutineHandle _coroutineHandle;
        public void CallForCompanionEntityInitialization()
        {
            CompanionEntitySingleton.Instance.EntityCaller = this;
            _coroutineHandle 
                = Timing.RunCoroutineSingleton(_Tracking(), _coroutineHandle, SingletonBehavior.Overwrite);

            IEnumerator<float> _Tracking()
            {
                BlancaEntity blancaEntity = BlancaEntitySingleton.Instance.Entity;
                PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;

                //There's no check for player since there's always
                // player while Blanca is present
                yield return Timing.WaitUntilTrue(BlancaInjectionWait);

                _blancaTransform = blancaEntity.CharacterTransformData;
                _playerTransform = playerEntity.CharacterTransformData;

                Transform blancaCheck = _blancaTransform.Root;
                Transform playerCheck = _playerTransform.Root;


                // Checks if the reference of both Transform exits (if on change of scene or
                // anything happens it should stop)
                while (blancaCheck && playerCheck) 
                {
                    yield return Timing.WaitForOneFrame;
                    DoEntityAction();
                }


                bool BlancaInjectionWait()
                {
                    return blancaEntity.CharacterTransformData != null && blancaEntity.CharacterTransformData.Root != null;
                }
            }
        }


        private void DoEntityAction()
        {
            Vector3 vectorTowardsPlayer = (_playerTransform.MeshWorldPosition - _blancaTransform.MeshWorldPosition);
            _entity.VectorTowardsPlayer = vectorTowardsPlayer;
            _entity.NormalizedVectorTowardsPlayer = vectorTowardsPlayer.normalized;
            _entity.DistanceOfSeparation = vectorTowardsPlayer.magnitude;
        }
    }
}
