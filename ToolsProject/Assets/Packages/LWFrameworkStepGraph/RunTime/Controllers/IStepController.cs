using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public interface IStepController
{
    /// <summary>
    /// 当前控制器执行完成的回调
    /// </summary>
    Action ControllerExecuteCompleted { get; set; }
    /// <summary>
    /// 当前的Graph
    /// </summary>
    StepGraph CurrStepGraph { get; set; }
    /// <summary>
    ///开始控制器
    /// </summary>
    void ControllerBegin();
    /// <summary>
    /// 结束控制器
    /// </summary>
    void ControllerEnd();
    /// <summary>
    /// 执行控制器
    /// </summary>
    void ControllerExecute();
   
}
