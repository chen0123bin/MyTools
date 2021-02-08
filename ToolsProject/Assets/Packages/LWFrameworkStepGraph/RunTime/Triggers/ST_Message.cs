using Cysharp.Threading.Tasks;
using LWFramework.Core;
using LWFramework.Message;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ST_Message : BaseStepTrigger
{
   
    [LabelText("消息名称"),LabelWidth(70)]
    public string m_Message;
    
    public override void TriggerBegin()
    {
        base.TriggerBegin();
        MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(m_Message, OnMessage);
    }

    private void OnMessage(Message msg)
    {
        TiggerAction();
    }

    public override void TriggerEnd()
    {
        base.TriggerEnd();
        MainManager.Instance.GetManager<GlobalMessageManager>().RemoveListener(m_Message);
    }

    public override XElement ToXml()
    {
        XElement trigger = new XElement("Trigger");
        trigger.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
        trigger.Add(new XAttribute("Message", $"{m_Message}"));
        trigger.Add(new XAttribute("m_ResultIndex", $"{m_ResultIndex}"));
        return trigger;
    }
    public override void InputXml(XElement xElement)
    {
        m_Message = xElement.Attribute("Message").Value;
        m_ResultIndex = int.Parse(xElement.Attribute("m_ResultIndex").Value);
    }
}
