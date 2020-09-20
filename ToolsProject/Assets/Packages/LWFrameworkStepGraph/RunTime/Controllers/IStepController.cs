using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public interface IStepController
{
    Action ControllerCompleted { get; set; }
    void ControllerBegin();
    void ControllerEnd();
    void ControllerExecute();
   
}
