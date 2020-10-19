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

    protected Action m_ControllerExecuteCompleted;

    protected StepGraph m_CurrStepGraph;
    /// <summary>
    /// 当前控制器执行完成的回调
    /// </summary>
    public Action ControllerExecuteCompleted { get => m_ControllerExecuteCompleted; set => m_ControllerExecuteCompleted = value; }
    /// <summary>
    /// 当前的Graph
    /// </summary>
    public StepGraph CurrStepGraph { get => m_CurrStepGraph; set => m_CurrStepGraph = value; }
    public string Remark { get => m_Remark; }

    public abstract void ControllerBegin();
    public abstract void ControllerEnd();
    public abstract void ControllerExecute();


}
