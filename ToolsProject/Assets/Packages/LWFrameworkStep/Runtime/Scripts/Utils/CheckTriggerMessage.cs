using LWFramework.Core;
using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTriggerMessage  {

    public static  void CheckDispatcher(string type) {


        if (StaticValue.CurrMode == Mode.testMode) {
            if (ObjectManager.Instance.GetGameObject("UIPrompt") && ObjectManager.Instance.GetGameObject("UIPrompt").GetComponent<Trigger_ContentMessage>() && ObjectManager.Instance.GetGameObject("UIPrompt").GetComponent<Trigger_ContentMessage>().MessageType != type){
                //ResultManager.GetInstance().UIError();
            }
        }
        if (ObjectManager.Instance.GetGameObject("UIPrompt") &&ObjectManager.Instance.GetGameObject("UIPrompt").GetComponent<Trigger_ContentMessage>() &&ObjectManager.Instance.GetGameObject("UIPrompt").GetComponent<Trigger_ContentMessage>().MessageType == type)
        {
            MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(type);
        }
    }
}
