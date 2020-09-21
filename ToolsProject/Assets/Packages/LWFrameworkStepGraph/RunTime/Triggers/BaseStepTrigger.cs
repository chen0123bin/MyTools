using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStepTrigger :IStepTrigger
{
    [LabelText("触发对象"), LabelWidth(70),ValueDropdown("GetSceneObjectList")]
    public string m_ObjName; 
    [LabelText("触发结果"),LabelWidth(70)]
    public int m_TriggerResultIndex;
    /// <summary>
    /// 是否触发，避免多次触发效果
    /// </summary>
    protected bool m_IsTrigger;
    protected Action<int> m_TiggerAction;
    public Action<int> TiggerCompleted { get => m_TiggerAction; set => m_TiggerAction = value; }
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }
    public virtual void TriggerBegin() {
        m_IsTrigger = false;
    }
    public virtual void TriggerEnd() { 
    
    }
}
