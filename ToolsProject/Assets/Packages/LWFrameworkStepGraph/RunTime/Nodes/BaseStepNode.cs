using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[NodeWidth(300), ShowOdinSerializedPropertiesInInspector]
public class BaseStepNode : Node, IStepNode, ISerializationCallbackReceiver
{
    private StepGraph m_StepGraph;
    [Input, LabelText("进入")] 
    public int enter;
    [Output, LabelText("退出")] 
    public int exit;
    public int m_NextIndex;
    [TextArea]
    [LabelText("描述")]
    [SerializeField]
    private string _remark;
    [Button("下一步")]
    public void MoveNext()
    {
        if (m_StepGraph.CurrStep!=null&&!m_StepGraph.CurrStep.Equals(this))
        {
            Debug.LogWarning("当前节点不是选中状态");
            return;
        }
        NodePort exitPort = GetOutputPort("exit");
        if (!exitPort.IsConnected)
        {
            Debug.LogWarning("exit端口未连接");
            return;
        }
        if (m_StepGraph.CurrStep != null)
        {
            m_StepGraph.CurrStep.OnExit();
        }
        IStepNode node = exitPort.GetConnection(m_NextIndex).node as IStepNode;
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
       
    }
    public virtual void OnExit()
    {

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