
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStepTrigger :IStepTrigger
{
    
    [LabelText("触发结果"),LabelWidth(70)]
    public int m_ResultIndex;
    /// <summary>
    /// 是否触发，避免多次触发效果
    /// </summary>
    protected bool m_IsTrigger;
    protected Action<int> m_TiggerCompleted;
    public Action<int> TiggerCompleted { get => m_TiggerCompleted; set => m_TiggerCompleted = value; }
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }
    public virtual void TriggerBegin() {
        m_IsTrigger = false;
    }
    public virtual void TriggerEnd() { 
    
    }
    public virtual void CallTiggerAction()
    {
        m_IsTrigger = true;
        m_TiggerCompleted?.Invoke(m_ResultIndex);
    }
}
