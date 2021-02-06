using LWFramework.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AssetMode { 
    Resources = 0,
    AssetBundle = 1,
    AssetBundleDev = 2
}
[CreateAssetMenu(fileName = "LWGlobalAsset", menuName = "LWFramework/LWGlobalAsset", order = 0)]
public class LWGlobalAsset : ScriptableObject
{
    //是否连接服务器
    //public bool connServer;
    //是否开启ab模式
    // public bool assetBundleMode;
    [OnValueChanged("ChangeAssetMode")]
    [LabelText("资源加载方式")]
    public AssetMode assetMode;
    //热更模式
    [LabelText("代码执行方式")]
    public HotfixCodeRunMode hotfixCodeRunMode;
    public bool loggable;
    public int verifyBy = 1 ;
    [InfoBox("结构以（Bundles/）结尾")]
    public string downloadURL;
    [HideInInspector]
    public bool development;//该属性用于适配XASSET 设置时不使用
    public string[] searchPaths;
    public string[] updatePatches4Init;
    public bool updateAll;

    public void ChangeAssetMode() {
        switch (assetMode)
        {
            case AssetMode.Resources:
                development = false;
                hotfixCodeRunMode = HotfixCodeRunMode.ByCode;
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

    public LWGlobalConfig GetLWGlobalConfig() {
        LWGlobalConfig globalConfig = new LWGlobalConfig
        {
            assetMode = (int)this.assetMode,
            hotfixCodeRunMode = (int)this.hotfixCodeRunMode,
            loggable = this.loggable,
            verifyBy = this.verifyBy,
            downloadURL = this.downloadURL,
            searchPaths = this.searchPaths,
            updatePatches4Init = this.updatePatches4Init,
            updateAll = this.updateAll
        };
        return globalConfig;
    }

    [Button("创建外部配置数据")]
    public void CreateJson() {
        ConfigDataTool.Create("config", GetLWGlobalConfig());
    }
    [Button("测试查看DownloadURL")]
    public void TestJson()
    {
        LWDebug.Log(ConfigDataTool.ReadData<LWGlobalConfig>("config").downloadURL);
    }
}
public class LWGlobalConfig
{
    public int assetMode;
    public int hotfixCodeRunMode;
    public bool loggable;
    public int verifyBy = 1;
    public string downloadURL;
    public string[] searchPaths;
    public string[] updatePatches4Init;
    public bool updateAll;
}