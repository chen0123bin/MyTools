using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResAssetsManger : IAssetsManager,IManager
{
    public Action<bool> OnUpdateCallback {  set => throw new NotImplementedException(); }

    public void Init()
    {
    }
    public void Update()
    {
    }
    public T Load<T>(string path)
    {      
        return (T)(object)Resources.Load(path, typeof(T));
    }

    public T LoadAsync<T>(string path,Type type)
    {
        return  (T)(object)Resources.LoadAsync(path,type);
    }
    public void Unload<T>(T param) where T: UnityEngine.Object
    {
        
        Resources.UnloadAsset(param);
    }

    public void LoadScene(string scenePath,bool additive)
    {

    }
}
