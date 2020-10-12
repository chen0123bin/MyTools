using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public class SC_WaitTime : BaseStepController
{    

    [LabelText("结束等待时间"), LabelWidth(90)]
    public float m_EndWaitTime;
    public override void ControllerBegin()
    {
      
    }
    public override void ControllerEnd()
    {
        
    }
    public override void ControllerExecute()
    {
       _ = WaitTimeAsync();
    }

    //使用Task处理等待时间
    /// </summary>
    async UniTaskVoid WaitTimeAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(m_EndWaitTime), ignoreTimeScale: false);
        m_ControllerCompleted?.Invoke();
    }
}
