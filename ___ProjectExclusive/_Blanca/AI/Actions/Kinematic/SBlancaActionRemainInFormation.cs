using System;
using System.Collections.Generic;
using AIEssentials;
using Blanca;
using Companion;
using KinematicEssentials;
using MEC;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "Remain in Formation - N [Blanca Action]",
    menuName = "AI/Blanca/Action/Remain in Formation")]
public class SBlancaActionRemainInFormation : ScriptableObject,IAction
{
    [Title("Positioning")] 
    [SerializeField, SuffixLabel("%/meter")]
    private AnimationCurve _pathSpeedByDistance = new AnimationCurve(
        new Keyframe(0, 0), new Keyframe(1, 1));

    [SerializeField]
    private Vector2 _formationPosition = new Vector2(.3f,.1f);
    [SerializeField, SuffixLabel("%/meter")]
    private AnimationCurve _copyPlayerByDistance = new AnimationCurve(
            new Keyframe(0,1),new Keyframe(.3f,1.5f), 
            new Keyframe(1,0));
    [Range(0f,3f)]
    public float PositionThreshold = .1f;

    [Title("Rotation")] 
    [SerializeField, SuffixLabel("%/meter")]
    private AnimationCurve _copyPlayerBySpeed = new AnimationCurve(
        new Keyframe(.5f,1), new Keyframe(1.3f,.2f));


    [Title("Smoothing")]
    public BreakableAcceleration Acceleration = new BreakableAcceleration(2f,12f);
    public float AngularSpeed = 2f;


    public Vector2 GetFormationPosition() => _formationPosition;

    public IEnumerator<float> _DoAction(IPathCalculator pathCalculator, Func<bool> breakConditional)
    {
        while (breakConditional())
        {
            pathCalculator.SetDestination(GetFormationPoint());

            IKinematicMotorHandler motor = BlancaUtilsKinematic.Motor;
            KinematicData playerKinematicData = PlayerUtilsKinematic.GetPlayerKinematicData();

            Vector3 blancaSmallForward = BlancaUtilsTransform.CharacterTransform.MeshForward * 0.1f;

            Vector3 copyVelocity = playerKinematicData.CurrentVelocity;
            Vector3 copyRotation = PlayerUtilsTransform.GetTransformData().MeshForward;

            float separationDistance = CompanionUtils.DirectDistanceOfSeparation;
            float velocityModifier = _copyPlayerByDistance.Evaluate(separationDistance);

            Vector3 pathVelocity = pathCalculator.DesiredVelocity();
            pathVelocity *= _pathSpeedByDistance.Evaluate(separationDistance);

            Vector3 finalVelocity = pathVelocity + copyVelocity * velocityModifier;


            float rotationWeight = _copyPlayerBySpeed.Evaluate(finalVelocity.magnitude);
            Vector3 finalRotation = Vector3.SlerpUnclamped(finalVelocity + blancaSmallForward, copyRotation, rotationWeight);

            float deltaAcceleration = Acceleration.DeltaModifier(motor.DesiredVelocity, finalVelocity);


            motor.DoVelocityLerp(finalVelocity, deltaAcceleration);
            motor.DoRotationSlerp(finalRotation,AngularSpeed);

            yield return Timing.DeltaTime;
        }


        Vector3 GetFormationPoint()
        {
            Vector3 localPoint = new Vector3(
                _formationPosition.x,
                0,
                _formationPosition.y);
            Vector3 finalPoint = PlayerUtilsTransform.GetTransformData().MeshRoot.TransformPoint(localPoint);
            finalPoint = Vector3.MoveTowards(
                BlancaUtilsTransform.CharacterTransform.MeshWorldPosition,
                finalPoint,
                PositionThreshold);
            return finalPoint;
        }
    }

}
