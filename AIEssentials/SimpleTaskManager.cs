using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AIEssentials
{


    public class SimpleRequestManager : SimpleRequestManagerBase
    {
        public int PriorityThreshold;
        protected override bool IsPriorityPause(int priorityLevel)
        {
            return priorityLevel > PriorityThreshold;
        }

        public SimpleRequestManager(int priorityThreshold = 30)
        {
            PriorityThreshold = priorityThreshold;
        }

        public SimpleRequestManager(IRequestOnEmpty<IEnumerator<float>> onEmptyRequest, int priorityThreshold = 30) : base(onEmptyRequest)
        {
            PriorityThreshold = priorityThreshold;
        }
    }

    /// <summary>
    /// Simple version of <seealso cref="TaskManager"/>; Does keep track of <seealso cref="CoroutineHandle"/> by
    /// injecting <seealso cref="IEnumerator{T}"/>
    /// </summary>
    public abstract class SimpleRequestManagerBase : ITaskManager
    {
        private Dictionary<dynamic, IEnumerator<float>> _requestQueueDictionary;
        [ShowInInspector]
        private Queue<RequestKey> _requestQueue;
        private Dictionary<dynamic, IEnumerator<float>> _pausedRequestsDictionary;
        [ShowInInspector]
        private Stack<RequestKey> _pausedRequests;

        private CoroutineHandle _onEmptyHandle;
        [ShowInInspector]
        private IRequestOnEmpty<IEnumerator<float>> _onEmptyDoRequest;


        protected struct RequestKey
        {
            public IEnumerator<float> Element;
            public dynamic Key;

            public RequestKey(IEnumerator<float> element, dynamic key)
            {
                Element = element;
                Key = key;
            }
        }

        protected SimpleRequestManagerBase()
        {
            _requestQueueDictionary = new Dictionary<dynamic, IEnumerator<float>>(8);
            _requestQueue = new Queue<RequestKey>(8);
            _pausedRequestsDictionary = new Dictionary<dynamic, IEnumerator<float>>(8);
            _pausedRequests = new Stack<RequestKey>(8);
        }


        protected SimpleRequestManagerBase(IRequestOnEmpty<IEnumerator<float>> onEmptyRequest) : this()
        {
            _onEmptyDoRequest = onEmptyRequest;
        }

        public void InjectOnEmptyRequest(IRequestOnEmpty<IEnumerator<float>> onEmpty) => _onEmptyDoRequest = onEmpty;

        public IEnumerator<float> CurrentRequest { get; private set; }
        public CoroutineHandle CurrentHandle  {get; private set; }
        public dynamic CurrentReferenceKey { get; private set; }

        private void DoRequest(IEnumerator<float> request, dynamic referenceKey)
        {

            CurrentRequest = request;
            CurrentReferenceKey = referenceKey;
            CurrentHandle = Timing.RunCoroutine(request);

            Timing.WaitForOtherHandles(Timing.RunCoroutine(_WaitForNext()),CurrentHandle);
        }

        private IEnumerator<float> _WaitForNext()
        {
            yield return Timing.WaitForOneFrame;
            MoveNext();
        }

        protected abstract bool IsPriorityPause(int priorityLevel);
        public void RequestTask(IEnumerator<float> request, int priorityLevel, dynamic requestKey)
        {
            if (CurrentRequest is null)
            {
                Timing.KillCoroutines(_onEmptyHandle);
                DoRequest(request, requestKey);
                return;
            }

            if (IsPriorityPause(priorityLevel) && !ContainsInPaused(requestKey))
            {
                PauseCurrentAndDoTask(request, priorityLevel, requestKey);
            }
            else if(!ContainsInQueue(requestKey))
            {
                DirectEnqueue(request, priorityLevel, requestKey);
            }
        }

        public void DirectEnqueue(IEnumerator<float> request, int priorityLevel, dynamic requestKey)
        {
            _requestQueueDictionary.Add(requestKey, request);
            _requestQueue.Enqueue(new RequestKey(request, requestKey));
        }

        public void StopCurrentAndDoTask(IEnumerator<float> request, int priorityLevel, dynamic referenceKey)
        {
            Timing.KillCoroutines(CurrentHandle);
            DoRequest(request, referenceKey);
        }

        public void StopCurrentAndResume()
        {
            Timing.KillCoroutines(CurrentHandle);
            if(HasPaused())
                DoPopPause();
        }

        public void PauseCurrentAndDoTask(IEnumerator<float> request, int priorityLevel, dynamic referenceKey)
        {
            if (CurrentRequest != null)
            {
                _pausedRequestsDictionary.Add(referenceKey, request);
                _pausedRequests.Push(new RequestKey(request, referenceKey));
                Timing.PauseCoroutines(CurrentHandle);
            }
            DoRequest(request, referenceKey);
        }

        private void DoPopPause()
        {
            RequestKey pausedKey = _pausedRequests.Pop();
            IEnumerator<float> request = pausedKey.Element;
            dynamic key = pausedKey.Key;
            _pausedRequestsDictionary.Remove(key);
            DoRequest(request, pausedKey);
        }

        public void MoveNext()
        {
            if (HasPaused())
            {
                DoPopPause();
                return;
            }
            if (HasRequest())
            {
                RequestKey requestKey = _requestQueue.Dequeue();
                IEnumerator<float> request = requestKey.Element;
                dynamic key = requestKey.Key;
                _requestQueueDictionary.Remove(key);
                DoRequest(request, requestKey);

                return;
            }

            DoEmpty();
        }

        private void DoEmpty()
        {
            Timing.KillCoroutines(_onEmptyHandle);
            CurrentReferenceKey = null;
            CurrentRequest = null;
            _onEmptyHandle = Timing.RunCoroutine(_onEmptyDoRequest.OnEmptyRequest());
        }

        public bool HasRequest()
        {
            return _requestQueueDictionary.Count > 0;
        }

        public bool HasPaused()
        {
            return _pausedRequestsDictionary.Count > 0;
        }
        public bool IsEmpty()
        {
            return CurrentRequest is null; //Current should always be the last thing being null
        }

        public bool ContainsInQueue(dynamic referenceKey)
        {
            return _requestQueueDictionary.ContainsKey(referenceKey);
        }

        public bool ContainsInPaused(dynamic referenceKey)
        {
            return _pausedRequestsDictionary.ContainsKey(referenceKey);
        }
    }


}
