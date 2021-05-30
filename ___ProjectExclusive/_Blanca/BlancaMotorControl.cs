using KinematicEssentials;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
    public class BlancaMotorControl : ITicker
    {
        private readonly IKinematicMotorHandler _motor;
        public readonly BlancaVelocityControlHolder VelocityControl;
        public readonly BlancaRotationControlHolder RotationControl;

        public BlancaMotorControl(IKinematicMotorHandler motor, 
            BlancaVelocityControlHolder velocityControl,
            BlancaRotationControlHolder rotationControl)
        {
            _motor = motor;
            VelocityControl = velocityControl;
            RotationControl = rotationControl;
        }

        public bool Disabled { get; set; }
        public void Tick()
        {
            _motor.DesiredVelocity = VelocityControl.CalculateTargetVelocity();
            _motor.DesiredRotationForward = RotationControl.CalculateForwardEuler();
        }
    }
}
