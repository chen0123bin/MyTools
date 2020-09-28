using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;

[CreateAssetMenu]
public class StepGraph : LWNodeGraph {
    private IStepNode m_CurrStep;
    private Action m_StepGraphCompleted;
    public Action StepGraphCompleted {
        get => m_StepGraphCompleted;set => m_StepGraphCompleted = value;
    }
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStepNode CurrStep {
        get => m_CurrStep;set => m_CurrStep = value;
    }
    public void StartNode() {
        LWDebug.Log("nodes::" + nodes.Count);
        foreach (var item in nodes)
        {
            if (item.GetType() == typeof(StartNode)) {
                (item as StartNode).Start();
                break;
            }
        }
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
