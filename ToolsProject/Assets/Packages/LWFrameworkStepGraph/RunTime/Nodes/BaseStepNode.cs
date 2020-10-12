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
    protected StepGraph m_StepGraph;
    /// <summary>
    /// 下一步骤的脚标
    /// </summary>
    protected int m_NextIndex;
   
    protected StepNodeState m_CurrState;
    /// <summary>
    /// 当前的状态
    /// </summary>
    public StepNodeState CurrState { get => m_CurrState; set => m_CurrState = value; }
  
    protected IStepNode m_PrevNode;
    /// <summary>
    /// 上一节点
    /// </summary>
    public IStepNode PrevNode { get => m_PrevNode; set => m_PrevNode = value; }
    protected override void Init()
    {
        base.Init();
        m_StepGraph = graph as StepGraph;
    }
    public override object GetValue(NodePort port)
    {
        return m_NextIndex;
    }
    // 移除下一步的函数，放置到Graph中  [Button("下一步")]
   // public void MoveNext()
   // {
   //     if (m_StepGraph.CurrStep!=null&&!m_StepGraph.CurrStep.Equals(this))
   //     {
   //         Debug.LogWarning("当前节点不是选中状态");
   //         return;
   //     }       
   //     if (m_StepGraph.CurrStep != null)
   //     {
   //         m_StepGraph.CurrStep.StopTriggerList();
   //         m_StepGraph.CurrStep.StopControllerList();         
   //     }
   //     NodePort exitPort = GetOutputPort("exit");
   //     if (!exitPort.IsConnected)
   //     {
   //         Debug.LogWarning("exit端口未连接");
   //         m_StepGraph.StepGraphCompleted?.Invoke();
   //         return;
   //     }
   //     IStepNode node = exitPort.GetConnection(m_NextIndex).node as IStepNode;
   //     node.SetCurrent();
   // }
   //// [Button("上一步")]
   // public void MovePrev()
   // {
   //     if (m_StepGraph.CurrStep != null && !m_StepGraph.CurrStep.Equals(this))
   //     {
   //         Debug.LogWarning("当前节点不是选中状态");
   //         return;
   //     }
   //     if (m_StepGraph.CurrStep != null)
   //     {
   //         m_StepGraph.CurrStep.StartControllerList();
   //         m_StepGraph.CurrStep.StopTriggerList();
   //     }
   //     NodePort enterPort = GetInputPort("enter");
   //     if (!enterPort.IsConnected)
   //     {
   //         Debug.LogWarning("enter端口未连接");
            
   //         return;
   //     }        
   //     IStepNode node = enterPort.GetConnection(0).node as IStepNode;
   //     node.StopControllerList();
   //     node.SetCurrent();

   // }
    //[Button("设置当前")]
    public void SetCurrent()
    {
       // m_StepGraph.CurrStep = this;
        StartControllerList();
        StartTriggerList();
        Message msg = MessagePool.GetMessage(nameof(StepCommonMessage.StepHelpMessage));
        msg["HelpText"] = m_Remark;
        MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(msg);
    }
    public abstract void StartTriggerList();
    public abstract void StopTriggerList();
    public virtual void StartControllerList()
    {
        m_CurrState = StepNodeState.Wait;
    }
    public virtual void StopControllerList()
    {
        m_CurrState = StepNodeState.Complete;
    }
   
   

    public IStepNode GetNextNode()
    {
        NodePort exitPort = GetOutputPort("exit");
        if (!exitPort.IsConnected)
        {
            Debug.LogWarning("exit端口未连接");
            m_StepGraph.StepGraphCompleted?.Invoke();
            return null;
        }
        IStepNode node = exitPort.GetConnection(m_NextIndex).node as IStepNode;
        return node;
    }

    public IStepNode GetPrevNode()
    {
        return m_PrevNode;       
    }

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