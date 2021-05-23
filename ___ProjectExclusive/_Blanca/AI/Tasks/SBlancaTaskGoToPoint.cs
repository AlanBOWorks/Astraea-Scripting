using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.AIEssentials
{
    [CreateAssetMenu(fileName = "Go To Point - N [Blanca Task]",
        menuName = "AI/Blanca/Task/Go To Point")]
    public class SBlancaTaskGoToPoint : ScriptableSequenceTask
    {
        [SerializeField]
        private SBlancaActionGoToPoint _goToPointAction = null;

        public IEnumerator<float> _DoSequence(Func<Vector3> onPoint, float distanceThreshold = .2f)
        {
            yield return Timing.WaitUntilDone(_goToPointAction._DoAction(onPoint, distanceThreshold));
        }

        [Button,DisableInEditorMode]
        public void RequestOnMain(Transform onTarget, float distanceThreshold = .2f)
        {
            RequestOnMain(TargetPoint,distanceThreshold);
            Vector3 TargetPoint() => onTarget.position;
        }
        public void RequestOnMain(Func<Vector3> onPoint, float distanceThreshold = .2f)
        {
            RequestOnMain(_DoSequence(onPoint,distanceThreshold));
        }
    }
}
