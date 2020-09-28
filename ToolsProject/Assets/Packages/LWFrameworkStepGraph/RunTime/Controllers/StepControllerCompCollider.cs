using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public class StepControllerCompCollider : BaseStepController
{
    [LabelText("中心位置"), LabelWidth(70)]
    public Vector3 m_ColliderCenter;
    [LabelText("碰撞大小"), LabelWidth(70)]
    public Vector3 m_ColliderSize;
    [LabelText("开始类型"), LabelWidth(70)]
    public ControllerCompType m_BeginCtrlType;
    [LabelText("结束类型"), LabelWidth(70)]
    public ControllerCompType m_EndCtrlType;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        CtrlComp(m_BeginCtrlType);
    }
    public override void ControllerEnd()
    {
        CtrlComp(m_EndCtrlType);
    }
    public override void ControllerExecute()
    {
        m_ControllerCompleted?.Invoke();
    }
    void CtrlComp(ControllerCompType type) {
        switch (type)
        {
            case ControllerCompType.Add:
                BoxCollider box1 = m_Target.gameObject.GetComponent<BoxCollider>();
                if (box1 == null) {
                    box1 = m_Target.gameObject.AddComponent<BoxCollider>();
                }             
                box1.size = m_ColliderSize;
                box1.center = m_ColliderCenter;
                break;
            case ControllerCompType.Remove:
                BoxCollider box2 = m_Target.gameObject.GetComponent<BoxCollider>();
                if (box2 != null) {
                    GameObject.DestroyImmediate(box2);
                }
                break;
            case ControllerCompType.None:
                break;
            default:
                break;
        }
    }
   
}
