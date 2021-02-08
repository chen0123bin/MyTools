using LWFramework.Core;
using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public abstract class BaseStepXml : IStep
{
    public string m_Remark;
    /// <summary>
    /// 步骤管理器
    /// </summary>
    protected IStepManager m_StepManager;
    public IStepManager StepManager {
        get => m_StepManager;set => m_StepManager = value;
    }
    /// <summary>
    /// 下一步骤的脚标
    /// </summary>
    protected int m_NextIndex;

    protected StepNodeState m_CurrState;
    /// <summary>
    /// 当前的状态
    /// </summary>
    public StepNodeState CurrState { get => m_CurrState; set => m_CurrState = value; }

    protected IStep m_PrevNode;
    protected IStep m_NextNode;
    /// <summary>
    /// 上一节点
    /// </summary>
    public IStep PrevNode { get => m_PrevNode; set => m_PrevNode = value; }
    /// <summary>
    /// 下一节点
    /// </summary>
    public IStep NextNode
    {
        set => m_NextNode = value;
        get
        {
            m_NextNode = m_StepManager.GetNextStepByIndex(m_NextIndex);
            if (m_NextNode == null) {
                m_StepManager.StepAllCompleted?.Invoke();
                LWDebug.Log("执行结束了");
                return null;
            }
            return m_NextNode;
        }
    }
    public void SetSelfCurrent()
    {
        StartControllerList();
        StartTriggerList();
        Message msg = MessagePool.GetMessage(nameof(StepCommonMessage.StepHelpMessage));
        msg["HelpText"] = m_Remark;
        MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
    }

    public abstract void StartTriggerList();
    public abstract void StopTriggerList();
    public virtual void StartControllerList()
    {
        m_CurrState = StepNodeState.Wait;
    }
    public virtual void StopControllerList()
    {
        m_CurrState = StepNodeState.Complete;
    }

    public virtual XElement ToXml()
    {
        LWDebug.LogError("未实现ToXml函数");
        return null;
    }

    public virtual void InputXml(XElement xElement)
    {
        LWDebug.LogError("未实现InputXml函数");
    }
}
