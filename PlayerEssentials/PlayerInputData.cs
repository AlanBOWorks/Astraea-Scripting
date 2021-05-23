using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerEssentials
{


    [Serializable]
    public class PlayerInputData : IInputLookAround, IInputMovement
    {
        [Title("Mouse - Data")]
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public Vector2 LookDeltaAxis { get; private set; }

        public void UpdateLookAtAxis(Vector2 set)
        {
            LookDeltaAxis = set;
        }

        [Title("Keyboard - Data")]
        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public Vector3 GlobalDesiredDirection { get; private set; }
        public void UpdateGlobalDirection(Vector3 direction)
        {
            GlobalDesiredDirection = direction;
        }

        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public bool IsMoving { get; private set; }

        public void UpdateIsMoving(bool set)
        {
            IsMoving = set;
        }

        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public bool IsSprintPress { get; private set; }

        public void UpdateIsSprintPress(bool set)
        {
            IsSprintPress = set;
        }

        [ShowInInspector, DisableInPlayMode, DisableInEditorMode]
        public Vector2 PlaneMovement { get; private set; }

        public void UpdateKeyboardDirection(Vector2 set)
        {
            PlaneMovement = set;
        }

        public void Reset()
        {
            LookDeltaAxis = Vector2.zero;
            GlobalDesiredDirection = Vector3.zero;
            IsSprintPress = false;
            IsMoving = false;
            PlaneMovement = Vector2.zero;
        }
    }

}
