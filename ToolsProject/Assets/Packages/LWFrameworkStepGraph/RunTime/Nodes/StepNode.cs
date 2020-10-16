﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace LWNode.LWStepGraph
{
    public class StepNode : BaseStepNode
    {
        [LabelText("触发器集合")]
        public List<IStepTrigger> m_StepTriggerList;
        [LabelText("控制器集合")]
        public List<IStepController> m_StepControllerList;
        /// <summary>
        /// 执行完成的数量
        /// </summary>
        private int m_CompletedCount;
        // Use this for initialization
        protected override void Init()
        {
            base.Init();

        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }
        public override void StartTriggerList()
        {
            for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
            {
                m_StepTriggerList[i].TriggerBegin();
                m_StepTriggerList[i].TiggerActionCompleted = OnTiggerActionCompleted;
                m_StepTriggerList[i].CurrStepGraph = m_StepGraph;
               
            }
        }

        public override void StopTriggerList()
        {
            for (int i = 0; m_StepTriggerList != null && i < m_StepTriggerList.Count; i++)
            {
                m_StepTriggerList[i].TiggerActionCompleted = null;
                m_StepTriggerList[i].CurrStepGraph = null;
                m_StepTriggerList[i].TriggerEnd();
            }
        }

        public override void StartControllerList()
        {
            base.StartControllerList();
            m_CompletedCount = 0;
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].ControllerBegin();
                m_StepControllerList[i].ControllerExecuteCompleted = OnControllerExecuteCompleted;
                m_StepControllerList[i].CurrStepGraph = m_StepGraph;
            }
            //如果没有触发器直接开始执行控制器
            if (m_StepTriggerList == null || m_StepTriggerList.Count == 0)
            {
                OnTiggerActionCompleted(0);
            }
        }

        public override void StopControllerList()
        {
            base.StopControllerList();
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].ControllerExecuteCompleted = null;
                m_StepControllerList[i].ControllerEnd();
            }
        }
        void OnTiggerActionCompleted(int index)
        {
            m_NextIndex = index;
            m_CurrState = StepNodeState.Execute;
            for (int i = 0; m_StepControllerList != null && i < m_StepControllerList.Count; i++)
            {
                m_StepControllerList[i].ControllerExecute();
            }
            //如果没有控制器直接进入下一步
            if (m_StepControllerList == null || m_StepControllerList.Count == 0)
            {
                m_StepGraph.MoveNext();
            }
        
        }

        private void OnControllerExecuteCompleted()
        {
            m_CompletedCount++;
            if (m_CompletedCount == m_StepControllerList.Count)
            {
                m_StepGraph.MoveNext();
            }
        }

       

    }
}