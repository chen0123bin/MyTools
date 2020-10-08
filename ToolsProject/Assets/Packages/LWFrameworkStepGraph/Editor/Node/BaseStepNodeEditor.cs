using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LWNodeEditor;
using static LWNodeEditor.NodeEditor;

[CustomNodeEditor(typeof(BaseStepNode))]
public class BaseStepNodeEditor : NodeEditor {
	public override void OnHeaderGUI() {
		GUI.color = Color.white;
        BaseStepNode node = target as BaseStepNode;
        StepGraph graph = node.graph as StepGraph;
		if (graph.CurrStep != null && graph.CurrStep.Equals(node)) {
			
			switch (graph.CurrStep.CurrState)
			{
				case StepNodeState.Wait:
					GUI.color = Color.red;
					break;
				case StepNodeState.Execute:
					GUI.color = Color.blue;
					break;
				case StepNodeState.Complete:
					GUI.color = Color.green;
					break;
				default:
					break;
			}
		} 
		string title = target.name;
		GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
		GUI.color = Color.white;
	}

	public override void OnBodyGUI()
	{
		BaseStepNode node = target as BaseStepNode;
		node.m_IsShowData = EditorGUILayout.Toggle("显示数据 ", node.m_IsShowData);
		if (!node.m_IsShowData)
		{
			return;
		}
		base.OnBodyGUI();
	}
}
