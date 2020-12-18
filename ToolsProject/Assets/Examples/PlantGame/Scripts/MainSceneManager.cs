using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using LitJson;
public enum Weather {
    Sun,Rain
}
public class MainSceneManager : MonoBehaviour {
    public string type;
    //玩家的ID
    private int NetID;
    public GameObject clientScene;
    public GameObject serverScene;
    public GameObject inputLeap;

    public AudioClip btnAudio;
    void Awake()
    {
        InitMessage();
        //JsonDataTool.TestTool();
        //获取配置文件数据
        string str = ReadFile.GetRoot("config.xml");
        XElement xElement = XElement.Parse(str);
        type = xElement.Element("Config").Attribute("Type").Value;
        string ip = xElement.Element("Config").Attribute("IP").Value;
        int port = int.Parse( xElement.Element("Config").Attribute("Port").Value);
        NetID = int.Parse(xElement.Element("Config").Attribute("Name").Value);
        //客户端延迟时间
        float waitTime = float.Parse(xElement.Element("Config").Attribute("WaitTime").Value);
        Debug.Log(waitTime);
       
        
    }
  
   
    void InitMessage (){
        ZWMessageType.GrabToolStateChange = "GrabToolStateChange";
        ZWMessageType.PlantAnimationEnd = "PlantAnimationEnd";
        ZWMessageType.ShowTips = "ShowTips";
    }
    public void StartGameOnClick() {
        
        Debug.Log("开始游戏");
      
       
    }
    //public void ExitGameOnClick() {
    //    Debug.Log("退出游戏");
    //    clientManager.ExitGame();
    //}
    //public void AgainGameOnClick() {
    //    Debug.Log("重新开始");
    //    clientManager.AgainGame();
    //}
    //public void OpenPakgeOnClick() {
    //    Debug.Log("打开包裹");
    //    clientManager.SetPackge();
    //}
    //public void PlantOnEnter(int plantId) {
    //    clientManager.HandEnterPlant(plantId);
    //}
    //public void PlantOnExit(int plantId)
    //{
    //    clientManager.HandExitPlant();
    //}
    //public void ChoosePlantOnClick(int plantId)
    //{
    //    Debug.Log("选择植物" + plantId);
    //    clientManager.ChoosePlant(plantId);
    //}
    public void SetWeather(int type) {
        Debug.Log(NetID + "我这边的天气状态" + type);
    }

    public void PlayBtnAudio() {
        GetComponent<AudioSource>().PlayOneShot(btnAudio);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Load");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Screen.SetResolution(1920, 1080, false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Screen.SetResolution(1920, 1080, true);
        }
      
    }
    private void OnDestroy()
    {
    }
}
