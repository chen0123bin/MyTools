using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using LWFramework;
using System.Collections.Generic;

[UIViewData("Assets/ExampleStep/Resources/UI/TestView.prefab", FindType.Name, "LWFramework/Canvas/Normal")]
public class TestView : BaseLogicUIView<TestViewLogic>  
{

	[UIElement("Btn1")]
    private Button _btn1=null;
	[UIElement("Btn2")]
    private Button _btn2 =null;
	[UIElement("Lyt/TestNodeTemp")]
    private Transform _testNodeTemp = null;
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
                int index = i;
				TestNodeTemp node = _pool.Spawn();
				node.Text = _datas[i];
                node.OnClick = () =>
                {
                    ChooseIndex(index);
                };
				_list.Add(node);
			}
		}
	}
    public override void CreateView(GameObject go)
    {
        base.CreateView(go);
        _btn1.onClick.AddListener(() => {
            m_Logic.CreateNode();
        });
        _btn2.onClick.AddListener(() => {
            m_Logic.CreateNode2();
        });
        _pool = new GameObjectPool<TestNodeTemp>(5, _testNodeTemp.gameObject);
        _list = new List<TestNodeTemp>();
	}
    private void ChooseIndex(int index) {
        LWDebug.Log(_datas[index]);
        
    }
}
