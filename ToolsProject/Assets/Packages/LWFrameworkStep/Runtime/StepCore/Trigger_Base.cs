using LWFramework.Core;
using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Base : MonoBehaviour {
    public string TriggerName;
    public delegate void TriggerEventDelegate(GameObject go);
    // 基于上面的委托定义事件
    public static event TriggerEventDelegate nextEvent;

    // 基于上面的委托定义事件
    public static event TriggerEventDelegate backEvent;

    // 基于上面的委托定义事件
    public static event TriggerEventDelegate triggerUpdateEvent;
    private void OnEnable()
    {
        MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(CommonMessageType.Common_NextBtn, NextMessage);
        MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(CommonMessageType.Common_PrevBtn, BackMessage);
    }
    protected virtual void OnDestroy()
    {
        MainManager.Instance.GetManager<GlobalMessageManager>().RemoveListener(CommonMessageType.Common_PrevBtn);
        MainManager.Instance.GetManager<GlobalMessageManager>().RemoveListener(CommonMessageType.Common_NextBtn);
    }
    private void NextMessage(Message msg)
    {
        OnNextEvent(this.gameObject);
    }
    private void BackMessage(Message msg)
    {
        OnBackEvent(this.gameObject);
    }
    private void Start()
    {
        
    }
    // Use this for initialization
    public virtual void OnNextEvent(GameObject go)
    {
        if (nextEvent != null)
        {
            nextEvent(go);
            Destroy(this);
        }
    }
    /// <summary>
    /// 返回事件
    /// </summary>
    /// <param name="go"></param>
    public virtual void OnBackEvent(GameObject go)
    {
        
        if (backEvent != null)
        {
            backEvent(go);
            Destroy(this);
        }
    }
    public virtual void OnTriggerUpdateEvent(GameObject go)
    {
        if (triggerUpdateEvent != null)
        {
            triggerUpdateEvent(go);
        }
    }
    public void TestTrigger()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            OnNextEvent(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            OnBackEvent(gameObject);
        }
    }
}
