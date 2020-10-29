using libx;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABInitUpdate 
{
    private Action<bool> _onUpdateCallback;
    public Action<bool> OnUpdateCallback { get => _onUpdateCallback; set => _onUpdateCallback = value; }
    private GameObject m_MsgGo;
    /// <summary>
    /// 设置参数
    /// </summary>
    public void SetConfig() {
        LWGlobalConfig globalConfig = LWUtility.GlobalConfig;
        Assets.development = globalConfig.development;
        Assets.loggable = globalConfig.loggable;
        Assets.updateAll = globalConfig.updateAll;
        Assets.downloadURL = globalConfig.downloadURL;
        Assets.verifyBy = (VerifyBy)globalConfig.verifyBy;
        Assets.searchPaths = globalConfig.searchPaths;
        Assets.patches4Init = globalConfig.patches4Init;
        
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
                m_MsgGo = GameObject.Instantiate(Resources.Load<GameObject>("UI/MessageBoxView"));
                 StartUpdate();
                //MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", go, "是否更新资源?", (flag) =>
                //{
                //    if (flag)
                //    {
                //        StartUpdate();
                //    }
                //    else
                //    {
                //        Quit();
                //    }
                //});
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
                    UpdateAsset(Assets.patches4Init, "提示22222", OnComplete);                    
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
                    handler.Start();
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
        }
    }
    private void OnProgress(float progress)
    {
        Debug.Log("更新进度：" + progress);
    }

    private void OnMessage(string msg)
    {
        Debug.Log(msg);
    }

    private void OnComplete()
    {
        OnProgress(1);
        Debug.Log("本地资源版本：" + Assets.localVersions.ver);
        OnMessage("更新完成");
        OnUpdateCallback?.Invoke(true);
    }
    private void Quit()
    {
        OnUpdateCallback?.Invoke(false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
