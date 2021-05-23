using UnityEngine;

namespace TaskManager
{
    public interface ITask
    {
        void DoStart();
        void DoPause();
        void DoResume();
        void DoStop();
        void DoTick();

        TaskTypes.State State { get; set; }
        ITask PausingTask { get; set; }
    }
}
