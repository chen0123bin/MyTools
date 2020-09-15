using LWFramework;
using LWFramework.Asset;
using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }
    public static Action OnLateUpdate { get; set; }
    void Start()
    {
        DontDestroyOnLoad(gameObject);      
        MainManager.Instance.Init();
        // MainManager.Instance.AddManager(typeof(UpdateManager).ToString(), new UpdateManager());
        //  MainManager.Instance.AddManager(typeof(AssetsManager).ToString(), new AssetsManager());
        MainManager.Instance.AddManager(typeof(UIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(IAssetManager).ToString(), new ABAssetManger());       
        MainManager.Instance.AddManager(typeof(FSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(HotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());

       

        //MainManager.Instance.GetManager<UpdateManager>().onCompleted = OnCompleted;
        //MainManager.Instance.GetManager<UpdateManager>().onProgress = OnProgress;
        //MainManager.Instance.GetManager<UpdateManager>().onError = OnError;
        //MainManager.Instance.GetManager<UpdateManager>().Check();
    }
 
    private void OnError(string obj)
    {
        LWDebug.Log(obj);
      //  MainManager.Instance.StartProcedure();
        StartCoroutine(MainManager.Instance.GetManager<HotfixManager>().IE_LoadScript(LWUtility.GlobalConfig.hotfixCodeRunMode));
    }

    private void OnProgress(string arg1, float arg2)
    {
        //Debug.Log(arg1 + "   " + arg2);
    }

    private void OnCompleted()
    {
        LWDebug.Log("下载完成");
        //启动热更代码
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
        //MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher("OnApplicationQuit");
    }



}
