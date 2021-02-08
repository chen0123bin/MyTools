using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
/// <summary>
/// 步骤触发器，主要用于检测用户的操作，得出对应的结果
/// </summary>
public interface IStepTrigger : IConverXmlGraph
{
    /// <summary>
    ///  控制器执行完成
    /// </summary>
    Action<int> TiggerActionCompleted { get; set; }
    /// <summary>
    /// 当前的StepManager
    /// </summary>
    IStepManager CurrStepManager { get; set; }
    /// <summary>
    /// 开始触发器
    /// </summary>
    void TriggerBegin();
    /// <summary>
    /// 结束触发器
    /// </summary>
    void TriggerEnd();
    /// <summary>
    /// 调用触发器
    /// </summary>
    void TiggerAction();
}
