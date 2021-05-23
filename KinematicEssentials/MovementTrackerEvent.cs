using System;
using System.Collections.Generic;
using MEC;
using SharedLibrary;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace KinematicEssentials
{
    public class MovementTrackerEvent : IMovementEventListener
    {

        [SuffixLabel("Seconds")]
        public SRange SmallChangeCheck = new SRange(.1f,.4f);
        [SuffixLabel("Seconds")]
        public SRange LongChangeCheck = new SRange(2f, 10f);


        private readonly List<IShortMovementEvent> _shortMovementEvents;
        private readonly List<ILongMovementEvent> _longMovementEvents;

        public void AddShortListener(IShortMovementEvent listener)
        {
            _shortMovementEvents.Add(listener);
        }

        public void AddLongListener(ILongMovementEvent listener)
        {
            _longMovementEvents.Add(listener);
        }

        public void AddListener(IMovementEventListener listener)
        {
            AddShortListener(listener);
            AddLongListener(listener);
        }


        [ShowInInspector,DisableInEditorMode,DisableInPlayMode]
        private IKinematicVelocity _trackingVelocity;
        public MovementTrackerEvent(IKinematicVelocity velocity, bool debugChanges = false)
        {
            _shortMovementEvents = new List<IShortMovementEvent>(4) {this};
            _longMovementEvents = new List<ILongMovementEvent>(4) {this};

#if UNITY_EDITOR
            if (debugChanges)
            {
                DebugMovementEvent debugMovementEvent = new DebugMovementEvent();
                _shortMovementEvents.Add(debugMovementEvent);
                _longMovementEvents.Add(debugMovementEvent);
            }
#endif          

            _trackingVelocity = velocity;
            InvokeSwitchToMoveState(); //this call is not done in the first Coroutine call, so this is done here instead
            _movementCheckHandle = Timing.RunCoroutine(_CheckForMovement());

        }


        private bool _isStopState;

        public bool Enabled
        {
            set
            {
                if (value)
                {
                    Timing.ResumeCoroutines(_movementCheckHandle);
                }
                else
                {
                    Timing.PauseCoroutines(_movementCheckHandle);
                }
            }
        }

        private void InvokeSwitchToMoveState()
        {
            foreach (IShortMovementEvent listener in _shortMovementEvents)
            {
                listener.SwitchToMoveState(_currentSpeed);
            }
        }

        private void InvokeSwitchToStopState()
        {
            foreach (IShortMovementEvent listener in _shortMovementEvents)
            {
                listener.SwitchToStopState(_currentSpeed);
            }
        }

        private const float SpeedThreshold = .001f;
        private CoroutineHandle _movementCheckHandle;
        private float _currentSpeed;
        private IEnumerator<float> _CheckForMovement()
        {
            while (_trackingVelocity != null)
            {
                float waitAmount = SmallChangeCheck.RandomInRange();
                yield return Timing.WaitForSeconds(waitAmount);
                _currentSpeed = _trackingVelocity.CurrentSpeed;
                _currentSpeed = SMaths.SMaths.Round(_currentSpeed, 1000);


                if (_isStopState)
                {
                    if (_currentSpeed <= SpeedThreshold) continue;
                    InvokeSwitchToMoveState();
                }
                else
                {
                    if (_currentSpeed > SpeedThreshold) continue;
                    InvokeSwitchToStopState();

                }

            }
        }

        public void SwitchToStopState(float speed)
        {
            _isStopState = true;
            InvokeLongCheck();
        }

        public void SwitchToMoveState(float speed)
        {
            _isStopState = false;
            InvokeLongCheck();
        }

        private void InvokeLongCheck()
        {
            Timing.KillCoroutines(_longCheckHandle);
            _longCheckHandle = Timing.RunCoroutine(_LongTimerCheck());
        }

        private CoroutineHandle _longCheckHandle;
        private IEnumerator<float> _LongTimerCheck()
        {
            float waitAmount = LongChangeCheck.RandomInRange();
            yield return Timing.WaitForSeconds(waitAmount);
            if (_isStopState)
            {
                foreach (ILongMovementEvent listener in _longMovementEvents)
                {
                    listener.OnLongStop(_currentSpeed);
                }
            }
            else
            {
                foreach (ILongMovementEvent listener in _longMovementEvents)
                {
                    listener.OnLongMovement(_currentSpeed);
                }
            }
        }

        public void OnLongMovement(float currentSpeed)
        {
        }

        public void OnLongStop(float currentSpeed)
        {
        }


#if UNITY_EDITOR

        private class DebugMovementEvent : IShortMovementEvent, ILongMovementEvent
        {
            public void SwitchToMoveState(float currentSpeed)
            {
                Debug.Log($"Switch to Move State : {currentSpeed}");
            }

            public void SwitchToStopState(float currentSpeed)
            {
                Debug.Log($"Switch to Stop State : {currentSpeed}");
            }

            public void OnLongMovement(float currentSpeed)
            {
                Debug.Log($"On Long Movement : {currentSpeed}");
            }

            public void OnLongStop(float currentSpeed)
            {
                Debug.Log($"On Long Stop : {currentSpeed}");
            }
        } 
#endif
    }


    public interface IMovementEventListener : IShortMovementEvent, ILongMovementEvent { }
    public interface IShortMovementEvent
    {
        void SwitchToMoveState(float currentSpeed);
        void SwitchToStopState(float currentSpeed);
    }

    public interface ILongMovementEvent
    {
        void OnLongMovement(float currentSpeed);
        void OnLongStop(float currentSpeed);
    }
}
