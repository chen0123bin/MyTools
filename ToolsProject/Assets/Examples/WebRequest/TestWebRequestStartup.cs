using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using LWFramework.WebRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestWebRequestStartup : MonoBehaviour
{
    public RawImage rawImage;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        MainManager.Instance.AddManager(typeof(IWebRequestManager).ToString(), new WebRequestManager());

        MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("ABC", "http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt",RespABC);
        MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("ABC2", "http://192.168.100.112:8089/ToolsProject/Bundles/tt.png", RespABC2);
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequest("ABC");
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequest("ABC2");
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt", RespABC,"");
        //NetMsg.Instance.Request("","", "http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt", AAC);

        //SceneData sceneData = new SceneData() { sceneId = "111111"};

        //MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("QueryScene", "http://192.168.100.125:8288/psych/app/querySceneById", RespQueryScene);


        //WWWForm form = new WWWForm();
        //form.AddField("jsonParam", LitJson.JsonMapper.ToJson(sceneData));
        //MainManager.Instance.GetManager<IWebRequestManager>().SendRequest("QueryScene", form);
    }

    private void RespQueryScene(string obj)
    {

    }

    private void AAC(NetMsg.BaseJsonData obj)
    {
        JsonTestData jsonTestData = obj as JsonTestData;
    }

    private void RespABC(string obj)
    {
        LWDebug.Log(obj);
        JsonTestData jtd = LitJson.JsonMapper.ToObject<JsonTestData>(obj);
        LWDebug.Log(jtd.name);
    }
    private void RespABC2(Texture2D obj)
    {
        rawImage.texture = obj;
    }

    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();        
    }

    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }
    public class JsonTestData : NetMsg.BaseJsonData
    {
        public string name;
        public string url;
        public int page;
    }
    public class SceneData {
        public string sceneId;
    }
}
