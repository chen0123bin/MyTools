using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，处理旋转
/// </summary>
public class SC_ChangeRot : BaseStepController
{
    [LabelText("控制对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList")]
    public string m_ObjName;
    [LabelText("旋转时间"), LabelWidth(70)]
    public float m_RotTime = 1 ;
    [LabelText("变化角度"), LabelWidth(70)]
    public Vector3[] m_EulerArray;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        if (m_EulerArray.Length != 2)
        {
            LWDebug.LogError("当前节点的Controller的旋转参数必须是2个");
        }
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;       
        m_Target.localEulerAngles = m_EulerArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.localEulerAngles = m_EulerArray[1];
    }

    public override void ControllerExecute()
    {
        Quaternion oldQ;
        Vector3 oldE;
        oldE = m_Target.localEulerAngles;
        m_Target.localEulerAngles = m_EulerArray[1];
        oldQ = m_Target.rotation;
        m_Target.localEulerAngles = oldE;
        m_Target.DORotateQuaternion(oldQ, m_RotTime).SetEase(Ease.Linear).OnComplete(() =>
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
        m_EulerArray[m_EulerArray.Length - 1] = m_Target.localEulerAngles;
    }
}
