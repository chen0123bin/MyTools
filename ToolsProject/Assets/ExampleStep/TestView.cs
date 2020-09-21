using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using LWFramework;
using System.Collections.Generic;

[UIViewData("Assets/ExampleStep/Resources/UI/TestView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class TestView : BaseLogicUIView<TestViewLogic>  
{

	[UIElement("Btn1")]
	public Button _btn1;
	[UIElement("Btn2")]
	public Button _btn2;
	[UIElement("Lyt/TestNodeTemp")]
	public Transform _testNodeTemp;
	private GameObjectPool<TestNodeTemp> _pool;
	private List<TestNodeTemp> _list;
	private string[] _datas;
	public string[] Datas {
		set {
			_datas = value;
			_list.ForEach(item => _pool.Unspawn(item));
			_list.Clear();
			for (int i = 0; i < _datas.Length; i++)
			{
				TestNodeTemp node = _pool.Spawn();
				node.Text = _datas[i];
				_list.Add(node);
			}
		}
	}
	public override  void OnCreateView()
	{
		_btn1.onClick.AddListener(() => 		{
			m_Logic.CreateNode();
		});
		_btn2.onClick.AddListener(() => {
			m_Logic.CreateNode2();
		});
		_pool = new GameObjectPool<TestNodeTemp>(5, _testNodeTemp.gameObject);
		_list = new List<TestNodeTemp>();
	}
}
