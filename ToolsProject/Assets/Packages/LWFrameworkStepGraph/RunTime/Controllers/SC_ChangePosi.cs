using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，处理位移
/// </summary>
public class SC_ChangePosi:BaseStepController
{
    [LabelText("控制对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList")]
    public string m_ObjName;
    [LabelText("移动时间"), LabelWidth(70)]
    public float m_MoveTime = 1;
    [LabelText("移动位置"), LabelWidth(70)]
    public Vector3[] m_PosiArray;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        if (m_PosiArray.Length < 2) {
            LWDebug.LogError("当前节点的Controller的移动参数少于2个");
        }
        m_Target.localPosition = m_PosiArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.localPosition = m_PosiArray[m_PosiArray.Length-1];
    }

    public override void ControllerExecute()
    {
        m_Target.DOLocalPath(m_PosiArray, m_MoveTime).SetEase(Ease.Linear).OnComplete(() =>
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
        m_PosiArray[m_PosiArray.Length - 1] = m_Target.localPosition;
    }
    
}
