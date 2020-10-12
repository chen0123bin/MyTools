using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_WaitTime : BaseStepTrigger
{
   
    [LabelText("等待时间"),LabelWidth(70)]
    public float m_WaitTime;
    
    public override void TriggerBegin()
    {
        base.TriggerBegin();
       _= WaitTimeAsync();
    }
    /// <summary>
    //使用Task处理等待时间
    /// </summary>
    async UniTaskVoid WaitTimeAsync() {
        await UniTask.Delay(TimeSpan.FromSeconds(m_WaitTime), ignoreTimeScale: false);
        TiggerAction();
    }
    public override void TriggerEnd()
    {
        base.TriggerEnd();
    }
}
