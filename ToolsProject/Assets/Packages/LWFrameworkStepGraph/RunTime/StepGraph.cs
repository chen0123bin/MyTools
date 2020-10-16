using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
using LWNode.LWStepGraph;

[CreateAssetMenu]
public class StepGraph : LWNodeGraph {
    private DataNode m_DataNode;
    [HideInInspector, SerializeField]
    public List<string> m_ObjectArray;
    /// <summary>
    /// 当前Graph全部执行完成
    /// </summary>
    public Action StepGraphCompleted { get; set; }
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStepNode CurrStepNode { get; set; }
    /// <summary>
    /// 对象数据节点
    /// </summary>
    public DataNode CurrDataNode {
        get {
            if (m_DataNode == null) {
                foreach (var item in nodes)
                {
                    
                    if (item.GetType() == typeof(DataNode))
                    {
                        m_DataNode = item as DataNode;
                        break;
                    }
                }
            }
            if (m_DataNode == null) {
                LWDebug.LogError("当前数据没有DataNode");
            }
            return m_DataNode;
        }        
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
        CurrStepNode.StopTriggerList();
        CurrStepNode.StopControllerList();
        IStepNode stepNode = CurrStepNode.GetNextNode();
        if (stepNode != null)
        {
            stepNode.PrevNode = CurrStepNode;
            CurrStepNode = stepNode;
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
        CurrStepNode.StartControllerList();
        CurrStepNode.StopTriggerList();
        IStepNode stepNode = CurrStepNode.GetPrevNode();
        if (stepNode != null)
        {
            CurrStepNode = stepNode;
            stepNode.StopControllerList();
            stepNode.SetCurrent();
        }
       
    }
}
