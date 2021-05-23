using System;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using DG.Tweening.Core;
using MEC;
using UnityEngine;

namespace AnimancerEssentials
{
    public class AnimancerBaseStatesHandler
    {
        public AnimancerState IdleState { get; private set; }
        public AnimancerState MovementState { get; private set; }
        public AnimancerState CurrentState { get; private set; }

        public AnimancerBaseStatesHandler(AnimancerMovementStates onStart)
        {
            ChangeStates(onStart,0,.1f);

        }

        public void ChangeStates(AnimancerMovementStates target, float movementWeight, float fadeDuration = .25f)
        {
            IdleState = target.Idle;
            MovementState = target.Movement;
            ChangeWeight(movementWeight,fadeDuration);
        }
       
        private const float MinWeight = 0.01f;//0 will reset the animation
        public void ChangeWeight(float movementWeight, float fadeDuration = .25f)
        {
            CurrentState = movementWeight > .5f ? MovementState : IdleState;
            if (movementWeight < MinWeight) movementWeight = MinWeight;

            float idleWeight = 1 - movementWeight;

            IdleState.StartFade(idleWeight,fadeDuration);
            MovementState.StartFade(movementWeight,fadeDuration);
        }

    }

    public struct AnimancerMovementStates
    {
        public AnimancerState Idle;
        public AnimancerState Movement;

        public AnimancerMovementStates(AnimancerState idle,AnimancerState movement)
        {
            Idle = idle;
            Movement = movement;
        }

        public AnimancerMovementStates(AnimationClip idle,ITransition movement, AnimancerLayer targetLayer)
        {
            Idle = targetLayer.GetOrCreateState(idle);
            Movement = targetLayer.GetOrCreateState(movement);
        }
    }
}
