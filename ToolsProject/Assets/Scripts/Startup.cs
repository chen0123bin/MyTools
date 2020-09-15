using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }
    void Start()
    {
        DontDestroyOnLoad(gameObject);      
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(UIManager).ToString(), new UIManager());    
        MainManager.Instance.AddManager(typeof(FSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(HotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        if (LWUtility.GlobalConfig.assetMode == AssetMode.AssetBundle)
        {
            MainManager.Instance.AddManager(typeof(IAssetManager).ToString(), new ABAssetManger());
        }
        else if (LWUtility.GlobalConfig.assetMode == AssetMode.Resources)
        {
            MainManager.Instance.AddManager(typeof(IAssetManager).ToString(), new ResAssetManger());
        }
        MainManager.Instance.GetManager<IAssetManager>().OnUpdateCallback = OnUpdateCallback;
    }

    private void OnUpdateCallback(bool obj)
    {
        StartCoroutine(MainManager.Instance.GetManager<HotfixManager>().IE_LoadScript(LWUtility.GlobalConfig.hotfixCodeRunMode));
    } 
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }
   
    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }



}
