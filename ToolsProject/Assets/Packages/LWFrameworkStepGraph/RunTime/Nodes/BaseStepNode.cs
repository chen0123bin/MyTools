﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[NodeWidth(300), ShowOdinSerializedPropertiesInInspector]
public class BaseStepNode : Node, IStepNode, ISerializationCallbackReceiver
{  
    [Input, LabelText("进入")] 
    public int enter;
    [Output, LabelText("退出")] 
    public int exit;    
    [TextArea,LabelText("描述")]
    public string m_Remark;
    /// <summary>
    /// 步骤Graph
    /// </summary>
    private StepGraph m_StepGraph;
    /// <summary>
    /// 下一步骤的脚标
    /// </summary>
    protected int m_NextIndex;
    /// <summary>
    /// 当前的状态
    /// </summary>
    protected StepNodeState m_CurrState;
    public StepNodeState CurrState { get => m_CurrState; set => m_CurrState = value; }
    [Button("下一步")]
    public void MoveNext()
    {
        if (m_StepGraph.CurrStep!=null&&!m_StepGraph.CurrStep.Equals(this))
        {
            Debug.LogWarning("当前节点不是选中状态");
            return;
        }       
        if (m_StepGraph.CurrStep != null)
        {
            m_StepGraph.CurrStep.OnExit();
        }
        NodePort exitPort = GetOutputPort("exit");
        if (!exitPort.IsConnected)
        {
            Debug.LogWarning("exit端口未连接");
            return;
        }
        IStepNode node = exitPort.GetConnection(m_NextIndex).node as IStepNode;
        m_StepGraph.CurrStep = node;
        m_StepGraph.CurrStep.OnEnter();
    }
    [Button("上一步")]
    public void MovePrev()
    {
        if (m_StepGraph.CurrStep != null && !m_StepGraph.CurrStep.Equals(this))
        {
            Debug.LogWarning("当前节点不是选中状态");
            return;
        }
        if (m_StepGraph.CurrStep != null)
        {
            m_StepGraph.CurrStep.OnEnter();
        }
        NodePort enterPort = GetInputPort("enter");
        if (!enterPort.IsConnected)
        {
            Debug.LogWarning("enter端口未连接");
            return;
        }
        
        IStepNode node = enterPort.GetConnection(0).node as IStepNode;
        m_StepGraph.CurrStep = node;
        m_StepGraph.CurrStep.OnEnter();
    }
    [Button("设置当前")]
    public void SetCurrent()
    {
        m_StepGraph.CurrStep = this;
        m_StepGraph.CurrStep.OnEnter();
    }
    public virtual void OnEnter()
    {
        m_CurrState = StepNodeState.Wait;
    }
    public virtual void OnExit()
    {
        m_CurrState = StepNodeState.Complete;
    }
    protected override void Init()
    {
        base.Init();
        m_StepGraph = graph as StepGraph;
    }
    public override object GetValue(NodePort port)
    {
        return m_NextIndex;
    }

    [SerializeField, HideInInspector]
    private SerializationData serializationData;

   

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }

}