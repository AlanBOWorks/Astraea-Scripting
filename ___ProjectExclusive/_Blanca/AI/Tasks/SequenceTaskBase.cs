using System.Collections.Generic;
using AIEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blanca.AIEssentials
{
    public abstract class BlancaSequenceTask : ISequenceTask
    {
        [SerializeField, Range(-100,100),BoxGroup] private int _taskPriority = 0;
        public int TaskPriority => _taskPriority;

        protected void RequestOnMain(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.MainManager.RequestTask(request,_taskPriority,key);
        }
        protected void RequestOnMain(IEnumerator<float> request) => RequestOnMain(request, this);


        protected void RequestOnLook(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.LookAtManager.RequestTask(request,_taskPriority,key);
        }
        protected void RequestOnLook(IEnumerator<float> request)=> RequestOnLook(request, this);


        protected void PauseCurrentAndDoOnMain(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.MainManager.PauseCurrentAndDoTask(request,_taskPriority,key);
        }
        protected void PauseCurrentAndDoOnMain(IEnumerator<float> request) => PauseCurrentAndDoOnMain(request,this);

    }

    public abstract class ScriptableSequenceTask : ScriptableObject, ISequenceTask
    {
        [SerializeField, Range(-100, 100)] private int _taskPriority = 0;
        public int TaskPriority => _taskPriority;


        protected void RequestOnMain(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.MainManager.RequestTask(request, _taskPriority, key);
        }
        protected void RequestOnMain(IEnumerator<float> request) => RequestOnMain(request, this);


        protected void RequestOnLook(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.LookAtManager.RequestTask(request, _taskPriority, key);
        }
        protected void RequestOnLook(IEnumerator<float> request) => RequestOnLook(request, this);


        protected void PauseCurrentAndDoOnMain(IEnumerator<float> request, dynamic key)
        {
            BlancaUtilsManagers.MainManager.PauseCurrentAndDoTask(request, _taskPriority, key);
        }
        protected void PauseCurrentAndDoOnMain(IEnumerator<float> request) => PauseCurrentAndDoOnMain(request, this);

    }
}
