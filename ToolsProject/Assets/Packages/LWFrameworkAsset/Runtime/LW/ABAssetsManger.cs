using libx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;
using Cysharp.Threading.Tasks;

public class ABAssetsManger : IAssetsManager,IManager
{
    private ABInitUpdate _abInitUpdate;
    public Action<bool> OnInitUpdateComplete { set => _abInitUpdate.OnInitUpdateComplete = value; }
    /// <summary>
    /// 自定义初始化更新器
    /// </summary>
    public ABInitUpdate ABInitUpdate { set => _abInitUpdate = value; get => _abInitUpdate; }
    public void Init()
    {
        if (_abInitUpdate == null) {
            _abInitUpdate = new ABInitUpdate();
        }       
        _abInitUpdate.SetConfig();
        _abInitUpdate.AssetsInitialize();
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
        AssetRequest assetRequest = Assets.LoadAssetAsync(path, type);
        object ret = (object)assetRequest;
        return (T)ret;
    }
    public async UniTask<T> LoadAsync<T>(string path)
    {
        AssetRequest request = Assets.LoadAssetAsync(path, typeof(T)); 
        await UniTask.WaitUntil(() => request.isDone);
        
        return (T)(object)request.asset;
    }
    public UniTask<T> LoadRequestAsync<T>(string path)
    {
        throw new NotImplementedException();
    }

    public void Unload<T>(T param) where T : UnityEngine.Object
    {
        LWDebug.LogWarning("AB模式下没用Unload函数");
        
    }
    public void LoadSceneAsync(string scenePath,bool additive, Action loadComplete = null)
    {
        SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive);
        sceneAssetRequest.completed = (asset) =>
        {
            loadComplete?.Invoke();
        };
       
    }
    public T GetRequest<T,K>(string path)
    {
        AssetRequest request = Assets.LoadAssetAsync(path, typeof(K));
        return (T)(object)request;
    }
    public T GetSceneRequest<T>(string scenePath, bool additive)
    {
        SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive);
        return (T)(object)sceneAssetRequest;
    }
    public void UpdatePatchAndLoadSceneAsync(string scenePath, bool additive, Action loadComplete = null)
    {
        string patchName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, (scenePath.LastIndexOf(".") - scenePath.LastIndexOf("/") - 1));
        _abInitUpdate.UpdateAsset(new[] { patchName }, "更新提示", () =>
        {
            SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(scenePath, additive);
            sceneAssetRequest.completed = (asset) =>
            {
                loadComplete?.Invoke();
            };          
        });
    }
    public void UpdatePatchAsset(string patchName)
    {
        _abInitUpdate.UpdateAsset(new[] { patchName }, "更新提示", () =>
        {
            LWDebug.Log("更新完成");
        });
    }

  
}
