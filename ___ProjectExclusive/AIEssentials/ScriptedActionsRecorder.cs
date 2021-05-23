using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace AIEssentials
{
    public class ScriptedActionsRecorder : MonoBehaviour
    {

        [Title("Setting up - Local")]
        [HideInPlayMode]
        public Camera ReferenceCamera;
        [ShowInInspector, HideInEditorMode]
        private Transform _cameraTransform;
        public Transform PositionReference;
        [SerializeField,HideInPlayMode] 
        private SPlayerLikeScriptedAction _action = null;

        [Title("Setting up - Global")]
        [SerializeField, HideInPlayMode] private InputActionReference _addPoint;
        [SerializeField, HideInPlayMode] private InputActionReference _removePoint;
        [SerializeField, HideInPlayMode] private InputActionReference _updateCurrent;
        [SerializeField, HideInPlayMode] private InputActionReference _indexVariation;


        [SerializeField, HideInPlayMode] private InputActionReference _variateLookAt;
        [SerializeField, HideInPlayMode] private InputActionReference _variateDistance;


        [SerializeField, HideInPlayMode]
        private InputActionReference[] _scriptableActionMaps = new InputActionReference[3];
        [SerializeField, HideInPlayMode]
        private ScriptedAction[] _scriptedActions = new ScriptedAction[3];

        [ShowInInspector,HideInEditorMode, InfoBox("Can take a little time for updating")]
        private List<PlayerLikeScriptedActions.ScriptedNode> _scriptedNodes;
        private void Awake()
        {
            if(_action is null)
                throw new NullReferenceException("There's none Action Scriptable Object");
            _action.OnScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(SceneManager.GetActiveScene().path);

            _scriptedNodes = _action.Actions.GetActions();
            _cameraTransform = ReferenceCamera.transform;
        }

        private void Start()
        {
            ReferenceCamera = null;

            _addPoint.action.performed += AddPoint;
            _removePoint.action.performed += RemovePoint;
            _updateCurrent.action.performed += UpdateCurrent;
            _indexVariation.action.performed += VariateIndex;

            _variateLookAt.action.performed += VariateLookAtDistance;
            _variateDistance.action.performed += VariateDistance;

            _scriptableActionMaps[0].action.performed += InsertActionOne;
            _scriptableActionMaps[1].action.performed += InsertActionTwo;
            _scriptableActionMaps[2].action.performed += InsertActionThree;


            ListIndex = _scriptedNodes.Count - 1;
        }


        [Title("Distance Params")]
        public float DistanceVariationOnPress = .1f;
        [HideInEditorMode] 
        public float CurrentDistanceThreshold = .2f;
        [Title("LookAt Params")]
        public float ScrollVariation = 0.1f;
        [HideInEditorMode]
        public float CameraForwardModifier = 4f;

        [HideInEditorMode, GUIColor(.3f,.4f, .8f),BoxGroup]
        public int ListIndex;

        

        private void AddPoint(InputAction.CallbackContext context)
        {
            Vector3 onPoint = PositionReference.position;
            Vector3 lookAtPoint = _cameraTransform.position + _cameraTransform.forward * CameraForwardModifier;
            PlayerLikeScriptedActions.ScriptedNode node =
                new PlayerLikeScriptedActions.ScriptedNode(onPoint, lookAtPoint, CurrentDistanceThreshold);
            _scriptedNodes.Insert(ListIndex, node);
        }

        private void RemovePoint(InputAction.CallbackContext context)
        {
            _scriptedNodes.RemoveAt(ListIndex);
        }

        private void UpdateCurrent(InputAction.CallbackContext context)
        {
            PlayerLikeScriptedActions.ScriptedNode current = _scriptedNodes[ListIndex];
            Vector3 onPoint = PositionReference.position;
            Vector3 lookAtPoint = _cameraTransform.position + _cameraTransform.forward * CameraForwardModifier;
            current.TranslationPoint = onPoint;
            current.LookAtPoint = lookAtPoint;
            current.DistanceThreshold = CurrentDistanceThreshold;

            _scriptedNodes[ListIndex] = current;
        }

        private void VariateIndex(InputAction.CallbackContext context)
        {
            if (context.ReadValue<Vector2>().y > 0)
            {
                ListIndex++;
            }
            else
            {
                ListIndex--;
            }

            ListIndex = Mathf.Clamp(ListIndex,0, _scriptedNodes.Count - 1);
        }



        private void VariateDistance(InputAction.CallbackContext context)
        {
            CurrentDistanceThreshold += context.ReadValue<float>() * DistanceVariationOnPress;
            if (CurrentDistanceThreshold < 0) CurrentDistanceThreshold = 0;
        }

        private void VariateLookAtDistance(InputAction.CallbackContext context)
        {
            CameraForwardModifier += context.ReadValue<float>() * ScrollVariation;
            if (CameraForwardModifier < 0) CameraForwardModifier = 0;
        }

        private void InsertActionOne(InputAction.CallbackContext context)
        {
            if(_scriptedActions[0] is null) return;
            _scriptedNodes[ListIndex].InsertAction(_scriptedActions[0]);
        }
        private void InsertActionTwo(InputAction.CallbackContext context)
        {
            if(_scriptedActions[1] is null) return;
            _scriptedNodes[ListIndex].InsertAction(_scriptedActions[1]);
        }
        private void InsertActionThree(InputAction.CallbackContext context)
        {
            if(_scriptedActions[2] is null) return;
            _scriptedNodes[ListIndex].InsertAction(_scriptedActions[2]);
        }
    }
}
