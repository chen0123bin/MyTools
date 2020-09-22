using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

public class TestNodeTemp : BaseUINode 
{

	[UIElement("Text")]
	public Text _text;
	public string Text {
		set => _text.text = value;
	}
	public override  void Create(GameObject gameObject)
	{
		base.Create(gameObject);
	}
	public override void OnUnSpawn()
	{
	}
	public override void Release()
	{
		base.Release();
	}
}
