using System;
using System.Collections.Generic;
using Blanca;
using Blanca.Actions;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AIEssentials
{
    
    [Serializable]
    public class BlancaTasksDebugger
    {
        [ShowInInspector,DisableInEditorMode]
        private IRequestManager<CoroutineHandle, IEnumerator<float>> CurrentManager => BlancaEntitySingleton.Instance.Entity.MainTaskManager;


    }
}
