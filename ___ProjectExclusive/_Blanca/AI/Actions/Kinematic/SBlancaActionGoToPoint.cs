using System;
using System.Collections.Generic;
using AIEssentials;
using KinematicEssentials;
using MEC;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Blanca.AIEssentials
{
    [CreateAssetMenu(fileName = "Go to Point - N [Blanca Action]",
        menuName = "AI/Blanca/Action/Go to Point")]
    public class SBlancaActionGoToPoint : ScriptableObject, IAction
    {
        public BreakableAcceleration Acceleration = new BreakableAcceleration(4, 12);
        public AnimationCurve SpeedByDistance = new AnimationCurve(
            new Keyframe(0, 0), new Keyframe(2, 1.5f));
        [InfoBox("If speed is lower than 0: will be ignored")]
        public float AngularSpeed = 4f;



        public IEnumerator<float> _DoAction(IPathCalculator pathCalculator, Func<Vector3> onPoint, float distanceThreshold)
        {
            IPathCalculator mainPathCalculator = pathCalculator;
            IKinematicMotorHandler motorHandler = BlancaUtilsKinematic.Motor;

            yield return Timing.WaitUntilDone(mainPathCalculator.RefreshSearch(onPoint()));
            do
            {
                mainPathCalculator.SetDestination(onPoint());
                Vector3 velocity = mainPathCalculator.DesiredVelocity();
                Vector3 rotationDirection = velocity;
                velocity *= SpeedByDistance.Evaluate(mainPathCalculator.GetRemainingDistance());


                float velocityDelta =
                    Time.deltaTime * Acceleration.DeltaModifier(motorHandler.DesiredVelocity, velocity);

                if (AngularSpeed > 0)
                {
                    float rotationDelta =
                        Time.deltaTime * AngularSpeed;
                    motorHandler.DesiredRotationForward
                        = Vector3.Slerp(motorHandler.CurrentRotationForward, rotationDirection, rotationDelta);
                }

                motorHandler.DesiredVelocity 
                    = Vector3.Lerp(motorHandler.DesiredVelocity, velocity, velocityDelta);
               
                yield return Timing.DeltaTime;
            } while (mainPathCalculator.GetRemainingDistance() > distanceThreshold);
        }

        public IEnumerator<float> _DoAction(Func<Vector3> onPoint, float distanceThreshold)
        {
            return _DoAction(BlancaUtilsKinematic.MainPathCalculator, onPoint, distanceThreshold);
        }

        private Func<Vector3> _onTransformFunc;
        public IEnumerator<float> _DoAction(IPathCalculator pathCalculator, Transform onFormation,
            Func<Vector3> localFormationPoint, float distanceThreshold)
        {
            _onTransformFunc = FinalPoint;
            return _DoAction(pathCalculator, _onTransformFunc, distanceThreshold);
            Vector3 FinalPoint()
            {
                return onFormation.TransformPoint(localFormationPoint());
            }
        }

        private readonly Func<Vector3> _onPlayerFunc = PlayerUtilsTransform.GetCurrentPosition;
        public IEnumerator<float> _DoActionOnPlayer(IPathCalculator pathCalculator, float distanceThreshold)
        {
            return _DoAction(pathCalculator, _onPlayerFunc, distanceThreshold);
        }

        public IEnumerator<float> _DoActionOnPlayer
            (IPathCalculator pathCalculator, Func<Vector3> localFormationPoint, float distanceThreshold)
        {
            return _DoAction(pathCalculator, PlayerUtilsTransform.GetTransformData().MeshRoot,
                localFormationPoint, distanceThreshold);
        }

    }
}
