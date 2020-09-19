using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

public class TestViewLogic : BaseUILogic<TestView>  
{

	public TestViewLogic(TestView view): base(view)
	{
	}
	public void CreateNode() {
		string[] values = new string[] { "aaaa", "bbbbb", "ccccc", "ddddd" };
		_view.Datas = values;
	}
	public void CreateNode2()
	{
		string[] values = new string[] { "11111", "bbb222bb", "ccc3333cc", "dddd4444d" };
		_view.Datas = values;
	}
}
