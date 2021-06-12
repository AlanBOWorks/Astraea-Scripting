using System;
using System.Collections.Generic;
using AIEssentials;
using Companion;
using IKEssentials;
using KinematicEssentials;
using MEC;
using Player;
using PlayerEssentials;
using SharedLibrary;
using UnityEngine;

namespace Blanca
{
   
    /// <summary>
    /// Used for calculations and similar; Should be used after Initialization (character's Awake())
    /// </summary>
    public static class BlancaUtilsTransform
    {
        public static ICharacterTransformData CharacterTransform = null;

        public static Vector3 GetCharacterPosition() => CharacterTransform.MeshWorldPosition;


    }

    /// <summary>
    /// <inheritdoc cref="BlancaUtilsTransform"/>
    /// </summary>
    public static class BlancaUtilsKinematic
    {
        public static IKinematicVelocity KinematicVelocity;
        public static IKinematicRotation KinematicRotation;
        public static IKinematicMotorHandler Motor;
        public static BlancaVelocityControlHolder VelocityControls;
        public static BlancaRotationControlHolder RotationControls;
        public static SerializedBlancaPathControls PathControls;

        public static IEnumerator<float> _SearchByThresholdOverride(IPathCalculator pathCalculator, Vector3 onPoint, float distanceThreshold)
        {
            pathCalculator.SetReachDestinationDistance(distanceThreshold);
            return pathCalculator.RefreshSearch(onPoint);
        }

        public static void SearchByThresholdOverride(IPathCalculator pathCalculator, Vector3 onPoint,
            float distanceThreshold)
        {
            pathCalculator.SetReachDestinationDistance(distanceThreshold);
            pathCalculator.RefreshSearch(onPoint);
        }

        public static bool ClosePathCheck(IPathCalculator pathCalculator, float distanceThreshold = .2f)
        {
            return pathCalculator.IsCloseEnough(distanceThreshold);
        }

        public static Vector3 PointOfDesiredVelocity(IPathCalculator pathCalculator, float distanceModifier = 1)
        {
            return pathCalculator.DesiredVelocity() * distanceModifier +
                   BlancaUtilsTransform.CharacterTransform.MeshWorldPosition;
        }

        public static Vector3 PointOfDesiredVelocityInMovement
            (IPathCalculator pathCalculator, float distanceModifier = 1, float velocityWeight = 1)
        {
            return PointOfDesiredVelocity(pathCalculator, distanceModifier)
                   + Vector3.LerpUnclamped(Vector3.zero, KinematicVelocity.CurrentVelocity, velocityWeight);
        }
    }

    /// <summary>
    /// <inheritdoc cref="BlancaUtilsTransform"/>
    /// </summary>
    public static class BlancaUtilsIK
    {
        public static HumanoidIKSolver HumanoidIkSolver = null;
        public static BlancaLookAtControlHolder LookAtControls = null;
        public static HeadLookAtSolverBase HeadIkSolver => HumanoidIkSolver.HeadIkSolver;
        private static ICharacterTransformData CharacterTransform => BlancaUtilsTransform.CharacterTransform;
        public static IKinematicVelocity Velocity => BlancaUtilsKinematic.KinematicVelocity;

        private static IPlayerTransformData PlayerTransform => PlayerUtilsTransform.TransformData;
        public static bool IsLeftHanded() => BlancaEntitySingleton.Instance.Parameters.IsLeftMainHand;
        

        public static void SetLookPoint(Vector3 targetPoint)
        {
            HeadIkSolver.SetTarget(targetPoint);
        }

        public static void LookAtPoint(Vector3 targetPoint, float targetWeight = 1)
        {
            IIKSolver headIkSolver = HeadIkSolver;
            headIkSolver.SetTarget(targetPoint);
            headIkSolver.SetWeight(targetWeight);
        }

        public static void LookAtDirection(Vector3 globalDirection, float targetWeight = 1)
        {
            Vector3 lookPosition = CharacterTransform.HeadPosition
                                   + CharacterTransform.MeshForward * .1f;
            lookPosition += globalDirection;
            LookAtPoint(lookPosition,targetWeight);
        }

        public static Vector3 CalculateLookAtVelocity(float velocityVectorModifier = 5f)
        {
            Vector3 lookPosition = CharacterTransform.HeadPosition
                                   + CharacterTransform.MeshForward * .1f; //Small forward to avoid clipping

            lookPosition += Velocity.CurrentVelocity * velocityVectorModifier;
            return lookPosition;
        }

        public static void LookAtVelocity(float velocityModifier = 5f, float targetWeight = 1)
        {
            LookAtPoint(CalculateLookAtVelocity(velocityModifier), targetWeight);
        }

        public static void SetLookAtPlayer(float targetWeight)
        {
            LookAtPoint(PlayerTransform.HeadPosition, targetWeight);
        }

        public static void SetLookAtPlayer()
        {
            SetLookPoint(PlayerTransform.HeadPosition);
        }

        public static Vector3 LookAtPlayerPoint()
        {
            return PlayerTransform.HeadPosition;
        }



        public static IEnumerator<float> LookAtVelocityTask(Func<bool> loopCondition, float velocityModifier = 4f, float targetWeight= 1f)
        {
            while (loopCondition())
            {
                LookAtVelocity(velocityModifier,targetWeight);
                yield return Timing.DeltaTime;
            }
        }

        public static IEnumerator<float> LookAtPathTask(IPathCalculator pathCalculator, Func<bool> loopCondition,
            float velocityModifier = 4f, float targetWeight = 1f)
        {
            while (loopCondition())
            {
                LookAtDirection(pathCalculator.DesiredVelocity() * velocityModifier, targetWeight);
                yield return Timing.DeltaTime;
            }
        }

    }

}
