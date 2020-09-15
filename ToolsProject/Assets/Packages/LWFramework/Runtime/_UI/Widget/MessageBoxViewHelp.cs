using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxViewHelp:Singleton<MessageBoxViewHelp>
{
    public void OpenMessageBox(string viewName,GameObject uiGameObject, string msgContent, Action<bool> OnBtnClick, string confirmStr = "确定",string cancelStr = "取消") {
        MainManager.Instance.GetManager<UIManager>().OpenView<MessageBoxView>(viewName, uiGameObject);
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<UIManager>().GetView<MessageBoxView>(viewName);
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, confirmStr, cancelStr);
    }
    public void OpenMessageBox(string viewName, string msgContent, Action<bool> OnBtnClick, string confirmStr = "确定", string cancelStr = "取消")
    {
        MainManager.Instance.GetManager<UIManager>().OpenView<MessageBoxView>(viewName);
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<UIManager>().GetView<MessageBoxView>(viewName);
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, confirmStr, cancelStr);
    }
    public void OpenMessageBox(string msgContent, Action<bool> OnBtnClick,  string confirmStr = "确定", string cancelStr = "取消")
    {
        MainManager.Instance.GetManager<UIManager>().OpenView<MessageBoxView>();
        MessageBoxView messageBoxView = MainManager.Instance.GetManager<UIManager>().GetView<MessageBoxView>();
        SetMessageBoxView(messageBoxView, OnBtnClick, msgContent, confirmStr, cancelStr);
    }
    void SetMessageBoxView(MessageBoxView messageBoxView, Action<bool> OnBtnClick, string msgContent, string confirmStr = "确定", string cancelStr = "取消") {
        messageBoxView.OnBtnClick = OnBtnClick;
        messageBoxView.MsgStr = msgContent;
        messageBoxView.ConfirmStr = confirmStr;
        messageBoxView.CancelStr = cancelStr;
    }
}
