using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
using LWNode.LWStepGraph;

[CreateAssetMenu]
public class StepGraph : LWNodeGraph {
    private IStepNode m_CurrStep;
    private Action m_StepGraphCompleted;
    [HideInInspector, SerializeField]
    public List<string> m_ObjectArray;
    /// <summary>
    /// 当前Graph全部执行完成
    /// </summary>
    public Action StepGraphCompleted {
        get => m_StepGraphCompleted;set => m_StepGraphCompleted = value;
    }
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStepNode CurrStep {
        get => m_CurrStep;set => m_CurrStep = value;
    }
    /// <summary>
    /// 开始节点
    /// </summary>
    public void StartNode() {
        foreach (var item in nodes)
        {
            if (item.GetType() == typeof(StartNode)) {
                (item as StartNode).Start();
                break;
            }
        }
    }
    /// <summary>
    ///  下一节点
    /// </summary>
    public void MoveNext() {
        m_CurrStep.StopTriggerList();
        m_CurrStep.StopControllerList();
        IStepNode stepNode = m_CurrStep.GetNextNode();
        if (stepNode != null)
        {
            stepNode.PrevNode = m_CurrStep;
            m_CurrStep = stepNode;
            stepNode.SetCurrent();
        }
        else {
            StepGraphCompleted?.Invoke();
        }
    }
    /// <summary>
    ///  上一节点
    /// </summary>
    public void MovePrev() {
        m_CurrStep.StartControllerList();
        m_CurrStep.StopTriggerList();
        IStepNode stepNode = m_CurrStep.GetPrevNode();
        if (stepNode != null)
        {
            m_CurrStep = stepNode;
            stepNode.StopControllerList();
            stepNode.SetCurrent();
        }
       
    }
}
