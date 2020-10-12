using Cysharp.Threading.Tasks;
using LWFramework.Core;
using LWFramework.Message;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
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
}
