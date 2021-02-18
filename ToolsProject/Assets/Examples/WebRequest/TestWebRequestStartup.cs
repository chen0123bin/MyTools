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
    public RawImage rawImage2;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        MainManager.Instance.AddManager(typeof(IWebRequestManager).ToString(), new WebRequestManager());

        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/ToolsProject/Bundles/abc.txt", RespABC, "");

        //SceneData sceneData = new SceneData() { sceneId = "111111"};

        //MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface("QueryScene", "http://192.168.100.125:8288/psych/app/querySceneById", RespQueryScene);


        //WWWForm form = new WWWForm();
        //form.AddField("jsonParam", LitJson.JsonMapper.ToJson(sceneData));
        //MainManager.Instance.GetManager<IWebRequestManager>().SendRequest("QueryScene", form);


        
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.124:8288/psych/scenePicture/picture/1605164119730.png", RespABC2);
       MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.112:8089/ToolsProject/Bundles/tt.png", RespABC3);
        WWWForm form = new WWWForm();
        CourseData courseData = new CourseData { courseId = "20" };
        form.AddField("jsonParam", LitJson.JsonMapper.ToJson(courseData));
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequestUrl("http://192.168.100.124:8288/psych/app/queryCourseById", RespABC, form);
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
    private void RespABC3(Texture2D obj)
    {
        rawImage2.texture = obj;
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
    public class CourseData
    {
        public string courseId;
    }
}
