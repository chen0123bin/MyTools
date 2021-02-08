using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;

public class StepXmlManager:IManager, IStepManager
{
    private List<BaseStepXml> m_BaseStepXmlList;
    /// <summary>
    /// 当前Graph全部执行完成
    /// </summary>
    public Action StepAllCompleted { get; set; }
    /// <summary>
    /// 当前进行中的步骤
    /// </summary>
    public IStep CurrStepNode { get; set; }


    public void Init()
    {
    }

    public void Update()
    {
    }
    public StepXmlManager(string xmlData) {
        m_BaseStepXmlList = new List<BaseStepXml>();
        XElement root = XElement.Parse(xmlData);
        List<XElement>stepList = root.Elements("Node").ToList();
        for (int i = 0; i < stepList.Count; i++)
        {
            BaseStepXml baseStepXml = ConverHelp.Instance.CreateInstance<BaseStepXml>(stepList[i].Attribute("ScriptName").Value);
            baseStepXml.InputXml(stepList[i]);
            m_BaseStepXmlList.Add(baseStepXml);
        }

       
    }
    public void StartStep()
    {
        SetCurrStepNode(m_BaseStepXmlList[0]);
    }


    /// <summary>
    ///  下一节点
    /// </summary>
    public void MoveNext()
    {
        CurrStepNode.StopTriggerList();
        CurrStepNode.StopControllerList();
        IStep stepNode = CurrStepNode.NextNode;
        if (stepNode != null)
        {
            stepNode.PrevNode = CurrStepNode;
            SetCurrStepNode(stepNode);
        }
        else
        {
            StepAllCompleted?.Invoke();
        }
    }
    /// <summary>
    ///  上一节点
    /// </summary>
    public void MovePrev()
    {
        CurrStepNode.StartControllerList();
        CurrStepNode.StopTriggerList();
        IStep stepNode = CurrStepNode.PrevNode;
        if (stepNode != null)
        {
            SetCurrStepNode(stepNode);
            stepNode.StopControllerList();
        }

    }

    private void SetCurrStepNode(IStep step) {
        CurrStepNode = step;
        CurrStepNode.StepManager = this;
        CurrStepNode.SetSelfCurrent();
    }

    public IStep GetNextStepByIndex(int index)
    {
        if (index >= m_BaseStepXmlList.Count) {
            return null;
        }
        return m_BaseStepXmlList[index];
    }
}
