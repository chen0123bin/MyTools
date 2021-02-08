using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    public override XElement ToXml()
    {
        XElement trigger = new XElement("Trigger");
        trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
        trigger.Add(new XAttribute("WaitTime", $"{m_WaitTime}"));
        trigger.Add(new XAttribute("m_ResultIndex", $"{m_ResultIndex}"));
        return trigger;
    }
    public override void InputXml(XElement xElement)
    {
        m_WaitTime = float.Parse( xElement.Attribute("WaitTime").Value);
        m_ResultIndex = int.Parse(xElement.Attribute("m_ResultIndex").Value);
    }
}
