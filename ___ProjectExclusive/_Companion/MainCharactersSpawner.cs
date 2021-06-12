using System;
using Blanca;
using BOC.BTagged;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Companion
{
    [Serializable]
    public class MainCharactersSpawner
    {
        [TitleGroup("Player")]
        [SerializeField]
        private GameObject _player;

        [TitleGroup("Blanca")]
        [SerializeField]
        private GameObject _blanca;

        
        [TitleGroup("Local References")]
        public Transform PlayerSpawnPoint;
        [TitleGroup("Local References")]
        public Transform BlancaSpawnPoint;

        public void Spawn()
        {
            if (PlayerSpawnPoint != null)
            {
                PlayerEntitySingleton playerSingleton = PlayerEntitySingleton.Instance;
                PlayerEntity playerEntity = playerSingleton.Entity;
                ICharacterTransformData playerTransformData = playerEntity.CharacterTransformData;

                if(playerTransformData is null)
                    Object.Instantiate(_player, PlayerSpawnPoint.position, Quaternion.identity);
            }

            if (BlancaSpawnPoint != null)
            {
                BlancaEntitySingleton blancaSingleton = BlancaEntitySingleton.Instance;
                BlancaEntity blancaEntity = blancaSingleton.Entity;
                ICharacterTransformData blancaTransformData = blancaEntity.CharacterTransformData;

                if (blancaTransformData is null)
                    Object.Instantiate(_blanca, BlancaSpawnPoint.position, Quaternion.identity);
            }
        }

    }
}
