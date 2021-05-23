using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AIEssentials
{
    public abstract class CoroutineManager<TRequest> : RequestManagerBase<TRequest,CoroutineHandle>, 
        IRequestManager<CoroutineHandle, TRequest>
    {
        protected CoroutineManager() :base()
        {}

        protected CoroutineManager(int taskLimit) : base(taskLimit)
        {}

        protected CoroutineManager(IRequestOnEmpty<TRequest> defaultRequest, int taskLimit = 8) 
        : base(defaultRequest,taskLimit)
        { }

        
        public CoroutineHandle Current { get; protected set; }

        protected override bool IsCurrentRunning()
        {
            return Current.IsRunning;
        }

        protected override CoroutineHandle DoPauseCurrent()
        {
            Timing.PauseCoroutines(Current);
            return Current;
        }

        protected override void DoResumePaused(CoroutineHandle pausedRequest)
        {
            Timing.ResumeCoroutines(pausedRequest);
            Current = pausedRequest;
        }

        protected override void StopCurrent()
        {
            Timing.KillCoroutines(Current);
        }

        protected abstract CoroutineHandle InvokeRequest(TRequest request);

        protected override void InjectRequest(TRequest request)
        {
            Current = InvokeRequest(request);
        }

    }

    public abstract class RequestManagerBase<TRequest,TPause> : IRequestManager<TRequest>
    {
        [ShowInInspector, HideInEditorMode] protected Queue<UsedRequest> RequestTasks;
        [ShowInInspector, HideInEditorMode] protected Stack<PausedRequest> PausedTasks;



        [ShowInInspector, HideInEditorMode, DisableInPlayMode]
        private IRequestOnEmpty<TRequest> _defaultRequest;
        public IRequestOnEmpty<TRequest> OnEmptyRequest
        {
            set => _defaultRequest = value;
        }

        private UsedRequest _currentUsedRequest;
        public dynamic CurrentReferenceKey => _currentUsedRequest.ReferenceKey;
        public TRequest CurrentRequest => _currentUsedRequest.Request;


        private readonly int _taskLimit;

        protected RequestManagerBase() : this(8) { }

        protected RequestManagerBase(int taskLimit)
        {
            _taskLimit = taskLimit;
            RequestTasks = new Queue<UsedRequest>(_taskLimit);
            PausedTasks = new Stack<PausedRequest>(_taskLimit);
        }
        protected RequestManagerBase(IRequestOnEmpty<TRequest> defaultRequest, int taskLimit = 8) : this(taskLimit)
        {
            _defaultRequest = defaultRequest;
        }

        protected abstract void InjectRequest(TRequest request);

       
       

        private void DoRequest(UsedRequest request)
        {
            TRequest requestObject = request.Request;
            dynamic key = request.ReferenceKey;
            _currentUsedRequest = request;
            InjectRequest(requestObject);
#if UNITY_EDITOR
            Debug.Log($"{this.GetHashCode()} - Requesting : {key} - {request}");
#endif
        }
        public void DirectEnqueue(TRequest request, int priorityLevel, dynamic requestKey)
        {
            if (RequestTasks.Count >= _taskLimit)
                throw new OverflowException("Too many tasks requested.");

            RequestTasks.Enqueue(new UsedRequest(request, priorityLevel, requestKey));
        }


        /// <returns>The return value is stored for later re-use</returns>
        protected abstract TPause DoPauseCurrent();
        public void PauseCurrentAndDoTask(TRequest request, int priorityLevel, dynamic requestKey)
        {
#if UNITY_EDITOR
            Debug.Log($"{this.GetHashCode()} - Pausing: {CurrentReferenceKey}");
#endif
            if (PausedTasks.Count >= _taskLimit)
                throw new OverflowException("Too many paused tasks.");

            TPause pausedTask = DoPauseCurrent();
            PausedRequest pausedRequest = new PausedRequest(pausedTask,_currentUsedRequest);
            PausedTasks.Push(pausedRequest);
            DoRequest(new UsedRequest(request, priorityLevel, requestKey));
        }

        protected abstract void StopCurrent();
        public void StopCurrentAndDoTask(TRequest request, int priorityLevel, dynamic requestKey)
        {
            PausedTasks.Clear();
            StopCurrent();
            DoRequest(new UsedRequest(request, priorityLevel, requestKey));
        }

        public void StopCurrentAndResume()
        {
            StopCurrent();
            ResumeTask();
        }

        public void StopCurrentAndResume(TPause request)
        {
            //To take temporally from the stack
            Stack<PausedRequest> peekRequests = new Stack<PausedRequest>(PausedTasks.Count);
            while (PausedTasks.Count > 0)
            {
                PausedRequest pop = PausedTasks.Pop();
                TPause popRequest = pop.Paused;
                if (popRequest.Equals(request))
                {
                    DoRequest(pop.OnUsedRequest);
                    ReturnPeekElements();
                    return;
                }
                peekRequests.Push(pop);
            }
            ReturnPeekElements();

            void ReturnPeekElements()
            {
                foreach (PausedRequest peekRequest in peekRequests)
                {
                    //return what I taken and is not the request
                    PausedTasks.Push(peekRequest);
                }
            }
        }

        private void ResumeTask()
        {
            PausedRequest pausedRequest = PausedTasks.Pop();
            _currentUsedRequest = pausedRequest.OnUsedRequest;
            TPause paused = pausedRequest.Paused;
            DoResumePaused(paused);
            Debug.Log($"{this.GetHashCode()} -Resume: {_currentUsedRequest.ReferenceKey}");
        }

        protected abstract void DoResumePaused(TPause pausedRequest);

        public void MoveNext()
        {
            if (HasPaused())
            {
                ResumeTask();
            }
            if (HasRequest())
            {
                UsedRequest request = RequestTasks.Dequeue();
                DoRequest(request);
            }

            Reset();
        }

        protected abstract void DoReset(TRequest reset);
        public void Reset()
        {
#if UNITY_EDITOR
            Debug.Log($"{this.GetHashCode()} - All empty <Action>");
#endif
            if (_defaultRequest is null) return;
            DoReset(_defaultRequest.OnEmptyRequest());
        }

        public void Dispose()
        {
            RequestTasks.Clear();
            PausedTasks.Clear();
        }

        protected abstract bool IsCurrentRunning();

        public void RequestTask(TRequest request, int priorityLevel, dynamic requestKey)
        {
            if (!IsCurrentRunning())
            {
                DoRequest(new UsedRequest(request, priorityLevel, requestKey));
                return;
            }
            if ( _currentUsedRequest.PriorityLevel < priorityLevel)
            {
                PauseCurrentAndDoTask(request, priorityLevel, requestKey);
            }
            else
            {
                DirectEnqueue(request, priorityLevel, requestKey);
            }

        }


        public bool HasRequest()
        {
            return RequestTasks.Count > 0;
        }

        public bool HasPaused()
        {
            return PausedTasks.Count > 0;
        }

        public bool IsEmpty()
        {
            return CurrentRequest == null; //Current should always be the last thing being null
        }

        public bool ContainsInQueue(dynamic referenceKey)
        {
            foreach (UsedRequest request in RequestTasks)
            {
                if (request.ReferenceKey.Equals(request))
                    return true;
            }

            return false;
        }

        public bool ContainsInPaused(dynamic referenceKey)
        {
            foreach (PausedRequest pausedRequest in PausedTasks)
            {
                if (pausedRequest.OnUsedRequest.ReferenceKey.Equals(referenceKey))
                    return true;
            }

            return false;
        }


        protected readonly struct UsedRequest
        {
            public readonly TRequest Request;
            public readonly int PriorityLevel;
            public readonly dynamic ReferenceKey;

            public UsedRequest(TRequest request, int priorityLevel, dynamic referenceKey)
            {
                Request = request;
                PriorityLevel = priorityLevel;
                ReferenceKey = referenceKey;
            }
        }

        protected struct PausedRequest
        {
            public readonly TPause Paused;
            public UsedRequest OnUsedRequest;

            public PausedRequest(TPause paused, UsedRequest onUsedRequest)
            {
                Paused = paused;
                OnUsedRequest = onUsedRequest;
            }
        }
    }
}
