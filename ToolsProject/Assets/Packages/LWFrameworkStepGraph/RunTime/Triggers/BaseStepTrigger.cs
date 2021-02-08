
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
[Serializable]
public class BaseStepTrigger :IStepTrigger
{ 
    // 是否触发，避免多次触发效果
    protected bool m_IsTrigger;
    protected Action<int> m_TiggerActionCompleted;
    protected IStepManager m_CurrStepManager;
    [LabelText("触发结果"),LabelWidth(70)]
    public int m_ResultIndex;
    /// <summary>
    /// TiggerCompleted触发完成
    /// </summary>
    public Action<int> TiggerActionCompleted { get => m_TiggerActionCompleted; set => m_TiggerActionCompleted = value; }
    /// <summary>
    /// 当前的Graph
    /// </summary>
    public IStepManager CurrStepManager { get => m_CurrStepManager; set => m_CurrStepManager = value; }

 
    public virtual void TriggerBegin() {
        m_IsTrigger = false;
    }
    public virtual void TriggerEnd() { 
    
    }
    public virtual void TiggerAction()
    {
        m_IsTrigger = true;
        m_TiggerActionCompleted?.Invoke(m_ResultIndex);
        
    }


    public virtual XElement ToXml()
    {
        return null;
    }

    public virtual void InputXml(XElement xElement)
    {
    }
}
