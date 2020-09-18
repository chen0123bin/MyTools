using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class StepGraph : NodeGraph {
    private IStepNode currStep;
    public IStepNode CurrStep {
        get => currStep;set => currStep = value;
    }
    /// <summary>
    /// 继续下一步
    /// </summary>
    public void Continue()
    {
        currStep.MoveNext();
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