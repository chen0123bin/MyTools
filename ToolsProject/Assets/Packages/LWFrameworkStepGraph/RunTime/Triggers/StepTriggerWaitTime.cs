using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTriggerWaitTime : BaseStepTrigger
{
   
    [LabelText("等待时间"),LabelWidth(70)]
    public float m_WaitTime;
    
    public override void TriggerBegin()
    {
       _= WaitTimeAsync();
    }
    /// <summary>
    //使用Task处理等待时间
    /// </summary>
    async UniTaskVoid WaitTimeAsync() {
        await UniTask.Delay(TimeSpan.FromSeconds(m_WaitTime), ignoreTimeScale: false);
        LWDebug.Log("等待时间到");
        m_TiggerAction?.Invoke(m_TriggerResultIndex);
    }
    public override void TriggerEnd()
    {
    }
}
