using Animancer;
using KinematicEssentials;
using SMaths;
using UnityEngine;

namespace AnimancerEssentials
{
    public class MovementStatesHandler : IMovementEventListener
    {
        private AnimancerBaseStatesHandler _baseStatesHandler;

        private float _leftLegStart;
        public float LeftLegStartsAt
        {
            get => _leftLegStart;
            set
            {
                _leftLegStart = value;
                _leftStartIsSmall = _leftLegStart < .5f;
            }
        }

        private bool _leftStartIsSmall;

        public MovementStatesHandler(AnimancerBaseStatesHandler baseStates, float leftLegStartsAt = 0)
        {
            _baseStatesHandler = baseStates;
            LeftLegStartsAt = leftLegStartsAt;

            _leftStartIsSmall = LeftLegStartsAt < .5f;
        }

        public void SwitchToMoveState(float currentSpeed)
        {
            _baseStatesHandler.ChangeWeight(1);
        }

        public void SwitchToStopState(float currentSpeed)
        {
            _baseStatesHandler.ChangeWeight(0);
        }

        public void OnLongMovement(float currentSpeed)
        {
            
        }

        public void OnLongStop(float currentSpeed)
        {
            float currentTime = _baseStatesHandler.MovementState.NormalizedTime % 1;
            float rangeStartingTime = LeftLegStartsAt;
            if (_leftStartIsSmall)
                rangeStartingTime += .5f; //to avoid negative; the XOR will fix this offset
            SRange leftTimeRang = new SRange(rangeStartingTime- .25f, rangeStartingTime+ .25f);

            float targetTime;
            if (_leftStartIsSmall ^ leftTimeRang.IsInRange(currentTime))
            {
                targetTime = LeftLegStartsAt;
            }
            else
            {
                targetTime = LeftLegStartsAt + .5f; //set to right leg
            }

            _baseStatesHandler.MovementState.NormalizedTime = targetTime;
        }
    }
}
