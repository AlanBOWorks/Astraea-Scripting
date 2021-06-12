using System;
using ___ProjectExclusive;
using Blanca;
using IKEssentials;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Companion
{
    public class CompanionDeclaration : MonoBehaviour
    {
        [Title("Spawn")]
        [SerializeField] 
        private MainCharactersSpawner _spawner = new MainCharactersSpawner();
        [Title("Holder")]
        [SerializeField] private CompanionEntityScriptableBehaviour behaviourHolder = null;
        [SerializeField] private HoldHandTransforms holdHandTransforms = new HoldHandTransforms();

        [Title("Runtime Parameters")]
        [SerializeField] private HoldHandParameters holdHandParameters = new HoldHandParameters();

        void Awake()
        {
            _spawner.Spawn();
        }

        private void Start()
        {
            CompanionEntity entity = CompanionEntitySingleton.Instance.Entity; 
            BlancaEntity blancaEntity = BlancaEntitySingleton.Instance.Entity;
            PlayerEntity playerEntity = PlayerEntitySingleton.Instance.Entity;

            InstantiateHandHoldHandler();
            behaviourHolder.CallForCompanionEntityInitialization();


            void InstantiateHandHoldHandler()
            {
                bool isPlayerRightHanded = holdHandParameters.IsPlayerRightHanded;
                entity.IsPlayerRightHanded = isPlayerRightHanded;

                ICharacterDataHolder rightHanded;
                ICharacterDataHolder leftHanded;
                if (isPlayerRightHanded)
                {
                    rightHanded = playerEntity;
                    leftHanded = blancaEntity;
                }
                else
                {
                    rightHanded = blancaEntity;
                    leftHanded = playerEntity;
                }
                HoldHandHandler holdHandHandler = new HoldHandHandler(
                    leftHanded, rightHanded, 
                    holdHandTransforms);
                entity.HoldHandHandler = holdHandHandler;
                holdHandHandler.InjectParameters(holdHandParameters);


            }
        }

    }
}
