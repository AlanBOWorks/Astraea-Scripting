using System.Collections.Generic;
using KinematicEssentials;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerEssentials
{

    public class PlayerBodyRotationHandler : MonoInjector, IKinematicRotation,
        IRotationTriggerHolder
    {
        private IPlayerTransformData _playerTransform = null;
        private IInputMovement _inputMovement = null;
        public Quaternion CurrentRotation => transform.rotation;
        public Vector3 NormalizedCurrentRotationForward { get; private set; }
        public Vector3 NormalizedDesiredRotationForward => _playerTransform.CameraPlanarForward;


        [Title("Params")]
        [Range(-1,1), HideInPlayMode] public float AngleDotThreshold = -.3f;
        public AnimationCurve RotationSpeedByAngle = new AnimationCurve(
            new Keyframe(0,2),new Keyframe(180f,20f));

        public void Injection(IPlayerTransformData transformData, IInputMovement inputMovement)
        {
            _playerTransform = transformData;
            _inputMovement = inputMovement;
        }


        private List<IRotationTriggerListener> _listeners;
        public void AddListener(IRotationTriggerListener listener)
        {
            _listeners.Add(listener);
        }
        public void RemoveListener(IRotationTriggerListener listener)
        {
            _listeners.Remove(listener);
        }
        public override void DoInjection()
        {
            _listeners = new List<IRotationTriggerListener>(4);//The predicted amount of listeners
            NormalizedCurrentRotationForward = transform.forward;
            _canRotate = true;
            //Because this component could be disabled but the forward still needs to be updated
            Timing.RunCoroutine(_TrackForward());
        }


        private IEnumerator<float> _TrackForward()
        {
            do
            {
                NormalizedCurrentRotationForward = transform.forward;
                yield return Timing.WaitForOneFrame;
            }
            while (transform != null);
        }

        private bool _canRotate;
        private void Update()
        {
            HandleAngle();
            bool isMoving = _inputMovement.IsMoving;
            if (_canRotate || isMoving)
            {
                LerpRotation();
            }
        }

        private void HandleAngle()
        {
            float angleDot = Vector3.Dot(NormalizedDesiredRotationForward, NormalizedCurrentRotationForward);
            if (!_canRotate)
            {
                if (angleDot > AngleDotThreshold) return;

                _canRotate = true;
                InvokeLongAngleEvent(angleDot);
            }
            else
            {
                if(angleDot < .85f) return;
                _canRotate = false;
                InvokeReturnToForward(angleDot);
            }
        }

        private void InvokeLongAngleEvent(float dotAngle)
        {
            foreach (IRotationTriggerListener listener in _listeners)
            {
                listener.InLargeAngle(dotAngle);
            }
        }

        private void InvokeReturnToForward(float dotAngle)
        {
            foreach (IRotationTriggerListener listener in _listeners)
            {
                listener.InReturnToForward(dotAngle);
            }
        }

        private void LerpRotation()
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(NormalizedDesiredRotationForward, _playerTransform.MeshUp);

            float deltaVariation = RotationSpeedByAngle.Evaluate(Quaternion.Angle(currentRotation, targetRotation));

            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * deltaVariation);
        }

    }
}
