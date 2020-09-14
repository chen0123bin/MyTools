using UnityEngine;
using LWFramework.Asset;
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class AssetsInit : MonoBehaviour
{
    AssetsManager Assets;
    public string assetPath;
    private AssetRequest asset;
    private GameObject go;
    // Start is called before the first frame update
    async void Start()
    {
        Assets = new AssetsManager();
        /// 初始化
       // Assets.Initialize(OnInitialized, (error) => { LWDebug.Log(error); });

        AssetsManagerRequest _Request = await Assets.InitializeAsync();
        if (_Request.isSuccess)
        {
            OnInitialized();
        }
        else
        {
            LWDebug.Log(_Request.error);
          //  onError(_Request.error);
        }
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Destroy(go);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            go = (GameObject)Instantiate(asset.asset);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject go = await Test22Async();
        }
        Assets.Update();
    }
    private async void OnInitialized()
    {
        if (assetPath.EndsWith(".prefab", StringComparison.CurrentCulture))
        {
            // GameObject  go = await Test22Async();    // await模式
            //StartCoroutine(LoadAsync());    //协程模式
            //回调模式
            asset = Assets.LoadAsync<UnityEngine.Object>(assetPath);
            asset.completed += delegate (AssetRequest a)
            {
                go = (GameObject)Instantiate(a.asset);
                go.name = a.asset.name;
                /// 设置关注对象，当关注对象销毁时，回收资源
                a.Require(go);
                //                Destroy(go, 3);
                /// 设置关注对象后，只需要释放一次，可以按自己的喜好调整，
                /// 例如 ABSystem 中，不需要 调用这个 Release，
                /// 这里如果之前没有调用 Require，下一帧这个资源就会被回收
                a.Release();
            };
        }
        else if(assetPath.EndsWith(".unity", StringComparison.CurrentCulture))
        {
            StartCoroutine(LoadSceneAsync());
        }
    }
    async UniTask<GameObject> Test22Async() {
        AssetRequest a = await LoadTest();
        go = (GameObject)Instantiate(a.asset);
        go.name = a.asset.name;
        return go;

    }
    public async UniTask<AssetRequest> LoadTest()
    {
        asset = Assets.LoadAsync<UnityEngine.Object>(assetPath);
        await UniTask.WaitUntil(() =>
        {

            return asset.isDone;
        });
        return asset;
    }
    IEnumerator LoadAsync()
    {
        asset = Assets.LoadAsync<UnityEngine.Object>(assetPath);
        while (!asset.isDone)
        {
            LWDebug.Log(asset.progress);
            yield return null;
        }
        go = (GameObject)Instantiate(asset.asset);
        go.name = asset.asset.name;
    }

    IEnumerator LoadSceneAsync()
    {
        var sceneAsset = Assets.LoadScene(assetPath, true, true);
        while(!sceneAsset.isDone)
        {
            LWDebug.Log(sceneAsset.progress);
            yield return null;
        }
        
        yield return new WaitForSeconds(3);
        Assets.Unload(sceneAsset);
    }
}
