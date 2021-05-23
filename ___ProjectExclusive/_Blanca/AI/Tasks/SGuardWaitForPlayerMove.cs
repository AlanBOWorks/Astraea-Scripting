using System;
using System.Collections.Generic;
using AIEssentials;
using MEC;
using Player;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blanca.AIEssentials
{
    [CreateAssetMenu(fileName = "Wait For Player to Move - N [Blanca Guard Task]",
        menuName = "AI/Blanca/Guards/Wait For Player to Move")]
    public class SGuardWaitForPlayerMove : ScriptableObject, IGuardTask
    {
        [Title("Actions")] 
        [SerializeField] 
        private SBlancaActionGoToPoint _goToFormation = null;
        public float PathDifferenceThreshold = .4f;


        [SerializeField]
        private BlancaActionWaitForPlayerMovement _waitForPlayer = new BlancaActionWaitForPlayerMovement();

        [Title("Params")] 
        public Vector3 FormationPosition = new Vector3(.2f, 0, 1f);
        [SuffixLabel("Seconds")]
        public SRange CheckTriggerRange = new SRange(.4f,1.4f);
        [SuffixLabel("Seconds")]
        public SRange DelayAfterResume = new SRange(1,4);

        //These are mean to be for keep tracking what IPathCalculator are being used in here
        public static IPathCalculator PathForGuardingCalculator() => BlancaUtilsKinematic.MainPathCalculator;
        public static IPathCalculator PathCalculatorOnUse() => BlancaUtilsKinematic.HelperCalculators[0];

        public IEnumerator<float> _DoGuard(CoroutineHandle master)
        {
            CoroutineHandle guardHandle = Timing.CurrentCoroutine;
            Timing.LinkCoroutines(guardHandle, Timing.RunCoroutine(_SecurityKill()));
            _funcFormationPosition = GetFormationPosition;
            float distance = PathForGuardingCalculator().GetRemainingDistance();
            float playerDistance;
            DoPlayerSearchPath();


            while (master.IsRunning)
            {
                PathCalculatorOnUse().SetDestination(PlayerUtilsTransform.GetCurrentPosition());
                while (DoDistanceThreshold())
                {
                    yield return Timing.WaitForSeconds(CheckTriggerRange.RandomInRange());
                }

                Timing.PauseCoroutines(master);
                CoroutineHandle breakHandle = Timing.RunCoroutine(_BreakGuard());
                while (!ResumeCheck())
                {
                    yield return Timing.DeltaTime;
                }
                Timing.KillCoroutines(breakHandle);
                Timing.ResumeCoroutines(master);

                yield return Timing.WaitForSeconds(DelayAfterResume.RandomInRange());

                bool ResumeCheck()
                {
                    return DoDistanceThreshold() || !breakHandle.IsRunning;
                }
            }

            

            void DoPlayerSearchPath()
            {
                PlayerUtilsKinematic.MainHelper.SetDestination(PathForGuardingCalculator().GetDestination());
                playerDistance = PlayerUtilsKinematic.MainHelper.GetRemainingDistance();
            }

            IEnumerator<float> _SecurityKill()
            {
                // This is done for check is the master is done but the Timing.WaitForSeconds or
                // the _BreakGuard() is till running yet it needs to be killed
                while (master.IsRunning)
                {
                    yield return Timing.WaitForOneFrame;
                }
                Timing.KillCoroutines(guardHandle);
            }

            IEnumerator<float> _BreakGuard()
            {
                //TODO replace for animation
                yield return Timing.WaitUntilDone(BlancaActionStopMoving._StopMotorVelocity(
                    new SRange(.2f, 2f)));

                yield return Timing.WaitUntilDone(
                    _goToFormation._DoAction(
                        PathCalculatorOnUse(),
                        PlayerUtilsTransform.GetTransformData().MeshRoot,
                        _funcFormationPosition,
                        FormationPosition.magnitude - 0.01f
                    ));
                //TODO change for check PathDistance of Player in Range of LeadPathDistance
                CoroutineHandle checkMovementHandle = Timing.RunCoroutine(
                    _waitForPlayer._CheckForDirection(
                        PathForGuardingCalculator())
                    );
                Timing.LinkCoroutines(checkMovementHandle, 
                    Timing.RunCoroutine(BlancaActionStopMoving._StopMotorVelocity()));

                yield return Timing.WaitUntilDone(checkMovementHandle);
            }

            bool DoDistanceThreshold()
            {
                distance = PathForGuardingCalculator().GetRemainingDistance();
                DoPlayerSearchPath();
                return playerDistance - distance < PathDifferenceThreshold;
            }
        }

        private Func<Vector3> _funcFormationPosition;
        private Vector3 GetFormationPosition() => FormationPosition;
    }
}
