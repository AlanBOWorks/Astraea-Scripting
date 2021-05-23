using UnityEditor;
using UnityEngine;

namespace AIEssentials
{
    [CreateAssetMenu(fileName = "PlayerLike - N [Scripted Action]",
        menuName = "AI/Scripted/Player")]
    public class SPlayerLikeScriptedAction : ScriptableObject
    {
        [SerializeField]
        private PlayerLikeScriptedActions _actions = new PlayerLikeScriptedActions();
        public PlayerLikeScriptedActions Actions => _actions;

        public SceneAsset OnScene = null;


    }
}
