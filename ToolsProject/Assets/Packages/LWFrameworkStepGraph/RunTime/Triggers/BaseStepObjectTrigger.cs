
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BaseStepObjectTrigger : BaseStepTrigger
{
    [LabelText("触发对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList"), HorizontalGroup]
    public string m_ObjName;
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }
#if UNITY_EDITOR
    [Button("选中"), HorizontalGroup(30)]
    public void ChooseObj()
    {
        UnityEditor.Selection.activeObject = StepRuntimeData.Instance.FindGameObject(m_ObjName);
        //UnityEditor.SceneView.FrameLastActiveSceneView();
        GameObject chooseObj = UnityEditor.Selection.activeObject as GameObject;
        FocusPosition(chooseObj.transform.position);
    }
    void FocusPosition(Vector3 pos)
    {
        UnityEditor.SceneView.lastActiveSceneView.Frame(new Bounds(pos, Vector3.one), false);
    }
#endif
}
