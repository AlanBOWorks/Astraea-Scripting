using IKEssentials;
using KinematicEssentials;
using Sirenix.OdinInspector;
using UnityEngine;
using Utility;

namespace Blanca
{
    [CreateAssetMenu(fileName = "Blanca Parameters [Variable]",
        menuName = "Variable/Blanca/Parameter")]
    public class BlancaParametersVariable : ScriptableObject, 
         IMainHandSetter, IKinematicDeltaVariation, ICharacterGravity
    {
        [TabGroup("Hands")]
        [SerializeField]
        private bool _isLeftHandMain = true;
        public bool IsLeftMainHand
        {
            get => _isLeftHandMain;
            set => _isLeftHandMain = value;
        }

        [TabGroup("Kinematic")]
        [SerializeField] private Vector3 _gravity = new Vector3(0, -9.81f, 0);
        [TabGroup("Kinematic")]
        [SerializeField] private bool _canMoveOnAir = true;
        public Vector3 Gravity { get => _gravity; set => _gravity = value; }
        public bool CanMoveOnAir { get => _canMoveOnAir; set => _canMoveOnAir = value; }



        [TabGroup("Kinematic")]
        [SerializeField] private BreakableAcceleration _defaultDeltaAcceleration = new BreakableAcceleration(2,12);
        public BreakableAcceleration DeltaAcceleration
        {
            get => _defaultDeltaAcceleration;
            set => _defaultDeltaAcceleration = value;
        }

        [TabGroup("Kinematic")]
        [SerializeField] private float _defaultAngularSpeed = 4f;
        public float AngularSpeed
        {
            get => _defaultAngularSpeed;
            set => _defaultAngularSpeed = value;
        }

    }
}
