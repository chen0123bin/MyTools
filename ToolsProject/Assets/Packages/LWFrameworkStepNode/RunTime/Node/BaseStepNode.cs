using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[NodeWidth(260)]
public class BaseStepNode : Node, IStepNode
{

    [LabelText("进入")]
    [Input] public int enter;
    [LabelText("退出")]
    [Output] public int exit;
    public int nextIndex;
    [TextArea]
    [LabelText("描述")]
    [SerializeField]
    private string _remark;
    [Button("下一步")]
    public void MoveNext()
    {
        StepGraph fmGraph = graph as StepGraph;
        if (fmGraph.CurrStep != this)
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
        if (fmGraph.CurrStep != null)
        {
            fmGraph.CurrStep.OnExit();
        }
        IStepNode node = exitPort.GetConnection(nextIndex).node as IStepNode;
        node.OnEnter();
    }
    [Button("设置当前")]
    public void SetCurrent()
    {
        StepGraph fmGraph = graph as StepGraph;
        fmGraph.CurrStep = this;
    }
    public virtual void OnEnter()
    {
        StepGraph fmGraph = graph as StepGraph;
        fmGraph.CurrStep = this;
    }
    public virtual void OnExit()
    {

    }
    public override object GetValue(NodePort port)
    {
        return nextIndex;
    }
}