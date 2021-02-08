using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;

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
        m_ControllerExecuteCompleted?.Invoke();
    }

    public override XElement ToXml()
    {
        XElement control = new XElement("Control");
        control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
        control.Add(new XAttribute("EndWaitTime", $"{m_EndWaitTime}"));
        control.Add(new XAttribute("Remark", $"{m_Remark}"));
        return control;
    }
    public override void InputXml(XElement xElement)
    {
        m_EndWaitTime = float.Parse( xElement.Attribute("EndWaitTime").Value);

    }
}
