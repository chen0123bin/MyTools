using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
/// <summary>
/// 步骤节点状态
/// </summary>
public enum StepNodeState 
{
    Wait,
    Execute,
    Complete
}
