using LWFramework.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZWMessageType {
    /// <summary>
    ///  抓取工具状态改变
    /// </summary>
	public static string GrabToolStateChange { get; set; }
    /// <summary>
    /// 植物动画播放完成
    /// </summary>
    public static string PlantAnimationEnd { get; set; }

    /// <summary>
    /// 提示框
    /// </summary>
    public static string ShowTips { get; set; }


    public static void ShowTipsByMessage(string str, float showTime) {
        Message message = MessagePool.GetMessage(ShowTips);
        message["str"] = str;
        message["showTime"] = showTime;
        //ContentMessageManager.GetInstance().MessageManager.Dispatcher(message);
    }
}
