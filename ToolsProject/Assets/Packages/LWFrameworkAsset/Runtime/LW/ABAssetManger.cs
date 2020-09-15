using libx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;

public class ABAssetManger : IAssetManager,IManager
{
    private Action<bool> _onUpdateCallback;
    public Action<bool> OnUpdateCallback { get => _onUpdateCallback; set => _onUpdateCallback =value; }

    public void Init()
    {
        LWGlobalConfig globalConfig = LWUtility.GlobalConfig;
        Assets.development = globalConfig.development;
        Assets.loggable = globalConfig.loggable;
        Assets.updateAll = globalConfig.updateAll;
        Assets.downloadURL = globalConfig.downloadURL;
        Assets.verifyBy = (VerifyBy)globalConfig.verifyBy;
        Assets.searchPaths = globalConfig.searchPaths;
        Assets.patches4Init = globalConfig.patches4Init;
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
                GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("MessageBoxView"));
                MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", go, "是否更新资源?", (flag) => {
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
        });
    }

    public void Update()
    {
    }
    public T Load<T>(string path)
    {
        AssetRequest assetRequest = Assets.LoadAsset(path, typeof(T));       
        return (T)(object)assetRequest.asset;
    }

    public T LoadAsync<T>(string path, Type type)
    {
        return (T)(object)Assets.LoadAssetAsync(path, type);
    }

    public void Unload<T>(T param) where T : UnityEngine.Object
    {
        Debug.LogWarning("AB模式下没用Unload函数");
    }

    public void StartUpdate()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", "请检查网络连接状态", retry =>
            {
                if (retry)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }
            }, "重试", "退出");
        }
        else
        {
            Assets.DownloadVersions(error =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", string.Format("获取服务器版本失败：{0}", error), retry =>
                    {
                        if (retry)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    }, "重试", "退出");
                }
                else
                {
                    Downloader handler;
                    // 按分包下载版本更新，返回true的时候表示需要下载，false的时候，表示不需要下载
                    if (Assets.DownloadAll(Assets.patches4Init, out handler))
                    {
                        var totalSize = handler.size;
                        var tips = string.Format("发现内容更新，总计需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
                        MessageBoxViewHelp.Instance.OpenMessageBox("ABMessageBoxView", tips, download =>
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
                                handler.onFinished += OnComplete;
                                handler.Start();
                            }
                            else
                            {
                                Quit();
                            }
                        }, "重试", "退出");

                    }
                    else
                    {
                        OnComplete();
                    }
                }
            });
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

    public void LoadScene(string scenePath)
    {

    }
}
