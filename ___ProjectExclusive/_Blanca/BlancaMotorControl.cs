using KinematicEssentials;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
    public class BlancaMotorControl : ITicker
    {
        private readonly IKinematicMotorHandler _motor;
        private readonly BlancaVelocityControlHolder _velocityControl;
        private readonly BlancaRotationControlHolder _rotationControl;

        public BlancaMotorControl(IKinematicMotorHandler motor, 
            BlancaVelocityControlHolder velocityControl,
            BlancaRotationControlHolder rotationControl)
        {
            _motor = motor;
            _velocityControl = velocityControl;
            _rotationControl = rotationControl;
        }

        public bool Disabled { get; set; }
        public void Tick()
        {
            _motor.DesiredVelocity = _velocityControl.CalculateTargetVelocity();
            _motor.DesiredRotationForward = _rotationControl.CalculateForwardEuler();
        }
    }
}
