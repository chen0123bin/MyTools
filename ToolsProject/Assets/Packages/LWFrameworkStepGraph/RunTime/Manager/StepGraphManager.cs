using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
using LWNode.LWStepGraph;

[CreateAssetMenu]
public class StepGraphManager : LWNodeGraph,IManager, IStepManager
{
    private DataNode m_DataNode;
    [HideInInspector, SerializeField]
    public List<string> m_ObjectArray;
    /// <summary>
    /// 当前Graph全部执行完成
    /// </summary>
    public Action StepAllCompleted { get; set; }
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStep CurrStepNode { get; set; }
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
    public void StartStep() {
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
        IStep stepNode = CurrStepNode.NextNode;
        if (stepNode != null)
        {
            stepNode.PrevNode = CurrStepNode;
            CurrStepNode = stepNode;
            stepNode.SetSelfCurrent();
        }
        else {
            StepAllCompleted?.Invoke();
        }
    }
    /// <summary>
    ///  上一节点
    /// </summary>
    public void MovePrev() {
        CurrStepNode.StartControllerList();
        CurrStepNode.StopTriggerList();
        IStep stepNode = CurrStepNode.PrevNode;
        if (stepNode != null)
        {
            CurrStepNode = stepNode;
            stepNode.StopControllerList();
            stepNode.SetSelfCurrent();
        }
       
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public IStep GetNextStepByIndex(int index)
    {
        throw new NotImplementedException();
    }
}
