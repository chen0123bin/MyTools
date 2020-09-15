using LWFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AssetMode { 
    Resources,
    AssetBundle,
    AssetBundleDev
}
[CreateAssetMenu(fileName = "LWGlobalConfig", menuName = "LWFramework/LWGlobalConfig", order = 0)]
public class LWGlobalConfig : ScriptableObject
{
    //是否连接服务器
    public bool connServer;
    //是否开启ab模式
    public bool assetBundleMode;
    //热更模式
    public HotfixCodeRunMode hotfixCodeRunMode;
    [OnValueChanged("ChangeAssetMode")]
    public AssetMode assetMode;
    public bool loggable;
    public int verifyBy = 1 ;
    public string downloadURL;
    public bool development;
    public bool dontDestroyOnLoad = true;
    public string[] searchPaths;
    public string[] patches4Init;
    public bool updateAll;

    public void ChangeAssetMode() {
        switch (assetMode)
        {
            case AssetMode.Resources:
                development = false;
                break;
            case AssetMode.AssetBundle:
                development = false;
                break;
            case AssetMode.AssetBundleDev:
                development = true;
                break;
            default:
                break;
        }
    }
}
