using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public class StepControllerChangePosi:BaseStepController
{
    [LabelText("移动时间"), LabelWidth(70)]
    public float m_EndMoveTime;
    [LabelText("初始位置"), LabelWidth(70)]
    public Vector3 m_BeginPosi;
    [LabelText("移动位置"), LabelWidth(70)]
    public Vector3 m_EndPosi;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        m_Target.position = m_BeginPosi;
    }

    public override void ControllerEnd()
    {
        m_Target.position = m_EndPosi;
    }

    public override void ControllerExecute()
    {
        m_Target.DOMove(m_EndPosi, m_EndMoveTime).OnComplete(() =>
        {
            m_ControllerCompleted?.Invoke();
        });
    }

   
}
