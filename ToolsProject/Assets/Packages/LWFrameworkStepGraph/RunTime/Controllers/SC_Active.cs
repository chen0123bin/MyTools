using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Xml.Linq;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public class SC_Active : BaseStepObjectController
{
   
    [LabelText("开始Active"), LabelWidth(90)]
    public bool m_BeginActive;
    [LabelText("结束Active"), LabelWidth(90)]
    public bool m_EndActive;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        m_Target.gameObject.SetActive(m_BeginActive);
    }
    public override void ControllerEnd()
    {
        m_Target.gameObject.SetActive(m_EndActive);
    }
    public override void ControllerExecute()
    {
        m_ControllerExecuteCompleted?.Invoke();
    }
    public override XElement ToXml()
    {
        XElement control = new XElement("Control");
        control.Add(new XAttribute("ScriptName", $"{this.GetType()}"));
        control.Add(new XAttribute("ObjectName", $"{m_ObjName}"));
        control.Add(new XAttribute("BeginActive", $"{m_BeginActive}"));
        control.Add(new XAttribute("EndActive", $"{m_EndActive}"));
        control.Add(new XAttribute("Remark", $"{m_Remark}"));
        return control;
    }
    public override void InputXml(XElement xElement)
    {
        m_ObjName = xElement.Attribute("ObjectName").Value;
        m_BeginActive = xElement.Attribute("BeginActive").Value=="true"?true:false;
        m_EndActive = xElement.Attribute("EndActive").Value == "true" ? true : false;
       
    }
}
