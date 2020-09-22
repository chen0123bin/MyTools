﻿using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using System;

[UIViewData("",FindType.Tag, "DefaultUI")]
public class MessageBoxView : BaseUIView 
{

	[UIElement("ImgBox/LytBtns/BtnCancel/TxtCancel")]
	private Text _txtCancel;
	[UIElement("ImgBox/LytBtns/BtnConfirm/TxtConfirm")]
    private Text _txtConfirm;
	[UIElement("ImgBox/LytBtns/BtnCancel")]
    private Button _btnCancel;
	[UIElement("ImgBox/LytBtns/BtnConfirm")]
    private Button _btnConfirm;
	[UIElement("ImgBox/TxtMsg")]
    private Text _txtMsg;
    [UIElement("ImgBox/TxtTitle")]
    private Text _txtTitle;
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
        set => _txtMsg.text = value;
    }
    /// <summary>
    /// 提示标题
    /// </summary>
    public string TitleStr
    {
        set => _txtTitle.text = value;
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
