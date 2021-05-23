using System;
using BOC.BTagged;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Companion
{
    [Serializable]
    public class MainCharactersSpawner
    {
        [TitleGroup("Player")]
        [SerializeField] private Tag _playerCheck;
        [TitleGroup("Player")]
        [SerializeField]
        private GameObject _player;

        [TitleGroup("Blanca")]
        [SerializeField] private Tag _blancaCheck;
        [TitleGroup("Blanca")]
        [SerializeField]
        private GameObject _blanca;

        
        [TitleGroup("Local References")]
        public Transform PlayerSpawnPoint;
        [TitleGroup("Local References")]
        public Transform BlancaSpawnPoint;

        public void Spawn()
        {
            if(PlayerSpawnPoint is null || BlancaSpawnPoint is null)
                throw new ArgumentException("There's no spawners Transform");

            if(BTagged.Find(_playerCheck).GetFirstGameObject() is null)
                Object.Instantiate(_player, PlayerSpawnPoint.position, Quaternion.identity);
            if(BTagged.Find(_blancaCheck).GetFirstGameObject() is null)
                Object.Instantiate(_blanca, BlancaSpawnPoint.position, Quaternion.identity);
        }

    }
}
