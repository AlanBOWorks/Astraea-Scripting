using System;
using System.Collections.Generic;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TaskManager
{
    public class TaskManagerDebugger : MonoBehaviour
    {
        [ShowInInspector] 
        private TaskManager _manager = null;

        private bool HasManager()
        {
            return _manager != null;
        }

        public void InjectManager(TaskManager manager)
        {
            _manager = manager;
        }
        
        [Button(ButtonSizes.Gigantic),HideIf("HasManager"),HideInEditorMode]
        private void InjectTesterManager()
        {
            if(_manager != null) return;
            DebugTask defaultTask = new DebugTask("Default task");
            _manager = new TaskManager(defaultTask, this);
        }

        [Button, ShowIf("HasManager"), HideInEditorMode]
        private void RequestDefaultTask()
        {
            DebugTask defaultTask = new DebugTask("(New) Default task");
            _manager.SwitchDefaultTask(defaultTask);
        }
        [Button, ShowIf("HasManager"), HideInEditorMode]
        private void RequestDefaultTask(float finishAfterSeconds)
        {
            DebugTask defaultTask = new DebugTask("(Timer) Request task");
            _manager.SwitchDefaultTask(defaultTask);
            Timing.RunCoroutine(_DoStop());

            IEnumerator<float> _DoStop()
            {
                yield return Timing.WaitForSeconds(finishAfterSeconds);
                defaultTask.State = TaskTypes.State.Stop;
            }
        }
        
        [Button,ShowIf("HasManager"),HideInEditorMode]
        private void RequestTask()
        {
            _manager.RequestTask(new DebugTask("Request task"));
        }
        
        [Button,ShowIf("HasManager"),HideInEditorMode]
        private void RequestPauseTask()
        {
            _manager.PauseCurrentAndDoTask(new DebugTask("Pausing Request task"));
        }

        [Button,ShowIf("HasManager"),HideInEditorMode]
        private void RequestStopTask()
        {
            _manager.StopCurrentAndDoTask(new DebugTask("Stop Request task"));
        }

        [Button, ShowIf("HasManager"), HideInEditorMode, GUIColor(.7f,.3f,.2f)]
        private void StopCurrent()
        {
            _manager.CurrentTask.State = TaskTypes.State.Stop;
        }
        
    }

    public class DebugTask : ITask
    {
        private string _taskName;
        [ShowInInspector,DisableInPlayMode]
        public TaskTypes.State State { get; set; }
        

        public DebugTask(string taskName)
        {
            _taskName = taskName;
        }
        
        public void DoStart()
        {
            State = TaskTypes.State.Running;
            Debug.Log($"Starting task : {_taskName}");
        }

        public void DoPause()
        {
            State = TaskTypes.State.Paused;
            Debug.Log($"Pausing task : {_taskName}");

        }

        public void DoResume()
        {
            State = TaskTypes.State.Running;
            Debug.Log($"Resume task : {_taskName}");

        }

        public void DoStop()
        {
            State = TaskTypes.State.Stop;
            Debug.Log($"Stop task : {_taskName}");
        }

        public void DoTick()
        {
            Debug.Log($"Ticking task : {_taskName}");
        }

        
        

        public ITask PausingTask { get; set; }
    }
    
    public class TaskManager
    {
        [ShowInInspector,DisableInPlayMode]
        private readonly Queue<ITask> _requestTasks;
        [ShowInInspector]
        public ITask CurrentTask { get; private set; }
        [ShowInInspector]
        public ITask DefaultTask { get; private set; }
        public object EntityCheck;

        public Segment LoopSegment
        {
            set => Timing.SetSegment(_tickHandle, value);
        }

        public TaskManager(int taskAmountLimit = 16)
        {
            _requestTasks = new Queue<ITask>(taskAmountLimit);
            EntityCheck = this;
            _tickHandle = Timing.RunCoroutine(TickLoop());
        }

        public TaskManager(ITask defaultTask, int taskAmountLimit = 16)
        : this(taskAmountLimit)
        {
            DefaultTask = defaultTask;
            CurrentTask = defaultTask;
            DefaultTask.DoStart();
        }

        public TaskManager
            (ITask defaultTask, object entityCheck, int taskAmountLimit = 16)
        : this(defaultTask,taskAmountLimit)
        {
            EntityCheck = entityCheck;
        }

        private readonly CoroutineHandle _tickHandle;
        private IEnumerator<float> TickLoop()
        {
            yield return Timing.WaitForOneFrame;
            while (EntityCheck != null)
            {
                CurrentTask.DoTick();
                yield return Timing.WaitForOneFrame;
                HandleState();
            }
            CurrentTask.DoStop();

            void HandleState()
            {
                if (CurrentTask.State != TaskTypes.State.Stop) return;
                //This means that the ITask has done its .DoStop(), so is not necessary in here
                
                ITask pausingTask = CurrentTask.PausingTask;
                if (pausingTask != null)
                {
                    DoSwitchResume(pausingTask);
                }
                else
                {
                    HandleNextRequest();
                }
                
                void HandleNextRequest()
                {
                    if (_requestTasks.Count > 0)
                    {
                        DoSwitchTask(_requestTasks.Dequeue());
                    }
                    else
                    {
                        DoDefaultOrPause();
                    }
                }
                void DoDefaultOrPause()
                {
                    if (DefaultTask != null)
                    {
                        SwitchToDefault();
                    }
                    else
                    {
                        Timing.PauseCoroutines(_tickHandle); //it resumes in DoSwitchTask
                    }
                }
            }
        }

        
        private void SwitchToDefault()
        {
            DoSwitchResume(DefaultTask);
        }

        public void SwitchDefaultTask(ITask defaultTask)
        {
            bool isDefaultCurrent = DefaultTask == CurrentTask;
            
            DefaultTask.DoStop();
            DefaultTask = defaultTask;
            DefaultTask.DoStart();
            if (isDefaultCurrent) return;
            
            DefaultTask.DoPause();
            DefaultTask.State = TaskTypes.State.Paused;
        }
        
        private void DoSwitchTask(ITask task)
        {
            task.DoStart();
            CurrentTask = task;
            CurrentTask.State = TaskTypes.State.Running;
            Timing.ResumeCoroutines(_tickHandle);
        }

        private void DoSwitchResume(ITask task)
        {
            task.DoResume();
            task.State = TaskTypes.State.Running;
            CurrentTask = task;
            Timing.ResumeCoroutines(_tickHandle);
        }

        
        public void RequestTask(ITask task)
        {
            if(CurrentTask != DefaultTask)
                _requestTasks.Enqueue(task);
            else
            {
                DoSwitchTask(task);
            }
        }

        public void PauseCurrentAndDoTask(ITask task)
        {
            task.PausingTask = CurrentTask;
            CurrentTask.DoPause();
            DoSwitchTask(task);
        }

        public void StopCurrentAndDoTask(ITask task)
        {
            CurrentTask.DoStop();
            StopLoopThroughPaused();
            DoSwitchTask(task);

            void StopLoopThroughPaused()
            {
                ITask linkedPausedTask = CurrentTask.PausingTask;
                while (linkedPausedTask != null)
                {
                    linkedPausedTask.DoStop();
                    linkedPausedTask = linkedPausedTask.PausingTask;
                }
            }
        }
    }
    

    public static class TaskTypes
    {
        public enum State
        {
            Stop,
            Running,
            Paused
        }
    }
}
