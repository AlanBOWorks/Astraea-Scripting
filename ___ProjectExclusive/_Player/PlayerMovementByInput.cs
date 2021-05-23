using KinematicEssentials;
using PlayerEssentials;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerMovementByInput : IPlayerControlState
    {
        public PlayerMovementByInput()
        {
            PlayerEntity entity = PlayerEntitySingleton.Instance.Entity;
            _motor = entity.MotorHandler;
            _inputData = entity.InputData;

            PlayerParametersVariable parameters = PlayerEntitySingleton.Instance.Parameters;
            _deltaAcceleration = parameters.DeltaAcceleration;
            _sprintModifier = parameters.SprintModifier;
        }

        private IKinematicMotorHandler _motor;
        private PlayerInputData _inputData;
        private BreakableAcceleration _deltaAcceleration;
        private float _sprintModifier;

        public void UpdateMotor()
        {
            Vector3 targetVelocity = _inputData.GlobalDesiredDirection;
            if (_inputData.IsSprintPress) targetVelocity *= _sprintModifier;

            Vector3 currentVelocity = _motor.DesiredVelocity;
            float deltaVariation =
                Time.deltaTime * _deltaAcceleration.DeltaModifier(currentVelocity, targetVelocity);
            _motor.DesiredVelocity = Vector3.Lerp(currentVelocity, targetVelocity, deltaVariation);
        }
    }
}
