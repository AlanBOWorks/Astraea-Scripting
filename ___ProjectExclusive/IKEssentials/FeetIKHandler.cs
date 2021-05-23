using System.Collections.Generic;
using KinematicEssentials;
using MEC;
using RootMotion.FinalIK;
using UnityEngine;

namespace IKEssentials
{
    public class FeetIKHandler : IMovementEventListener
    {
        private readonly FootHandler _leftHandler;
        private readonly FootHandler _rightHandler;
        private readonly float _releaseSpeed;

        public FeetIKHandler(FullBodyBipedIK biped, float releaseSpeed = 4)
        {

            _leftHandler = new FootHandler(
                biped.solver.leftFootEffector,
                biped.references.leftFoot
                );
            _rightHandler = new FootHandler(
                biped.solver.rightFootEffector,
                biped.references.rightFoot);

            _releaseSpeed = releaseSpeed;
        }




        public void SwitchToMoveState(float currentSpeed)
        {
            _leftHandler.ReleaseEffector(_releaseSpeed);
            _rightHandler.ReleaseEffector(_releaseSpeed);
        }

        public void SwitchToStopState(float currentSpeed)
        {
            _leftHandler.PinEffector();
            _rightHandler.PinEffector();
        }
        public void OnLongMovement(float currentSpeed)
        {
        }

        public void OnLongStop(float currentSpeed)
        {
            //TODO animate to idle
            _leftHandler.ReleaseEffector(1);
            _rightHandler.ReleaseEffector(1);
        }

        internal class FootHandler
        {
            public IKEffector Effector { get; }
            public Transform Bone { get; }

            public FootHandler(IKEffector effector, Transform bone)
            {
                Effector = effector;
                Bone = bone;
            }

            private CoroutineHandle _pinHandle;
            public void PinEffector()
            {
                Effector.positionWeight = 1;
                Effector.position = Bone.position;
            }

            //TODO releaseOnRotation
            public void ReleaseEffector(float changeSpeed)
            {
                Timing.KillCoroutines(_pinHandle);
                _pinHandle = Timing.RunCoroutine(_LerpWeight());

                IEnumerator<float> _LerpWeight()
                {
                    while (Effector.positionWeight > 0.05f)
                    {
                        Effector.positionWeight -= Timing.DeltaTime * changeSpeed;
                        yield return Timing.DeltaTime;
                    }

                    Effector.positionWeight = 0;
                }

            }
        }

    }
}
