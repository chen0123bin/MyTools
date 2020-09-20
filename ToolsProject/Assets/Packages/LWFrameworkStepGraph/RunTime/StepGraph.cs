using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class StepGraph : NodeGraph {
    private IStepNode m_CurrStep;
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStepNode CurrStep {
        get => m_CurrStep;set => m_CurrStep = value;
    }
    /// <summary>
    /// 继续下一步
    /// </summary>
    public void Continue()
    {
        m_CurrStep.MoveNext();
    }
    /// <summary>
    /// 跳转节点
    /// </summary>
    /// <param name="index"></param>
    public void JumpNode(int index) {
        IStepNode stepNode  = nodes[index] as IStepNode;
        stepNode.SetCurrent();
    }
}
