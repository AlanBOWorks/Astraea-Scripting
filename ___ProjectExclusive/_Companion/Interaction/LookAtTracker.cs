using System;
using System.Collections.Generic;
using Blanca;
using IKEssentials;
using MEC;
using Player;
using SharedLibrary;
using Sirenix.OdinInspector;
using SMaths;
using UnityEngine;

namespace Companion
{
    public class LookAtTracker
    {
        private readonly IrisLookAt _blancaLookAt;
        private readonly Transform _playerCamera;
        private readonly LookAtTrackerParams _params;

        public readonly List<ICompanionLookAtShortListener> ShortListeners;
        public readonly List<ICompanionLookAtLongListener> LongListeners;

        private const int PredictedListCapacity = 4;
        public LookAtTracker(LookAtTrackerParams parameters)
        {
            _blancaLookAt = BlancaEntitySingleton.Instance.Entity.HeadIkControl.HeadIkSolver.IrisLookAt;
            _playerCamera = PlayerEntitySingleton.Instance.Entity.CharacterTransformData.PlayerCamera.transform;
            _params = parameters;

            ShortListeners = new List<ICompanionLookAtShortListener>(PredictedListCapacity);
            LongListeners = new List<ICompanionLookAtLongListener>(PredictedListCapacity);

            CoroutineLoopHandler loopHandler = CompanionEntitySingleton.Instance.CoroutineLoopHandler;

            _shortTrackHandle = loopHandler.TrackCoroutine(_ShortTrack());
            _longTrackHandle = loopHandler.TrackCoroutine(_LongTrack());
            Timing.PauseCoroutines(_longTrackHandle);

#if UNITY_EDITOR
            AddListener(new DebugEvents());
#endif

        }

        public void AddListener(ICompanionLookAtListener listener)
        {
            ShortListeners.Add(listener);
            LongListeners.Add(listener);
        }

        public void RemoveListener(ICompanionLookAtListener listener)
        {
            ShortListeners.Remove(listener);
            LongListeners.Remove(listener);
        }

        private readonly CoroutineHandle _shortTrackHandle;
        private IEnumerator<float> _ShortTrack()
        {
            //A random so this doesn't trigger right away of starting the game
            yield return Timing.WaitForSeconds(_params.longLookTrigger.RandomInRange());

            while (_blancaLookAt != null)
            {
                if (IsInAngle())
                {
                    yield return Timing.WaitForSeconds(_params.shortLookTrigger.RandomInRange());
                    if (!IsInAngle()) continue;

                    foreach (var listener in ShortListeners)
                    {
                        listener.OnShortEvent();
                    }
                    ResumeLongCheck();
                }
                else
                {
                    yield return Timing.WaitForOneFrame;
                }
            }
        }

        private readonly CoroutineHandle _longTrackHandle;
        private IEnumerator<float> _LongTrack()
        {
            while (_blancaLookAt != null)
            {
                if (IsInAngle())
                {
                    yield return Timing.WaitForSeconds(_params.longLookTrigger.RandomInRange());
                    if (!IsInAngle()) continue;

                    foreach (var listener in LongListeners)
                    {
                        listener.OnLongEvent();
                    }

                    Timing.PauseCoroutines(Timing.CurrentCoroutine);
                }
                else
                {
                    yield return Timing.WaitForOneFrame;
                }
            }
        }

        //TODO make IsInRange
        private bool IsInAngle()
        {
            // Characters needs to be looking one each other (opposite directions) ( O_O) ----> <------ (o.o )
            return Quaternion.Angle(_playerCamera.rotation, _blancaLookAt.IrisRotation) > 180 - _params.angleThreshold;
        }

        public void ResumeShortCheck()
        {
            Timing.ResumeCoroutines(_shortTrackHandle);
            Timing.PauseCoroutines(_longTrackHandle);
        }

        private void ResumeLongCheck()
        {
            Timing.ResumeCoroutines(_longTrackHandle);
            Timing.PauseCoroutines(_shortTrackHandle);
        }


#if UNITY_EDITOR

        internal class DebugEvents : ICompanionLookAtListener
        {
            public void OnShortEvent()
            {
                Debug.Log("Trigger: On SHORT [Look At Companion]");
            }

            public void OnLongEvent()
            {
                Debug.Log("Trigger: On LONG [Look At Companion]");
            }
        }

#endif

    }

    [Serializable]
    public class LookAtTrackerParams
    {
        [Range(0,180),SuffixLabel("Degrees")]
        public float angleThreshold = 30f;
        [SuffixLabel("Seconds")]
        public SRange shortLookTrigger = new SRange(.2f,1f);
        [SuffixLabel("Seconds")]
        public SRange longLookTrigger = new SRange(2f, 8f);
    }

    public interface ICompanionLookAtListener : ICompanionLookAtShortListener, ICompanionLookAtLongListener 
    { }

    public interface ICompanionLookAtShortListener
    {
        void OnShortEvent();
    }

    public interface ICompanionLookAtLongListener
    {
        void OnLongEvent();
    }
}
