using libx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.Core;

public class ABAssetManger : IAssetManager,IManager
{
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

   
}
