using LitJson;
using LWFramework.Core;
using LWFramework.WebRequest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMsg  :Singleton<NetMsg>
{
    bool m_IsInsideNet;
    private string m_HttpAddress = "http://192.168.100.124:8288/psych/app/";
    /// <summary>
    /// 服务器地址前缀
    /// </summary>
    public string HttpAddress
    {
        get => m_HttpAddress;set => m_HttpAddress = value;
    }
    public void Request(string interfaceName,string param,string url,Action<BaseJsonData> resoponeAction) {
        MainManager.Instance.GetManager<IWebRequestManager>().RegisterInterface(interfaceName, url,
           (string respStr) =>
           {
               Debug.Log(respStr);
               MainManager.Instance.GetManager<IWebRequestManager>().UnRegisterInterface(interfaceName);
              // BaseJsonData baseJsonData =  JsonMapper.ToObject<BaseJsonData>(respStr);
              // resoponeAction.Invoke(baseJsonData);
           });
        MainManager.Instance.GetManager<IWebRequestManager>().SendRequest(interfaceName, param);
    }

   

    public class BaseJsonData { 
    
    }
}
