using KinematicEssentials;
using PlayerEssentials;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Player
{
    [CreateAssetMenu(fileName = "Player Parameters [Variable]",
        menuName = "Variable/Player/Parameter")]
    public class PlayerParametersVariable : ScriptableObject,
        ICameraParameters, IKinematicDeltaVariation, ICharacterGravity
    {
        [Title("Motor Parameters")]
        [SerializeField] private Vector3 _gravity = new Vector3(0, -9.81f, 0);
        [SerializeField] private bool _canMoveOnAir = true;
        public Vector3 Gravity { get => _gravity; set => _gravity = value; }
        public bool CanMoveOnAir { get => _canMoveOnAir; set => _canMoveOnAir = value; }
       

        [Title("Kinematic Variation")]
        [SerializeField]
        private BreakableAcceleration _defaultAcceleration = new BreakableAcceleration(2, 12);
        public BreakableAcceleration DeltaAcceleration
        {
            get => _defaultAcceleration;
            set => _defaultAcceleration = value;
        }

        [SerializeField] private float _defaultAngularSpeed = 4f;
        public float AngularSpeed
        {
            get => _defaultAngularSpeed;
            set => _defaultAngularSpeed = value;
        }


        [HideInPlayMode]
        public float SprintModifier = 1.5f;

        [Title("Camera Parameters")]
        public AnimationCurve AngleDelta
            = new AnimationCurve(new Keyframe(0, 4), new Keyframe(15, 8), new Keyframe(65, 8), new Keyframe(90, 4));
        public float CalculateAngleDelta(float currentAngle)
        {
            return AngleDelta.Evaluate(currentAngle);
        }
    }
}
