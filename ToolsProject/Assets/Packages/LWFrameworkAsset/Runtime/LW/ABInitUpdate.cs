using libx;
using LWFramework.Core;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABInitUpdate 
{
    private Action<bool> _onInitUpdateComplete;
    /// <summary>
    /// 初始化更新完成
    /// </summary>
    public Action<bool> OnInitUpdateComplete { get => _onInitUpdateComplete; set => _onInitUpdateComplete = value; }
    /// <summary>
    /// 是否自动更新
    /// </summary>
    public bool m_AutoUpdate { get; set; } = true;
    /// <summary>
    /// 更新提示框
    /// </summary>
    public GameObject m_MsgGo { get; set; }
    /// <summary>
    /// 更新进度条
    /// </summary>
    public GameObject m_ProgressBarGo { get; set; }
    private LoadAssetBarView m_LoadAssetBarView;
    /// <summary>
    /// 设置参数
    /// </summary>
    public void SetConfig() {
        LWGlobalConfig globalConfig = LWUtility.GlobalConfig;
        Assets.development = (globalConfig.assetMode == 2);
        Assets.loggable = globalConfig.loggable;
        Assets.updateAll = globalConfig.updateAll;
        Assets.downloadURL = globalConfig.downloadURL;
        Assets.verifyBy = (VerifyBy)globalConfig.verifyBy;
        Assets.searchPaths = globalConfig.searchPaths;
        Assets.patches4Init = globalConfig.updatePatches4Init;
        
    }
    public virtual void AssetsInitialize() {
        Assets.Initialize(error =>
        {
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError(error);
                return;
            }
            else
            {
                Debug.Log("Assets初始化成功");
                if (m_MsgGo == null) {
                    m_MsgGo = GameObject.Instantiate(Resources.Load<GameObject>("WidgetUI/MessageBoxView"));
                }
                if (m_ProgressBarGo == null)
                {
                    m_ProgressBarGo = GameObject.Instantiate(Resources.Load<GameObject>("WidgetUI/LoadAssetBarView"));
                }
                MainManager.Instance.GetManager<IUIManager>().OpenView<LoadAssetBarView>("LoadAssetBarView",m_ProgressBarGo);
                m_LoadAssetBarView = MainManager.Instance.GetManager<IUIManager>().GetView<LoadAssetBarView>();
                if (m_AutoUpdate)
                {
                    StartUpdate();
                }
                else {
                    MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", m_MsgGo, "是否更新资源?", (flag) =>
                    {
                        if (flag)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    });
                }
            }
        });
    }

    public void StartUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", m_MsgGo, "请检查网络连接状态", retry =>
            {
                if (retry)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }
            },"警告提示", "重试", "退出");
        }
        else
        {
            Assets.DownloadVersions(error =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", m_MsgGo, string.Format("获取服务器版本失败：{0}", error), retry =>
                    {
                        if (retry)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    }, "警告提示", "重试", "退出");
                }
                else
                {
                    UpdateAsset(Assets.patches4Init, "初始化更新", OnInitComplete);                    
                }
            });
        }
    }
    public void UpdateAsset(string []patchNameArray,string title,Action downloadCallback) {
        Downloader handler;
        // 按分包下载版本更新，返回true的时候表示需要下载，false的时候，表示不需要下载
        if (Assets.DownloadAll(patchNameArray, out handler))
        {
            var totalSize = handler.size;
            var tips = string.Format("需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
            MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", m_MsgGo, tips, download =>
            {
                if (download)
                {
                    handler.onUpdate += delegate (long progress, long size, float speed)
                    {
                        //刷新界面
                        OnMessage(string.Format("下载中...{0}/{1}, 速度：{2}",
                            Downloader.GetDisplaySize(progress),
                            Downloader.GetDisplaySize(size),
                            Downloader.GetDisplaySpeed(speed)));
                        OnProgress(progress * 1f / size);
                    };
                    handler.onFinished += downloadCallback;
                    handler.onFinished += OnUpdateComplete;
                    handler.Start();
                    m_LoadAssetBarView.OpenView();
                }
                else
                {
                    Quit();
                }
            }, title, "确认", "退出");

        }
        else
        {
            downloadCallback?.Invoke();
            OnUpdateComplete();
        }
    }
    private void OnProgress(float progress)
    {
        Debug.Log("更新进度：" + progress);
        m_LoadAssetBarView.SetLoadValue(progress);
    }

    private void OnMessage(string msg)
    {
        Debug.Log(msg);
        m_LoadAssetBarView.SetLoadMsg(msg);
    }

    private void OnInitComplete()
    {
        OnProgress(1);
        Debug.Log("本地资源版本：" + Assets.currentVersions.ver);
        OnMessage("更新完成");
        OnInitUpdateComplete?.Invoke(true);
    }
    private void OnUpdateComplete()
    {
        m_LoadAssetBarView.CloseView();
    }
    private void Quit()
    {
        OnInitUpdateComplete?.Invoke(false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
