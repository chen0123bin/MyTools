using LWNode;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace XNode.NodeGroups
{
	[CreateNodeMenu("Group")]
	public class NodeGroup : Node {
		[HideInInspector]
		public int width = 400;
		[HideInInspector]
		public int height = 400;
		/// <summary>
		/// 组背景颜色
		/// </summary>
		[LabelText("背景颜色")]
		public Color m_Color = new Color(1f, 1f, 1f, 0.4f);
		/// <summary>
		/// 备注
		/// </summary>
		[LabelText("备注")]
		public string m_Remark;
		private bool m_ShowAllStepData;
		

		public override object GetValue(NodePort port) {
			return null;
		}

		/// <summary> Gets nodes in this group </summary>
		public List<Node> GetNodes() {
			List<Node> result = new List<Node>();
			foreach (Node node in graph.nodes) {
				if (node == this) continue;
				if (node.position.x < this.position.x) continue;
				if (node.position.y < this.position.y) continue;
				if (node.position.x > this.position.x + width) continue;
				if (node.position.y > this.position.y + height + 30) continue;
				result.Add(node);
			}
			return result;
		}
		[ContextMenu("切换节点状态")]
		public void ShowAllStepDataChange()
		{
			m_ShowAllStepData = !m_ShowAllStepData;
			List<Node> nodes = GetNodes();
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] is BaseStepNode)
				{
					(nodes[i] as BaseStepNode).m_IsShowData = m_ShowAllStepData;
				}
			}
		}
		
	}
}