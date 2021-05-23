using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PlayerControllerStatesHandler : MonoBehaviour
    {
        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        public IPlayerControlState CurrentState { get; private set; }
        private PlayerMovementByInput _movementByInput;

        private void Start()
        {
            _movementByInput = new PlayerMovementByInput();
            CurrentState = _movementByInput;
        }

        private void Update()
        {
            CurrentState.UpdateMotor();
        }
    }

    public interface IPlayerControlState
    {
        void UpdateMotor();
    }
}
