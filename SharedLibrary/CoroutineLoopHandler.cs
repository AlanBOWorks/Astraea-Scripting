using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    /// <summary>
    /// This handles Coroutines that loops until the object doesn't exits (because of destroy or scene changes).
    /// <br></br>
    /// This also can control the flow of the slaves Coroutine (pausing or destroying all)
    /// </summary>
    public class CoroutineLoopHandler : MonoBehaviour
    {
#if UNITY_EDITOR
        [ShowInInspector,DisableInEditorMode,DisableInPlayMode,
         InfoBox("These are the added, but are not the active")]
        private List<IEnumerator<float>> _debugCoroutines;
#endif

        public void Awake()
        {
#if UNITY_EDITOR
            _debugCoroutines = new List<IEnumerator<float>>(16);
#endif
            _masterHandle = Timing.RunCoroutine(_TrackingLoop());
        }
        

        [Button, DisableInEditorMode]
        public void Enabled(bool value = true)
        {
            if (value)
                Timing.ResumeCoroutines(_masterHandle);
            else
                Timing.PauseCoroutines(_masterHandle);
        } 



        public void StopAll()
        {
            Timing.KillCoroutines(_masterHandle);
        }

        private IEnumerator<float> _TrackingLoop()
        {
            while (this != null)
            {
                yield return Timing.WaitForOneFrame;
            }
        }
        private CoroutineHandle _masterHandle;
        /// <summary>
        /// Starts the coroutine (and returns <seealso cref="CoroutineHandle"/>) while having a link
        /// towards <see cref="_masterHandle"/>.<br></br>
        /// If this master stops by not having its transform
        /// all others coroutine will stop as a consequence
        /// </summary>
        public CoroutineHandle TrackCoroutine(IEnumerator<float> coroutine, Segment targetSegment = Segment.Update)
        {
#if UNITY_EDITOR
            _debugCoroutines.Add(coroutine);
#endif

            CoroutineHandle handle = Timing.RunCoroutine(coroutine, targetSegment);
            Timing.LinkCoroutines(_masterHandle, handle);
            return handle;
        }

    }

}
