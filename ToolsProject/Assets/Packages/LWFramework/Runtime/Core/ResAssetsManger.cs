using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResAssetsManger : IAssetsManager,IManager
{
    private Action<bool> _onUpdateCallback;
    public Action<bool> OnUpdateCallback {  set => _onUpdateCallback= value; }

    public void Init()
    {
        _ = TaskUpdateAsync();
    }
   
    async UniTaskVoid TaskUpdateAsync()
    {
        await UniTask.WaitForEndOfFrame();
        _onUpdateCallback?.Invoke(true);
    }
    public void Update()
    {
    }
    public T Load<T>(string path)
    {      
        return (T)(object)Resources.Load(ConverResPath(path), typeof(T));
    }

    public T LoadAsync<T>(string path,Type type)
    {
        return  (T)(object)Resources.LoadAsync(ConverResPath(path), type);
    }
    public void Unload<T>(T param) where T: UnityEngine.Object
    {
        
        Resources.UnloadAsset(param);
    }

    public void LoadScene(string scenePath,bool additive, Action loadComplete = null)
    {
        string sceneName = scenePath.Substring(scenePath.LastIndexOf("/") + 1, (scenePath.LastIndexOf(".") - scenePath.LastIndexOf("/") - 1));
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneName, additive?LoadSceneMode.Additive: LoadSceneMode.Single);
        asyncOperation.completed += (a) =>
        {
            loadComplete?.Invoke();
        };
    }
    private string ConverResPath(string path) {
        int startIndex = path.IndexOf("Resources") + "Resources".Length + 1;
        int length = path.LastIndexOf(".") - startIndex;
        string resPath = path.Substring(startIndex, length);
        return resPath;
    }
}
