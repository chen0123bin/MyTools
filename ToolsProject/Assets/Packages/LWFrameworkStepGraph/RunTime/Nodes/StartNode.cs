using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWNode;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
namespace LWNode.LWStepGraph
{ 
    [NodeWidth(150)]
    public class StartNode : Node
    {
        [Output, LabelText("开始")]
        public int exit;
        /// <summary>
        /// 步骤Graph
        /// </summary>
        private StepGraph m_StepGraph;
        // Use this for initialization
        protected override void Init() {
		    base.Init();
            m_StepGraph = graph as StepGraph;

      
        }
        public void Start() {
            NodePort exitPort = GetOutputPort("exit");        
            if (!exitPort.IsConnected)
            {
                Debug.LogWarning("exit端口未连接");
                return;
            }      
            IStepNode node = exitPort.GetConnection(0).node as IStepNode;
            m_StepGraph.CurrStep = node;
            node.SetCurrent();
        }
        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port) {
		    return null; // Replace this
	    }
    
    }
}