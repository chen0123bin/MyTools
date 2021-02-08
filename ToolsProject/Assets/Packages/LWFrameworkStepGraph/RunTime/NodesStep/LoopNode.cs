using LWNode;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWNode.LWStepGraph {
    public class LoopNode : BaseStepNode
    {
        [LabelText("循环初始值")]
        public int m_StartValue;
        [LabelText("累加数值"), InfoBox("当累加数值为0时为无限循环")]
        public int m_AddValue = 1;
        protected override void Init()
        {
            base.Init();
            m_NextIndex = m_StartValue - 1;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            return null; // Replace this
        }
        public override void StartTriggerList()
        {
            m_NextIndex += m_AddValue;
            m_StepManager.MoveNext();
        }

        public override void StopTriggerList()
        {
        }
    }
}

