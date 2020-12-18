using LWNode;
using LWNode.LWStepGraph;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeWidth(300)]
public class SearchNode : Node
{
    private StepGraph m_StepGraph;
    // Use this for initialization
    protected override void Init() {
		base.Init();
        m_StepGraph = graph as StepGraph;
    }

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

  

#if UNITY_EDITOR
    ///搜索的功能
    public string m_SearchValue;
    public SearchType m_SearchType;
    public int m_SearchIndex = 0;
    [Button("下一个")]
    public void OnSearchNextClick()
    {
        SearchAllNode(1);
    }
    [Button("上一个")]
    public void OnSearchPrevClick()
    {
        SearchAllNode(-1);
       
    }
    void SearchAllNode(int value) {
        for (int i = m_SearchIndex + value; i >= 0&&i< m_StepGraph.nodes.Count; i+=value)
        {
            
            var item = m_StepGraph.nodes[i];
            if (item.GetType() == typeof(StepNode))
            {
                var itemStepNode = item as StepNode;
                if (SearchResult(itemStepNode))
                {
                    m_SearchIndex = i;
                    break;
                }
            }
        }
    }
    bool SearchResult(StepNode stepNode) {
        bool result = false;
        switch (m_SearchType)
        {
            case SearchType.StepNodeRemark:
                result =  SearchNodeRemark(stepNode);
                break;
            case SearchType.StepObjetName:
                result = SearchStepObjetName(stepNode);
                break;
            case SearchType.SC_Remark:
                result = SearchSC_Remark(stepNode);
                break;
            default:
                break;
        }
        return result;
    }
    /// <summary>
    /// 根据备注搜索
    /// </summary>
    /// <param name="stepNode"></param>
    /// <returns></returns>
    bool SearchNodeRemark(StepNode stepNode) {
        if (stepNode.m_Remark == m_SearchValue)
        {
            UnityEditor.Selection.activeObject = stepNode;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 根据控制器物体搜索
    /// </summary>
    /// <param name="stepNode"></param>
    /// <returns></returns>
    bool SearchStepObjetName(StepNode stepNode)
    {
        for (int i = 0; stepNode.m_StepControllerList !=null&& i < stepNode.m_StepControllerList.Count; i++)
        {
            if (stepNode.m_StepControllerList[i] is BaseStepObjectController)
            {
                var ctrl = stepNode.m_StepControllerList[i] as BaseStepObjectController;
                if (ctrl.m_ObjName == m_SearchValue)
                {
                    UnityEditor.Selection.activeObject = stepNode;
                    return true;
                }
            }
            
        }
        return false;
    }
    /// <summary>
    /// 根据控制器备注搜索
    /// </summary>
    /// <param name="stepNode"></param>
    /// <returns></returns>
    bool SearchSC_Remark(StepNode stepNode)
    {
        for (int i = 0; stepNode.m_StepControllerList != null && i < stepNode.m_StepControllerList.Count; i++)
        {
            if (stepNode.m_StepControllerList[i].Remark == m_SearchValue) {
                UnityEditor.Selection.activeObject = stepNode;
                return true;
            }
        }
        return false;
    }
    public enum SearchType
    {
        StepNodeRemark,
        StepObjetName,
        SC_Remark
    }
#endif
}
