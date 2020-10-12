using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，处理位移
/// </summary>
public class SC_ChangeScale:BaseStepController
{
    [LabelText("控制对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList")]
    public string m_ObjName;
    [LabelText("变化时间"), LabelWidth(70)]
    public float m_ChangeTime;
    [LabelText("变化大小"), LabelWidth(70)]
    public Vector3[] m_ScaleArray;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        if (m_ScaleArray.Length != 2) {
            LWDebug.LogError("当前节点的Controller的移动参数必须是2个");
        }
        m_Target.localScale = m_ScaleArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.localScale = m_ScaleArray[1];
    }

    public override void ControllerExecute()
    {
        m_Target.DOScale(m_ScaleArray[1], m_ChangeTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_ControllerCompleted?.Invoke();
        });     
    }
#if UNITY_EDITOR
    [Button("选择物体"), LabelWidth(70)]
    public void ChooseObj()
    {
        UnityEditor.Selection.activeObject = StepRuntimeData.Instance.FindGameObject(m_ObjName);
    }
#endif
    [Button("设置数据"), LabelWidth(70)]
    public void SetValue()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        m_ScaleArray[m_ScaleArray.Length - 1] = m_Target.localScale;
    }
    
}
