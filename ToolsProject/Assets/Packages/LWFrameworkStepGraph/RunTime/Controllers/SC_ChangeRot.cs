using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
/// <summary>
/// 步骤控制器，处理旋转
/// </summary>
public class SC_ChangeRot : BaseStepObjectController
{
    [LabelText("旋转时间"), LabelWidth(70)]
    public float m_RotTime = 1 ;
    [LabelText("变化角度"), LabelWidth(70), ListDrawerSettings(CustomAddFunction = "AddEulerValue", OnTitleBarGUI = ("SetValue"))]
    public Vector3[] m_EulerArray;
    private Transform m_Target;
    public override void ControllerBegin()
    {
    
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;       
        m_Target.localEulerAngles = m_EulerArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.localEulerAngles = m_EulerArray[m_EulerArray.Length - 1];
    }

    public override void ControllerExecute()
    {
        //Quaternion oldQ;
        //Vector3 oldE;
        //oldE = m_Target.localEulerAngles;
        //m_Target.localEulerAngles = m_EulerArray[1];
        //oldQ = m_Target.rotation;
        //m_Target.localEulerAngles = oldE;
        
        //m_Target.DORotateQuaternion(oldQ, m_RotTime).SetEase(Ease.Linear).OnComplete(() =>
        //{
        //    m_ControllerCompleted?.Invoke();
        //});



        Sequence sequence = DOTween.Sequence();
        float changeTimeUnit = m_RotTime / (m_EulerArray.Length-1);
        for (int i = 1; i < m_EulerArray.Length; i++)
        {
            Quaternion oldQ;
            Vector3 oldE;
            oldE = m_Target.localEulerAngles;
            m_Target.localEulerAngles = m_EulerArray[i];
            oldQ = m_Target.rotation;
            m_Target.localEulerAngles = oldE;

            sequence.Append(m_Target.DORotateQuaternion(oldQ, changeTimeUnit).SetEase(Ease.Linear));
          //  sequence.Append(m_Target.DOScale(m_ScaleArray[i], changeTimeUnit).SetEase(Ease.Linear));
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
            m_EulerArray[m_EulerArray.Length - 1] = m_Target.localEulerAngles;
        }
    }
    public void AddEulerValue()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        Array.Resize<Vector3>(ref m_EulerArray, m_EulerArray.Length + 1);
        m_EulerArray[m_EulerArray.Length - 1] = m_Target.localEulerAngles;
    }
#endif
}
