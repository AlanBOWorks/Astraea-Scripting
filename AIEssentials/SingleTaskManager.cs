using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AIEssentials
{
    public class SingleTaskManager : IRequestManagerEnumerator<CoroutineHandle>
    {
        [ShowInInspector]
        public dynamic CurrentReferenceKey { get; private set; }
        [ShowInInspector]
        public IEnumerator<float> CurrentRequest { get; private set; }
        public CoroutineHandle CurrentHandle { get; private set; }
        public int CurrentPriority { get; private set; }

        private IEnumeratorOnEmpty _onEmptyAction;

        [ShowInInspector, HideInEditorMode] 
        public RequestTaskReferencer RequestedTask { get; private set; }
        [ShowInInspector, HideInEditorMode]
        public RequestTaskReferencer PausedTask { get; private set; }

        public SingleTaskManager()
        {
            RequestedTask = new RequestTaskReferencer();
            PausedTask = new RequestTaskReferencer();
        }
        public SingleTaskManager(IEnumeratorOnEmpty onEmpty) : this()
        {
            _onEmptyAction = onEmpty;
            StartOnEmptyAction();
        }
        public void InjectOnEmptyAction(IEnumeratorOnEmpty onEmpty)
        {
            _onEmptyAction = onEmpty;
            StartOnEmptyAction();
        }


        private CoroutineHandle _onEmptyHandle;
        private void StartOnEmptyAction()
        {
            Timing.KillCoroutines(_onEmptyHandle);
            _onEmptyHandle = Timing.RunCoroutine(_onEmptyAction.OnEmptyRequest());
        }

        public void RequestTask(IEnumerator<float> request, int priorityLevel, dynamic requestKey)
        {
            if(CurrentPriority > priorityLevel)
            {
                if(!RequestedTask.HasTask() || RequestedTask.Priority < priorityLevel)
                    RequestedTask.Injection(request,priorityLevel,requestKey);
                return;
            }
            StopCurrentAndDoTask(request,priorityLevel,requestKey);
        }

        public void PauseCurrentAndDoTask(IEnumerator<float> request, int priorityLevel, dynamic referenceKey)
        {
            Timing.PauseCoroutines(CurrentHandle);
            PausedTask.Injection(CurrentRequest,CurrentPriority,CurrentReferenceKey);
            DoTask(request,priorityLevel,referenceKey);
        }
        public void StopCurrentAndDoTask(IEnumerator<float> request, int priorityLevel, dynamic referenceKey)
        {
            Timing.KillCoroutines(CurrentHandle);
            DoTask(request,priorityLevel,referenceKey);
        }

        private CoroutineHandle _trackHandle;
        private void DoTask(IEnumerator<float> request, int priorityLevel, dynamic referenceKey)
        {
            CurrentRequest = request;
            CurrentPriority = priorityLevel;
            CurrentReferenceKey = referenceKey;

            CurrentHandle = Timing.RunCoroutine(CurrentRequest);

            Timing.PauseCoroutines(_onEmptyHandle);
            Timing.KillCoroutines(_trackHandle);
            _trackHandle = Timing.RunCoroutine(_TrackCurrentEnd());

            IEnumerator<float> _TrackCurrentEnd()
            {
                while (CurrentHandle.IsRunning)
                {
                    yield return Timing.DeltaTime;
                }

                if (HasPaused())
                {
                    DoExtractPaused();
                    yield break;
                }

                if (HasRequest())
                {
                    DoExtractRequested();
                    yield break;
                }

                Timing.ResumeCoroutines(_onEmptyHandle);
            }
        }


        private void DoExtractPaused()
        {
            DoTask(PausedTask.Request,PausedTask.Priority,PausedTask.Key);
            PausedTask.Clear();
        }
        private void DoExtractRequested()
        {
            DoTask(RequestedTask.Request,RequestedTask.Priority,RequestedTask.Key);
            RequestedTask.Clear();
        }

        public bool HasRequest()
        {
            return RequestedTask.HasTask();
        }
        public bool HasPaused()
        {
            return PausedTask.HasTask();
        } public bool IsEmpty()
        {
            return CurrentRequest == null; //Current should always be the last thing being null
        }

        public class RequestTaskReferencer
        {
            public IEnumerator<float> Request;
            public int Priority;
            public dynamic Key;

            public RequestTaskReferencer()
            {}

            public void Injection(IEnumerator<float> request, int priority, dynamic key)
            {
                Request = request;
                Priority = priority;
                Key = key;
            }

            public void Clear()
            {
                Request = null;
                Priority = -1000;
                Key = null;
            }

            public bool HasTask()
            {
                return Request != null;
            }
        }
    }
}
