using System;
using System.Collections;
using System.Collections.Generic;
using LWFramework.Core;
using LWFramework.Message;
using UnityEngine;

public class Trigger_ContentMessage : Trigger_Base
{
    public string MessageType;
    private void Start()
    {
        MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(MessageType, TriggerMessage);
      //  ContentMessageManager.GetInstance()._messageManager.AddListener(CommonMessageType.Common_LastBtn, BackMessage);
    }

    private void TriggerMessage(Message msg)
    {
        OnNextEvent(this.gameObject);
    }
   
    // Update is called once per frame
    void Update()
    {
        TestTrigger();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        MainManager.Instance.GetManager<GlobalMessageManager>().RemoveListener(MessageType);
    }
   
}
