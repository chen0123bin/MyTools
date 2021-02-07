using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

/// <summary>
/// 步骤控制器，处理位移，延迟位移
/// </summary>
public class StepControllerChangePosiDeley: BaseStepObjectController
{
   
    [LabelText("延迟时间"), LabelWidth(90)]
    public float m_TimeDeley = 0;
    [LabelText("移动时间"), LabelWidth(70)]
    public float m_MoveTime;
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
        DoPath();

    }
    void DoPath() {
        m_Target.DOLocalPath(m_PosiArray, m_MoveTime).SetDelay(m_TimeDeley).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_ControllerExecuteCompleted?.Invoke();
        });
    }

    [Button("设置数据"), LabelWidth(70)]
    public void SetValue()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        m_PosiArray[m_PosiArray.Length - 1] = m_Target.localPosition;
    }

}
