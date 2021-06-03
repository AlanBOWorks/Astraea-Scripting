using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KinematicEssentials
{
    public class RotationTrackerEvent : IRotationTriggerHolder
    {
        [ShowInInspector, DisableInEditorMode, DisableInPlayMode]
        private IKinematicNormalizedRotation _trackingRotation;

        [Title("Params")]
        [Range(-1, 1)] public float DotAngleThreshold;

        private readonly List<IRotationTriggerListener> _listeners;
        public void AddListener(IRotationTriggerListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(IRotationTriggerListener listener)
        {
            _listeners.Remove(listener);
        }

        private const int PredictedAmountOfListeners = 4;
        public RotationTrackerEvent(IKinematicNormalizedRotation trackingRotation, float dotAngleThreshold = 0)
        {
            _listeners = new List<IRotationTriggerListener>(PredictedAmountOfListeners);
            _trackingRotation = trackingRotation;
            DotAngleThreshold = dotAngleThreshold;

            /*if (true) //debug
            {
                Listeners.Add(new DebugRotationEvent());
            }*/

            _angleHandle = Timing.RunCoroutine(_CheckForAngle());
        }

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Timing.ResumeCoroutines(_angleHandle);
                }
                else
                {
                    Timing.PauseCoroutines(_angleHandle);
                }
            }
        }

        private bool _isCheckingForLong = true;
        private readonly CoroutineHandle _angleHandle;
        private IEnumerator<float> _CheckForAngle()
        {
            yield return Timing.WaitForOneFrame; //This is to avoid Forward.Zero problem
            while (_trackingRotation != null)
            {
                UpdateForward();
                float angle;
                UpdateAngle();
                if (_isCheckingForLong)
                {
                    while (angle > DotAngleThreshold)
                    {
                        yield return Timing.WaitForOneFrame;
                        UpdateAngle();
                    }

                    _isCheckingForLong = false;
                    InvokeLargeAngleEvent(angle);
                }
                else
                {
                    while (angle < .85f)
                    {
                        yield return Timing.WaitForOneFrame;
                        UpdateAngle();
                    }

                    _isCheckingForLong = true;
                    InvokeReturnToForward(angle);
                }

                void UpdateAngle()
                {
                    angle = DotAngleValue();
                }
            }
        }

        private void UpdateForward()
        {
            _lastForwardCheck = _trackingRotation.NormalizedCurrentRotationForward;
        }

        private Vector3 _lastForwardCheck;
        private float DotAngleValue()
        {
            return Vector3.Dot(
                _lastForwardCheck,
                _trackingRotation.NormalizedCurrentRotationForward);
        }

        private void InvokeLargeAngleEvent(float dotAngle)
        {
            foreach (IRotationTriggerListener listener in _listeners)
            {
                listener.InLargeAngle(dotAngle);
            }
        }

        private void InvokeReturnToForward(float angle)
        {
            foreach (IRotationTriggerListener listener in _listeners)
            {
                listener.InReturnToForward(angle);
            }
        }

#if UNITY_EDITOR
        private class DebugRotationEvent : IRotationTriggerListener
        {
            public void InLargeAngle(float dotAngle)
            {
                Debug.Log($"On long Rotation: {dotAngle}");
            }

            public void InReturnToForward(float dotAngle)
            {
                Debug.Log($"On short Rotation: {dotAngle}");
            }
        } 
#endif

    }


    public interface IRotationTriggerHolder
    {
        void AddListener(IRotationTriggerListener listener);
        void RemoveListener(IRotationTriggerListener listener);
    }

    public interface IRotationTriggerListener
    {
        void InLargeAngle(float dotAngle);
        void InReturnToForward(float dotAngle);
    }
}
