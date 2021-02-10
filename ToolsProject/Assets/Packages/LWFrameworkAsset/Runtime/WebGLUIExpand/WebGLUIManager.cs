using Cysharp.Threading.Tasks;
using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///WebGL的UI管理器
/// </summary>
public class WebGLUIManager : UIManager
{
    public override void Init() {
        UIUtility.Instance.CustomUILoad = new WebGLUILoad();
        base.Init();
    }
    public override async UniTask<T> OpenViewAsync<T>(bool isLastSibling = false)
    {
        IUIView uiViewBase;
        if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
        {
            uiViewBase = await CreateView<T>();
            m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
        }
        await UniTask.WaitUntil(() => uiViewBase != null);
        if (!uiViewBase.IsOpen)
            uiViewBase.OpenView();
        uiViewBase.SetViewLastSibling(isLastSibling);
        return (T)uiViewBase;
    }
   
    /// <summary>
    /// 创建一个VIEW
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async UniTask<BaseUIView> CreateView<T>(GameObject uiGameObject = null, string loadPathParam = null)
    {
        BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;

        //获取UIViewDataAttribute特性
        var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
        if (attr != null)
        {
            if (uiGameObject == null)
            {
                string loadPath = attr.m_LoadPath;
                //创建UI对象
                if (loadPathParam != null)
                {
                    loadPath = loadPathParam;
                }
                uiGameObject = await UIUtility.Instance.CreateViewEntityAsync(attr.m_LoadPath);
            }

            Transform parent = UIUtility.Instance.GetParent(attr.m_FindType, attr.m_Param);
            if (uiGameObject == null)
            {
                LWDebug.LogError("没有找到这个UI对象" + attr.m_LoadPath);
            }
            if (parent == null)
            {
                LWDebug.LogError("没有找到这个UI父节点" + attr.m_Param);
            }
            if (parent != null)
            {
                uiGameObject.transform.SetParent(parent, false);
            }
            //初始化UI
            uiView.CreateView(uiGameObject);
        }
        else
        {
            LWDebug.Log("没有找到UIViewDataAttribute这个特性");
        }
        LWDebug.Log("UIManager：" + typeof(T).ToString());
        return uiView;
    }
}


