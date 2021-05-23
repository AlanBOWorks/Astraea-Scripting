
using System.Collections.Generic;
using MEC;

namespace AIEssentials
{
    public class TaskManager : CoroutineManager<IEnumerator<float>>, ITaskManager
    {
        public TaskManager() : base()
        { }
        public TaskManager(int taskLimit) : base(taskLimit)
        { }
        public TaskManager(IEnumeratorOnEmpty defaultRequest, int taskLimit = 8)
            : base(defaultRequest, taskLimit)
        { }


        protected override CoroutineHandle InvokeRequest(IEnumerator<float> request)
        {
            return Timing.RunCoroutine(request);
        }

        private CoroutineHandle _resetHandle;
        protected override void DoReset(IEnumerator<float> reset)
        {
            Timing.KillCoroutines(_resetHandle);
            _resetHandle = Timing.RunCoroutine(reset);
        }
    }
}
