using System;
using System.Collections;
using System.Collections.Generic;
using IKEssentials;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBoxStuff : MonoBehaviour
{
    public FingersHolder Holder = new FingersHolder();
    [ShowInInspector,HideInEditorMode]
    public FingersIKControl Control = null;

}
