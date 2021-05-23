using System;
using System.Collections.Generic;
using AIEssentials;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.AIEssentials
{
    [CreateAssetMenu(fileName = "Hold Hand - N [Blanca Task]",
        menuName = "AI/Blanca/Task/Hold Hand")]
    public class STaskHoldHand : ScriptableSequenceTask
    {
        [Title("Go to Point - Action")]
        [SerializeField]
        private SBlancaActionGoToPoint _goToPoint = null;
        [SerializeField]
        private BlancaActionRotationSeek _rotationSeek = new BlancaActionRotationSeek();

        [Title("Remain in Formation - Action")] 
        [SerializeField] private SBlancaActionRemainInFormation _formationAction = null;

        private Vector3 GetLocalPosition()
        {
            Vector2 formationPosition = _formationAction.GetFormationPosition();
            Vector3 targetPosition = new Vector3(
                formationPosition.x,
                0,
                formationPosition.y);
            return targetPosition;
        }


        private Func<Vector3> _formationPosition;
        public IEnumerator<float> _DoSequence(Func<bool> breakConditional,float holdHandDistanceTrigger = .3f)
        {
            _formationPosition = GetLocalPosition;
            IPathCalculator onPath = BlancaUtilsKinematic.MainPathCalculator;

            BlancaUtilsManagers.LookAtManager.RequestTask(
                _rotationSeek._DoLookAtRotation(Timing.CurrentCoroutine), TaskPriority, this
                );

            yield return Timing.WaitUntilDone(_goToPoint._DoActionOnPlayer(
                onPath, _formationPosition, holdHandDistanceTrigger));
            yield return Timing.WaitUntilDone(_formationAction._DoAction(
                onPath, breakConditional));
        }

        private Func<bool> _managerConditional;
        public IEnumerator<float> _DoSequenceWhileManagerIsEmpty(float holdHandDistanceTrigger = .3f)
        {
            _managerConditional = IsManagerEmpty;
            return _DoSequence(_managerConditional, holdHandDistanceTrigger);

            bool IsManagerEmpty()
            {
                ITaskManager manager = BlancaUtilsManagers.MainManager;
                return !manager.HasRequest() && !manager.HasPaused();
            }
        }


        public void RequestOnMain(Func<bool> breakConditional, float holdHandDistanceTrigger = .3f)
        {
            base.RequestOnMain(_DoSequence(breakConditional,holdHandDistanceTrigger));
        }

        [Button, DisableInEditorMode]
        public void RequestOnMainWhileEmpty(float holdHandDistanceTrigger = .3f)
        {
            if(BlancaUtilsManagers.MainManager.IsEmpty())
                base.RequestOnMain(_DoSequenceWhileManagerIsEmpty(holdHandDistanceTrigger));
        }
    }
}
