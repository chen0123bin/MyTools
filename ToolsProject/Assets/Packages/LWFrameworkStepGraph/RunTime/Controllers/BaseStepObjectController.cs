﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public abstract class BaseStepObjectController: BaseStepController
{
    [LabelText("控制对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList"), HorizontalGroup]
    public string m_ObjName;
    public List<string> GetSceneObjectList()
    {
        return StepRuntimeData.Instance.SceneObjectNameList;
    }

#if UNITY_EDITOR
    [Button("选中"), HorizontalGroup(30)]
    public virtual void ChooseObj()
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
