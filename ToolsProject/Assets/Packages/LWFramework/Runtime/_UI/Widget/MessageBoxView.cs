using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using System;

[UIViewData("",FindType.Name, "LWFramework/Canvas/Top")]
public class MessageBoxView : BaseUIView 
{

	[UIElement("ImgBox/LytBtns/BtnCancel/TxtCancel")]
	public Text _txtCancel;
	[UIElement("ImgBox/LytBtns/BtnConfirm/TxtConfirm")]
	public Text _txtConfirm;
	[UIElement("ImgBox/LytBtns/BtnCancel")]
	public Button _btnCancel;
	[UIElement("ImgBox/LytBtns/BtnConfirm")]
	public Button _btnConfirm;
	[UIElement("ImgBox/TxtMsg")]
	public Text _txtMsg;
    /// <summary>
    /// 按钮点击操作
    /// </summary>
    public Action<bool> OnBtnClick { get; set; }
    /// <summary>
    /// 放弃按钮文字
    /// </summary>
    public string CancelStr {
        set => _txtCancel.text = value;
    }
    /// <summary>
    /// 确认按钮文字
    /// </summary>
    public string ConfirmStr
    {
        set => _txtConfirm.text = value;
    }
    /// <summary>
    /// 提示内容
    /// </summary>
    public string MsgStr
    {
        set => _txtConfirm.text = value;
    }
    /// <summary>
    /// 按钮数量
    /// </summary>
    public int BtnCount { get; set; } = 2;
    public override  void OnCreateView()
	{
        _btnConfirm.onClick.AddListener(() => 		{
            HandleEvent(true);
        });

        _btnCancel.onClick.AddListener(() => 		{
            HandleEvent(false);
        });
	}
    public override void OpenView()
    {
        base.OpenView();
        if (BtnCount == 1) {
            _btnCancel.gameObject.SetActive(false);
        }
    }
    public override void CloseView()
    {
        base.CloseView();
        _btnCancel.gameObject.SetActive(true);
        _btnConfirm.gameObject.SetActive(true);
        BtnCount = 2;
    }
    private void HandleEvent(bool isOk)
    {
        OnBtnClick?.Invoke(isOk);
        OnBtnClick = null;
        CloseView();
    }
}
