using System;
using System.Collections.Generic;
using AIEssentials;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.AIEssentials
{
    [CreateAssetMenu(fileName = "Lead To Point - N [Blanca Task]",
        menuName = "AI/Blanca/Task/Lead to Point")]
    public class STaskLeadToPoint : ScriptableSequenceTask
    {
        [Title("Actions")]
        [SerializeField]
        private SBlancaActionGoToPoint _goToPoint = null;
        [SerializeField]
        private BlancaActionRotationSeek _rotationSeek = new BlancaActionRotationSeek();

        [Title("Guards")] 
        [SerializeField] private SGuardWaitForPlayerMove _playerDistanceGuard = null;
        private IEnumerator<float> _DoSequence(Func<Vector3> onPoint, float distanceThreshold = .2f)
        {
            CoroutineHandle goToHandle = Timing.RunCoroutine(_goToPoint._DoAction(onPoint, distanceThreshold));
            Timing.RunCoroutine(_rotationSeek._DoRotation(goToHandle, BlancaUtilsKinematic.MainPathCalculator));

            BlancaUtilsManagers.LookAtManager.PauseCurrentAndDoTask(
                _rotationSeek._DoLookAtRotation(goToHandle),
                TaskPriority,this);

            Timing.LinkCoroutines(Timing.CurrentCoroutine, goToHandle);
            Timing.RunCoroutine(_playerDistanceGuard._DoGuard(goToHandle));
            yield return Timing.WaitUntilDone(goToHandle);
        }

        private Func<Vector3> _onTargetFunc;
        [Button, DisableInEditorMode]
        public void RequestOnMain(Transform onTarget, float distanceThreshold = .2f)
        {
            _onTargetFunc = TargetPoint;
            RequestOnMain(_onTargetFunc, distanceThreshold);
            Vector3 TargetPoint() => onTarget.position;
        }
        public void RequestOnMain(Func<Vector3> onPoint, float distanceThreshold = .2f)
        {
            RequestOnMain(_DoSequence(onPoint, distanceThreshold));
        }
    }
}
