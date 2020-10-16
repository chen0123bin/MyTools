using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LWNodeEditor;
using static LWNodeEditor.NodeEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Linq;

[CustomNodeEditor(typeof(BaseStepNode))]
public class BaseStepNodeEditor : NodeEditor {
    private float m_SO_UpdateInterval;
    private float m_SO_UpdateIntervalMax=1f;

    public override void OnHeaderGUI() {
		GUI.color = Color.white;
        BaseStepNode node = target as BaseStepNode;
        StepGraph graph = node.graph as StepGraph;
		if (graph.CurrStepNode != null && graph.CurrStepNode.Equals(node)) {
			
			switch (graph.CurrStepNode.CurrState)
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
        if (m_SO_UpdateInterval <= 0) {
            serializedObject.Update();
        }     
        if (node.m_IsShowData)
        {
            inNodeEditor = true;            
            InspectorUtilities.BeginDrawPropertyTree(objectTree, true);
            GUIHelper.PushLabelWidth(84);
            objectTree.Draw(true);
            InspectorUtilities.EndDrawPropertyTree(objectTree);
            GUIHelper.PopLabelWidth();
            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (LWNode.NodePort dynamicPort in target.DynamicPorts)
            {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
           
            // Call repaint so that the graph window elements respond properly to layout changes coming from Odin
            if (GUIHelper.RepaintRequested)
            {
                GUIHelper.ClearRepaintRequest();
                window.Repaint();
            }
            inNodeEditor = false;
        }
        else {
            // Iterate through serialized properties and draw them like the Inspector (But with ports)         
            string[] excludes = { "m_Script", "graph", "position", "ports" };
            // Iterate through serialized properties and draw them like the Inspector (But with ports)
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (excludes.Contains(iterator.name)) continue;
                NodeEditorGUILayout.PropertyField(iterator, true);
            }

            // Iterate through dynamic ports and draw them in the order in which they are serialized
            foreach (LWNode.NodePort dynamicPort in target.DynamicPorts)
            {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }
           
        }
        if (m_SO_UpdateInterval <= 0)
        {
            serializedObject.ApplyModifiedProperties();
            m_SO_UpdateInterval = m_SO_UpdateIntervalMax;
        }
        m_SO_UpdateInterval -= Time.deltaTime;
        //serializedObject.ApplyModifiedProperties();   

    }
}
