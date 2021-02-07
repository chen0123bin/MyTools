using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

[UIViewData("", FindType.Name, "LWFramework/Canvas/Top")]
public class LoadAssetBarView : BaseUIView 
{
    [UIElement("TxtLoadMsg")]
    private Text m_TxtLoadMsg = null;
    [UIElement("TxtLoadValue")]
	private Text m_TxtLoadValue = null;
	[UIElement("ImgLoadBar")]
	private Image m_ImgLoadBar = null;
	public override  void CreateView(GameObject go)
	{
		base.CreateView(go);
	}
    public override void OpenView()
    {
        base.OpenView();
        SetLoadValue(0);
        SetLoadMsg("");
    }
    public override void CloseView()
    {
        base.CloseView();
        SetLoadValue(0);
        SetLoadMsg("");
    }
    public void SetLoadValue(float value) {
        m_ImgLoadBar.fillAmount = value;
        m_TxtLoadValue.text = (value*100).ToString("0.0")+ "%";
    }
    public void SetLoadMsg(string msg)
    {
        m_TxtLoadMsg.text = msg;
    }
}
