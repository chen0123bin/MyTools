using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
[Serializable]
public abstract class BaseStepController:IStepController
{
    [LabelText("备注"),GUIColor(0,1,0)]
    public string m_Remark;
   
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }
    protected Action m_ControllerCompleted;
    public Action ControllerCompleted { get => m_ControllerCompleted; set => m_ControllerCompleted = value; }
    public abstract void ControllerBegin();
    public abstract void ControllerEnd();
    public abstract void ControllerExecute();


}
