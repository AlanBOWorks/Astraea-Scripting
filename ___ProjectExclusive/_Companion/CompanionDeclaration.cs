using UnityEngine;

namespace Companion
{
    public class CompanionDeclaration : MonoBehaviour
    {
        [SerializeField] 
        private MainCharactersSpawner _spawner = new MainCharactersSpawner();

        [SerializeField]
        private CompanionEntityCallEvent _caller = new CompanionEntityCallEvent();

        // Start is called before the first frame update
        void Awake()
        {
            _spawner.Spawn();
        }

        private void Start()
        {
            _caller.DoCall();
        }

    }
}
