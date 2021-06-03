using System;
using IKEssentials;
using SharedLibrary;
using UnityEngine;

namespace Companion
{
    public class CompanionDeclaration : MonoBehaviour
    {
        [SerializeField] 
        private MainCharactersSpawner _spawner = new MainCharactersSpawner();

        [SerializeField] private CompanionEntityScriptableBehaviour behaviourHolder = null;

        void Awake()
        {
            _spawner.Spawn();
        }

        private void Start()
        {
            behaviourHolder.CallForCompanionEntityInitialization();
        }

    }
}
