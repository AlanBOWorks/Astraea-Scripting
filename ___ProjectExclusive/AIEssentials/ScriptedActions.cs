using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AIEssentials
{
    [Serializable]
    public class PlayerLikeScriptedActions 
    {
        [SerializeField]
        private List<ScriptedNode> _scriptedNodes;
        public List<ScriptedNode> GetActions() => _scriptedNodes;

        public PlayerLikeScriptedActions(int listAmount = 0)
        {
            _scriptedNodes = new List<ScriptedNode>(listAmount);
        }

        [Serializable]
        public struct ScriptedNode
        {
            public Vector3 TranslationPoint;
            public Vector3 LookAtPoint;
            public float DistanceThreshold;
            public ScriptedAction OnNodeAction;

            public ScriptedNode(Vector3 onPoint, Vector3 lookAt, float distanceThreshold = .2f)
            {
                TranslationPoint = onPoint;
                LookAtPoint = lookAt;
                DistanceThreshold = distanceThreshold;
                OnNodeAction = null;
            }

            public void InsertAction(ScriptedAction action)
            {
                OnNodeAction = action;
            }
        }

        public void InsertAction(ScriptedAction action, int listIndex)
        {
            _scriptedNodes[listIndex].InsertAction(action);
        }
    }


    public abstract class ScriptedAction : ScriptableObject
    {
        public abstract IEnumerator _DoAction();
    }
}
