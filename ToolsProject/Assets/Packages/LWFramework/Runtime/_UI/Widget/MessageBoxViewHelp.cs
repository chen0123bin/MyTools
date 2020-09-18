using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxViewHelp:Singleton<MessageBoxViewHelp>
{
    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="uiGameObject"></param>
    /// <param name="msgContent"></param>
    /// <param name="OnBtnClick"></param>
    /// <param name="titleStr"></param>
    /// <param name="confirmStr"></param>
    /// <param name="cancelStr"></param>
    public void OpenMessageBox(string viewName,GameObject uiGameObject, string msgContent, Action<bool> OnBtnClick, string titleStr = "提示", string confirmStr = "确定",string cancelStr = "取消") {
        MainManager.Instance.GetManager<IUIManager>().OpenView<MessageBoxView>(viewName, uiGameObject);
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<IUIManager>().GetView<MessageBoxView>(viewName);
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, titleStr,confirmStr, cancelStr);
    }
    public void OpenMessageBox(string viewName, string msgContent, Action<bool> OnBtnClick, string titleStr = "提示", string confirmStr = "确定", string cancelStr = "取消")
    {
        MainManager.Instance.GetManager<IUIManager>().OpenView<MessageBoxView>(viewName);
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<IUIManager>().GetView<MessageBoxView>(viewName);
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, titleStr, confirmStr, cancelStr);
    }
    public void OpenMessageBox(string msgContent, Action<bool> OnBtnClick, string titleStr = "提示", string confirmStr = "确定", string cancelStr = "取消")
    {
        MainManager.Instance.GetManager<IUIManager>().OpenView<MessageBoxView>();
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<IUIManager>().GetView<MessageBoxView>();
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, titleStr, confirmStr, cancelStr);
    }
    void SetMessageBoxView(MessageBoxView messageBoxView, Action<bool> OnBtnClick, string msgContent, string titleStr = "提示", string confirmStr = "确定", string cancelStr = "取消") {
        messageBoxView.OnBtnClick = OnBtnClick;
        messageBoxView.MsgStr = msgContent;
        messageBoxView.ConfirmStr = confirmStr;
        messageBoxView.TitleStr = titleStr;
        messageBoxView.CancelStr = cancelStr;
    }
}
