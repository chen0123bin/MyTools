using Cysharp.Threading.Tasks;
using libx;
using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLUILoad : IUILoad
{
 

    public Sprite GetSprite(string path)
    {
        var asset = MainManager.Instance.GetManager<IAssetsManager>().Load<UnityEngine.Sprite>(path);
        return (Sprite)asset;
    }

    public GameObject LoadUIGameObject(string path)
    {
        //AssetRequest assetRequest = MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<AssetRequest>(path, typeof(GameObject));
        //UniTask.WaitUntil(() => assetRequest.isDone);
        throw new System.NotImplementedException();
    }


    public async UniTask<GameObject> LoadUIGameObjectAsync(string path)
    {

        //AssetRequest assetRequest = MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<AssetRequest>(path, typeof(GameObject));
        //await UniTask.WaitUntil(() => assetRequest.isDone);       
        //GameObject uiGameObject = (GameObject)GameObject.Instantiate(assetRequest.asset);
        GameObject go = await MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<GameObject>(path);
        GameObject uiGameObject = (GameObject)GameObject.Instantiate(go);
        return uiGameObject;
    }
    //public async UniTask<GameObject> LoadUIGameObjectAsync(string path)
    //{

    //    AssetRequest assetRequest = MainManager.Instance.GetManager<IAssetsManager>().LoadAsync<AssetRequest>(path, typeof(GameObject));
    //    await UniTask.WaitUntil(() => assetRequest.isDone);
    //    var isWebURL = path.StartsWith("http://", StringComparison.Ordinal) ||
    //                      path.StartsWith("https://", StringComparison.Ordinal) ||
    //                      path.StartsWith("file://", StringComparison.Ordinal) ||
    //                      path.StartsWith("ftp://", StringComparison.Ordinal) ||
    //                      path.StartsWith("jar:file://", StringComparison.Ordinal);

    //    GameObject uiGameObject = null;
    //    if (isWebURL)
    //    {
    //        byte[] results = assetRequest.bytes;
    //        AssetBundle ab = AssetBundle.LoadFromMemory(results);
    //        uiGameObject = (GameObject)GameObject.Instantiate(ab.LoadAsset<GameObject>("HomeView"));
    //    }
    //    else {
    //        uiGameObject = (GameObject)GameObject.Instantiate(assetRequest.asset);
    //    }

    //    return uiGameObject;
    //}
}
