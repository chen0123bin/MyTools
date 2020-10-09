using LWNodeEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
[CustomNodeGraphEditor(typeof(StepGraph))]
public class StepGraphEditor : NodeGraphEditor
{
    private ReorderableList m_ObjectArray;
    public override string GetNodeMenuName(System.Type type)
    {
        if (type.Namespace == "LWNode.LWStepGraph")
        {            
            return base.GetNodeMenuName(type).Replace("LW Node/LW Step Graph/", "");
        }
        else return base.GetNodeMenuName(type);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        m_ObjectArray = new ReorderableList(serializedObject, serializedObject.FindProperty("stringArray")
         , true, true, true, true);
    }
    public override void OnGUI()
    {
        base.OnGUI();
        GUI.Label(new Rect(10, 10, 90, 20), "陈斌");
    }
}
