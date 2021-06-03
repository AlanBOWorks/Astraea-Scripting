using System.Collections.Generic;
using KinematicEssentials;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IKEssentials
{
    //Uses MonoBehaviour because it requires RayCast and transforms for IK
    public class IKFeetHandler : MonoBehaviour, IMovementEventListener, IRotationTriggerListener
    {
        [SerializeField,HideInPlayMode] private FullBodyBipedIK biped = null;

        private FootHandler _leftFoot;
        private FootHandler _rightFoot;
        private FeetHandler _feet;


        private void Awake()
        {
            _leftFoot = new FootHandler(biped.solver.leftFootEffector,biped.references.leftFoot);
            _rightFoot = new FootHandler(biped.solver.rightFootEffector, biped.references.rightFoot);
            _feet = new FeetHandler(_leftFoot,_rightFoot);

            _isMoving = false;
        }

        private bool _isMoving;
        public void SwitchToMoveState(float currentSpeed)
        {
            _isMoving = true;
            _feet.UnPin();
        }

        public void SwitchToStopState(float currentSpeed)
        {
            _isMoving = false;
            _feet.Pin();
        }

        public void OnLongMovement(float currentSpeed)
        {
        }

        public void OnLongStop(float currentSpeed)
        {
            //TODO Do feet animation of reposition
        }


        public void InLargeAngle(float dotAngle)
        {
            if(_isMoving) return;
            _feet.UnPin();
        }

        public void InReturnToForward(float dotAngle)
        {
            if(_isMoving) return;
            _feet.Pin();
        }

        private void Update()
        {
            _feet.HandleIK();
        }

        internal struct FeetHandler
        {
            internal FootHandler LeftFoot;
            internal FootHandler RightFoot;

            public FeetHandler(FootHandler left, FootHandler right)
            {
                LeftFoot = left;
                RightFoot = right;
            }

            public void Pin()
            {
                LeftFoot.Pin();
                RightFoot.Pin();
            }

            public void UnPin()
            {
                LeftFoot.UnPin();
                RightFoot.UnPin();
            }

            public void HandleIK()
            {
                LeftFoot.HandleIK();
                RightFoot.HandleIK();
            }
        }

        internal class FootHandler
        {
            public FootHandler(IKEffector effector,Transform bone)
            {
                _effector = effector;
                _bone = bone;
                _pinner = new PinFoot(bone);
                _weight = new WeightFoot(effector);
            }

            private readonly IKEffector _effector;
            private readonly Transform _bone;
            private readonly PinFoot _pinner;
            private readonly WeightFoot _weight;

            public void HandleIK()
            {
                Vector3 currentPosition = _bone.position;
                //TODO do the queue when there's more filters

                _effector.position = _pinner.CalculatePosition(currentPosition);

                Quaternion currentRotation = _bone.rotation;
                _effector.rotation = _pinner.CalculateRotation(currentRotation);
            }

            public void Pin()
            {
                _pinner.Pin();
                _weight.Full(1);
            }

            public void UnPin()
            {
                _pinner.UnPin();
                _weight.Full(0);
            }
        }

        internal class WeightFoot
        {
            private readonly IKEffector _effector;
            public WeightFoot(IKEffector foot)
            {
                _effector = foot;
            }

            public void Full(float targetWeight)
            {
                _effector.positionWeight = targetWeight;
                _effector.rotationWeight = targetWeight;
            }

            public void Position(float weight) => _effector.positionWeight = weight;
            public void Rotation(float weight) => _effector.rotationWeight = weight;
        }

        internal class PinFoot : IFeetIKFilter
        {
            private readonly Transform _bone;

            public PinFoot( Transform bone)
            {
                _bone = bone;
                _pinned = false;
                UpdateForPin();
            }


            private bool _pinned;
            private Vector3 _pinnedPosition;
            private Quaternion _pinnedRotation;

            private void UpdateForPin()
            {
                _pinnedPosition = _bone.position;
                _pinnedRotation = _bone.rotation;
            }

            public void Pin()
            {
                UpdateForPin();
                _pinned = true;
            }

            public void UnPin()
            {
                _pinned = false;
            }

            public Vector3 CalculatePosition(Vector3 currentPosition)
            {
                return _pinned
                    ? _pinnedPosition
                    : currentPosition;
            }

            public Quaternion CalculateRotation(Quaternion currentRotation)
            {
                return _pinned
                    ? _pinnedRotation
                    : currentRotation;
            }
        }
        private class RepositionOnStop : IFeetIKFilter
        {



            public Vector3 CalculatePosition(Vector3 currentPosition)
            {
                throw new System.NotImplementedException();
            }

            public Quaternion CalculateRotation(Quaternion currentRotation)
            {
                throw new System.NotImplementedException();
            }
        }

    }

    public interface IFeetIKFilter : IFeetPositionFilter, IFeetRotationFilter
    {}

    public interface IFeetPositionFilter
    {
        Vector3 CalculatePosition(Vector3 currentPosition);
    }

    public interface IFeetRotationFilter
    {
        Quaternion CalculateRotation(Quaternion currentRotation);

    }
}
