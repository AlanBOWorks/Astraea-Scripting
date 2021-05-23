using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibrary
{
    /// <summary>
    /// Emulates the <seealso cref="MonoBehaviour"/>'s Update for <seealso cref="ITicker"/> (which are
    /// traditional objects instead being <seealso cref="MonoBehaviour"/>). The reason for this
    /// is to keep the Inspector clean and avoid decoupling in the Editor (which it can be fatal
    /// if all behaviour scales too much; which is quite often)<br></br>
    ///
    /// It also has the potential of handling ticks in more controlled matter and even in different fragments of
    /// time (UnScaled, late, super late).
    /// </summary>
    public class TickerHandler : MonoBehaviour, ISerializationCallbackReceiver
    {
        [Title("Tick listeners")]
        [ShowInInspector, HideInEditorMode]
        private List<ITicker> _callbackReceivers;

        [Title("Params")] 
        [SerializeField] private Segment _timeSegment;
        [Range(1,1000)] public int FrameRate = 1;


        public void AddCallbackReceiver(ITicker ticker)
        {
            _callbackReceivers.Add(ticker);
        }

        public void RemoveCallbackReceiver(ITicker ticker)
        {
            _callbackReceivers.Remove(ticker);
        }

        private void RunTickCoroutine()
        {
            _tickHandle = Timing.RunCoroutine(_DoTicks(), _timeSegment);
        }

        private CoroutineHandle _tickHandle;
        private void Start()
        {
            RunTickCoroutine();
        }

        private IEnumerator<float> _DoTicks()
        {
            // Safety check of this = existing (instead of killing the Coroutine instantly OnDestroy)
            // It will run one time before stopping, securing all Ticks are made
            // (some Ticks could finish while others don't, making the data/states un-synchronized.
            // This prevents happening that)
            while (this) 
            {
                foreach (ITicker ticker in _callbackReceivers)
                {
                    if (!ticker.Disabled) ticker.Tick();
                }

                yield return Timing.WaitForSeconds(Timing.DeltaTime * FrameRate);
            }

        }

        private void OnEnable()
        {
            Timing.ResumeCoroutines(_tickHandle);
        }

        private void OnDisable()
        {
            Timing.PauseCoroutines(_tickHandle);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _callbackReceivers = new List<ITicker>();
        }
    }
}
