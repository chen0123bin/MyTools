using LWFramework.Core;
using LWFramework.Message;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
[NodeWidth(300), ShowOdinSerializedPropertiesInInspector]
public abstract class BaseStepNode : Node, IStepNode, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    [HideInInspector]
    public bool m_IsShowData = true;

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
   // [Button("下一步")]
    public void MoveNext()
    {
        if (m_StepGraph.CurrStep!=null&&!m_StepGraph.CurrStep.Equals(this))
        {
            Debug.LogWarning("当前节点不是选中状态");
            return;
        }       
        if (m_StepGraph.CurrStep != null)
        {
            m_StepGraph.CurrStep.StopTrigger();
            m_StepGraph.CurrStep.StopController();         
        }
        NodePort exitPort = GetOutputPort("exit");
        if (!exitPort.IsConnected)
        {
            Debug.LogWarning("exit端口未连接");
            m_StepGraph.StepGraphCompleted?.Invoke();
            return;
        }
        IStepNode node = exitPort.GetConnection(m_NextIndex).node as IStepNode;
        node.SetCurrent();
    }
   // [Button("上一步")]
    public void MovePrev()
    {
        if (m_StepGraph.CurrStep != null && !m_StepGraph.CurrStep.Equals(this))
        {
            Debug.LogWarning("当前节点不是选中状态");
            return;
        }
        if (m_StepGraph.CurrStep != null)
        {
            m_StepGraph.CurrStep.StartController();
            m_StepGraph.CurrStep.StopTrigger();
        }
        NodePort enterPort = GetInputPort("enter");
        if (!enterPort.IsConnected)
        {
            Debug.LogWarning("enter端口未连接");
            
            return;
        }        
        IStepNode node = enterPort.GetConnection(0).node as IStepNode;
        node.StopController();
        node.SetCurrent();

    }
    //[Button("设置当前")]
    public void SetCurrent()
    {
        m_StepGraph.CurrStep = this;
        m_StepGraph.CurrStep.StartController();
        m_StepGraph.CurrStep.StartTrigger();
        Message msg = MessagePool.GetMessage(nameof(StepCommonMessage.StepHelpMessage));
        msg["HelpText"] = m_Remark;
        MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
    }
    public virtual void StartController()
    {
        m_CurrState = StepNodeState.Wait;
    }
    public virtual void StopController()
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
    public abstract void StartTrigger();
    public abstract void StopTrigger();

    #region 序列号标准代码
    [SerializeField, HideInInspector]
    private SerializationData serializationData;
    SerializationData ISupportsPrefabSerialization.SerializationData { get { return this.serializationData; } set { this.serializationData = value; } }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }
    #endregion

}