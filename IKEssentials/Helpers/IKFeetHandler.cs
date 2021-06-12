using System;
using System.Collections.Generic;
using KinematicEssentials;
using MEC;
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
            Transform root = biped.references.pelvis.root;

            Transform leftFoot = biped.references.leftFoot;
            _leftFoot = new FootHandler(
                biped.solver.leftFootEffector,
                leftFoot,
                leftFoot.GetChild(0),
                root);

            Transform rightFoot = biped.references.rightFoot;
            _rightFoot = new FootHandler(
                biped.solver.rightFootEffector,
                rightFoot,
                rightFoot.GetChild(0),
                root);


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


        public void InLargeAngle(float dotAngle,bool isRight)
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
            public FootHandler(IKEffector effector,Transform bone, Transform toe, Transform rootReference)
            {
                _effector = effector;
                _bone = bone;
                _pinner = new PinFoot(bone);
                _weight = new WeightFoot(effector);

                _filters = new IFeetIKFilter[]
                {
                    _pinner,
                    new ProjectOnFloor(rootReference)
                };
            }

            private readonly IKEffector _effector;
            private readonly Transform _bone;
            private readonly PinFoot _pinner;
            private readonly WeightFoot _weight;

            private readonly IFeetIKFilter[] _filters;

            public void HandleIK()
            {
                Vector3 currentPosition = _bone.position;
                Vector3 filterPosition = currentPosition;

                Quaternion currentRotation = _bone.rotation;
                Quaternion filterRotation = currentRotation;
                
                foreach (IFeetIKFilter filter in _filters)
                {
                    filter.CalculatePosition(ref filterPosition, currentPosition);
                    filter.CalculateRotation(ref filterRotation,currentRotation);
                }

                _effector.position = filterPosition;
                _effector.rotation = filterRotation;

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

            public void CalculatePosition(ref Vector3 lastFilter, Vector3 currentPosition)
            {
                if (_pinned)
                    lastFilter = _pinnedPosition;
            }

            public void CalculateRotation(ref Quaternion lastFilter, Quaternion currentRotation)
            {
                if (_pinned)
                    lastFilter = _pinnedRotation;
            }
        }
        
        internal class ProjectOnFloor : IFeetIKFilter
        {
            private readonly Transform _rootReference;

            public ProjectOnFloor(Transform rootReference)
            {
                _rootReference = rootReference;
            }


            private void ProjectOnPlane(ref Vector3 lastFilter)
            {

                lastFilter.y = _rootReference.position.y;
            }

            public void CalculatePosition(ref Vector3 lastFilter, Vector3 currentPosition)
            {
                ProjectOnPlane(ref lastFilter);
            }


            public void CalculateRotation(ref Quaternion lastFilter, Quaternion currentRotation)
            {
                
            }
        }

        internal class SmoothFilter : IFeetIKFilter
        {

            private const float StepPerTick = .8f;
            public void CalculatePosition(ref Vector3 lastFilter, Vector3 currentPosition)
            {
                lastFilter = Vector3.Lerp(currentPosition,lastFilter,StepPerTick);
            }

            public void CalculateRotation(ref Quaternion lastFilter, Quaternion currentRotation)
            {
                lastFilter = Quaternion.Slerp(currentRotation,lastFilter, StepPerTick);
            }
        }
    }

    public interface IFeetIKFilter : IFeetPositionFilter, IFeetRotationFilter
    {}

    public interface IFeetPositionFilter
    {
        void CalculatePosition(ref Vector3 lastFilter, Vector3 currentPosition);
    }

    public interface IFeetRotationFilter
    {
        void CalculateRotation(ref Quaternion lastFilter, Quaternion currentRotation);

    }
}
