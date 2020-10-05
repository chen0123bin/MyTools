using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，处理旋转
/// </summary>
public class StepControllerChangeRot : BaseStepController
{
    [LabelText("旋转时间"), LabelWidth(70)]
    public float m_RotTime;
    [LabelText("开始角度"), LabelWidth(70)]
    public Vector3 m_BeginEuler;
    [LabelText("结束角度"), LabelWidth(70)]
    public Vector3 m_EndEuler;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;       
        m_Target.localEulerAngles = m_BeginEuler;
    }

    public override void ControllerEnd()
    {
        m_Target.localEulerAngles = m_EndEuler;
    }

    public override void ControllerExecute()
    {
        //m_Target.DOLocalRotate(m_EndEuler, m_RotTime).SetEase(Ease.Linear).OnComplete(() =>
        //{
        //    m_ControllerCompleted?.Invoke();
        //});



        Quaternion oldQ;
        Vector3 oldE;
        oldE = m_Target.localEulerAngles;
        m_Target.localEulerAngles = m_EndEuler;
        oldQ = m_Target.rotation;
        m_Target.localEulerAngles = oldE;
        m_Target.DORotateQuaternion(oldQ, m_RotTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_ControllerCompleted?.Invoke();
        });
    }

   
}
