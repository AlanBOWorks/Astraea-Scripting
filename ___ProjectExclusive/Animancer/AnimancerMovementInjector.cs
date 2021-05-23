using System;
using System.Collections.Generic;
using Animancer;
using KinematicEssentials;
using SharedLibrary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AnimancerEssentials
{
    [Serializable]
    public class AnimancerMovementInjector : ITicker
    {

        [SerializeField] private AnimationCurve _animationSpeedByVelocity 
            = new AnimationCurve(
                new Keyframe(0,0.5f),
                new Keyframe(.3f,1f), 
                new Keyframe(2f,1.5f));

        [ShowInInspector,HideInEditorMode,DisableInPlayMode]
        private IKinematicVelocity _velocity;

        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        private List<MixerState<Vector2>> _movementAnimations = new List<MixerState<Vector2>>(2);


        public void Injection(IKinematicVelocity velocity)
        {
            _velocity = velocity;
        }

        public void AddMixer(MixerState<Vector2> item)
        {
            _movementAnimations.Add(item);
        }

        public void AddMixer(MixerState.Transition2D transition)
        {
            AddMixer(transition.State);
        }

        public void RemoveMixer(MixerState<Vector2> item)
        {
            _movementAnimations.Remove(item);
        }

        public void RemoveMixer(MixerState.Transition2D transition)
        {
            RemoveMixer(transition.State);
        }

        public Vector2 LocalMovement { get; private set; }
        public bool Disabled { get; set; }
        public void Tick()
        {
            for (int i = 0; i < _movementAnimations.Count; i++)
            {
                LocalMovement = _velocity.CurrentLocalPlanarVelocity;
                float animationSpeed = _animationSpeedByVelocity.Evaluate(_velocity.CurrentSpeed);

                MixerState<Vector2> state = _movementAnimations[i];
                if (Math.Abs(animationSpeed) < .001f)
                {
                    state.IsPlaying = false;
                }
                else
                {
                    state.IsPlaying = true;
                    state.Parameter = LocalMovement;
                    state.Speed = animationSpeed;
                }
            }
        }

        public void SwitchTimer(float normalizedTime)
        {
            foreach (MixerState<Vector2> state in _movementAnimations)
            {
                state.NormalizedTime = normalizedTime;
            }
        }
    }
}
