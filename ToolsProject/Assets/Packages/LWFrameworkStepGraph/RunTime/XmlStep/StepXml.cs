using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StepXml : BaseStepXml
{
    //触发器集合
    public List<IStepTrigger> m_StepTriggerList;
    //控制器集合
    public List<IStepController> m_StepControllerList;
   
    /// <summary>
    /// 执行完成的数量
    /// </summary>
    private int m_CompletedCount;
    public override void StartTriggerList()
    {
        for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
        {
            m_StepTriggerList[i].TriggerBegin();
            m_StepTriggerList[i].TiggerActionCompleted = OnTiggerActionCompleted;
            m_StepTriggerList[i].CurrStepManager = m_StepManager;

        }
    }

    public override void StopTriggerList()
    {
        for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
        {
            m_StepTriggerList[i].TiggerActionCompleted = null;
            m_StepTriggerList[i].CurrStepManager = null;
            m_StepTriggerList[i].TriggerEnd();
        }
    }

    public override void StartControllerList()
    {
        base.StartControllerList();
        m_CompletedCount = 0;
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerBegin();
            m_StepControllerList[i].ControllerExecuteCompleted = OnControllerExecuteCompleted;
            m_StepControllerList[i].CurrStepGraph = m_StepManager;
        }
        //如果没有触发器直接开始执行控制器
        if (m_StepTriggerList == null || m_StepTriggerList.Count == 0)
        {
            OnTiggerActionCompleted(0);
        }
    }

    public override void StopControllerList()
    {
        base.StopControllerList();
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerExecuteCompleted = null;
            m_StepControllerList[i].ControllerEnd();
        }
    }
    void OnTiggerActionCompleted(int index)
    {
        m_NextIndex = index;
        m_CurrState = StepNodeState.Execute;
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerExecute();
        }
        //如果没有控制器直接进入下一步
        if (m_StepControllerList == null || m_StepControllerList.Count == 0)
        {
            m_StepManager.MoveNext();
        }

    }

    private void OnControllerExecuteCompleted()
    {
        m_CompletedCount++;
        if (m_CompletedCount == m_StepControllerList.Count)
        {
            m_StepManager.MoveNext();
        }
    }
    public override void InputXml(XElement xElement)
    {
        m_Remark = xElement.Attribute("Remark").Value;
        List<XElement> triggerList = xElement.Element("Triggers").Elements("Trigger").ToList();
        List<XElement> controlList = xElement.Element("Controls").Elements("Control").ToList();
        m_StepTriggerList = new List<IStepTrigger>();
        m_StepControllerList = new List<IStepController>();
        for (int i = 0; i < triggerList.Count; i++)
        {
            IStepTrigger trigger = ConverHelp.Instance.CreateInstance<IStepTrigger>(triggerList[i].Attribute("ScriptName").Value);
            trigger.InputXml(triggerList[i]);
            m_StepTriggerList.Add(trigger);
        }
        for (int i = 0; i < controlList.Count; i++)
        {
            IStepController control = ConverHelp.Instance.CreateInstance<IStepController>(controlList[i].Attribute("ScriptName").Value);
            control.InputXml(controlList[i]);
            m_StepControllerList.Add(control);
        }
    }
}
