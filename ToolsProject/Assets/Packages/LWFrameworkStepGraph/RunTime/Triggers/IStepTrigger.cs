using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 步骤触发器，主要用于检测用户的操作，得出对应的结果
/// </summary>
public interface IStepTrigger 
{
    Action<int> TiggerAction { get; set; }
    void TriggerBegin();
    void TriggerEnd();
}
