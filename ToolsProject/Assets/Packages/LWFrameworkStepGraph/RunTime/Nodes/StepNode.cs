using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public class StepNode : BaseStepNode
{   
    [LabelText("触发器集合"),NonSerialized, OdinSerialize]
    public List<IStepTrigger> m_StepTriggerList;
    [LabelText("控制器集合"), NonSerialized, OdinSerialize]
    public List<IStepController> m_StepControllerList;
    /// <summary>
    /// 执行完成的数量
    /// </summary>
    private int m_CompletedCount;
    // Use this for initialization
    protected override void Init() {
		base.Init();		
	}
   
    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port) {
		return null; // Replace this
	}

    public override void OnEnter()
    {
        base.OnEnter();
        m_CompletedCount = 0;
        for (int i = 0; m_StepTriggerList!=null&&i < m_StepTriggerList.Count; i++)
        {
            m_StepTriggerList[i].TiggerCompleted = OnTiggerAction;
            m_StepTriggerList[i].TriggerBegin();
        }
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerCompleted = OnControllerCompleted;
            m_StepControllerList[i].ControllerBegin();
        }
        //如果没有触发器直接开始执行控制器
        if (m_StepTriggerList == null || m_StepTriggerList.Count == 0) {
            OnTiggerAction(0);
        }
    }

  
    public override void OnExit()
    {
        base.OnExit();
        for (int i = 0; m_StepTriggerList != null&& i < m_StepTriggerList.Count; i++)
        {
            m_StepTriggerList[i].TiggerCompleted = null;
            m_StepTriggerList[i].TriggerEnd();
        }
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerCompleted = null;
            m_StepControllerList[i].ControllerEnd();
        }
    }
    void OnTiggerAction(int index) {
        m_NextIndex = index;
        //MoveNext();
        for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
        {
            m_StepControllerList[i].ControllerExecute();
        }
    }

    private void OnControllerCompleted()
    {
        m_CompletedCount++;
        if (m_CompletedCount == m_StepControllerList.Count) {
            MoveNext();
        }        
    }

}
