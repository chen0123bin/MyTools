using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantStartup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(IFSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(HotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        ABAssetsManger abAssetManger = new ABAssetsManger();
        abAssetManger.ABInitUpdate = new ABInitUpdate();
        abAssetManger.ABInitUpdate.m_AutoUpdate = false;
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), abAssetManger);
        MainManager.Instance.GetManager<IAssetsManager>().OnInitUpdateComplete = OnUpdateComplete;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateComplete(bool obj)
    {
        if (obj) {
            StartCoroutine(MainManager.Instance.GetManager<HotfixManager>().IE_LoadScript(LWUtility.GlobalConfig.hotfixCodeRunMode));
        }
        
    }
}
