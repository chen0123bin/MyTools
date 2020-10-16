using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

/// <summary>
/// 步骤控制器，处理位移
/// </summary>
public class SC_ChangeScale: BaseStepObjectController
{
   
    [LabelText("变化时间"), LabelWidth(70)]
    public float m_ChangeTime;
    [LabelText("变化大小"), LabelWidth(70), ListDrawerSettings(CustomAddFunction = "AddScaleValue", OnTitleBarGUI = ("SetValue"))]
    public Vector3[] m_ScaleArray;


    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        
        
        m_Target.localScale = m_ScaleArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.localScale = m_ScaleArray[m_ScaleArray.Length - 1];
    }

    public override void ControllerExecute()
    {
        Sequence sequence = DOTween.Sequence();
        float changeTimeUnit = m_ChangeTime / (m_ScaleArray.Length-1);
        for (int i = 1; i < m_ScaleArray.Length; i++)
        {
            sequence.Append( m_Target.DOScale(m_ScaleArray[i], changeTimeUnit).SetEase(Ease.Linear));
        }
        sequence.OnComplete(() =>
        {
            m_ControllerExecuteCompleted?.Invoke();
        });

      
    }
#if UNITY_EDITOR
    
    public void SetValue()
    {
        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
        {
            m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
            m_ScaleArray[m_ScaleArray.Length - 1] = m_Target.localScale;
        }
    }
    public void AddScaleValue()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        Array.Resize<Vector3>(ref m_ScaleArray, m_ScaleArray.Length + 1);
        m_ScaleArray[m_ScaleArray.Length - 1] = m_Target.localScale;
    }
#endif
}
