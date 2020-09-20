using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStepTrigger :IStepTrigger
{
    [LabelText("触发对象"), LabelWidth(70),ValueDropdown("GetSceneObjectList")]
    public string ObjName;
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }
    [LabelText("触发结果"),LabelWidth(70)]
    public int m_TriggerResultIndex;
    protected Action<int> m_TiggerAction;
    public Action<int> TiggerAction { get => m_TiggerAction; set => m_TiggerAction = value; }

    public abstract void TriggerBegin() ;
    public abstract void TriggerEnd();
}
