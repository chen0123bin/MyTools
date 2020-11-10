using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.WebRequest {
    public interface IWebRequestManager
    {
        bool IsOffline { get; set; }

        void ClearInterface();
        BaseWebInterface GetInterface(string interfaceName);
        bool IsExistInterface(string interfaceName);
        void RegisterInterface(string interfaceName, string interfaceUrl, System.Action<string> handler, System.Action offlineHandle = null);
        void RegisterInterface(string interfaceName, string interfaceUrl, System.Action<Texture2D> handler, System.Action offlineHandle = null);
        void RegisterInterface(string interfaceName, string interfaceUrl, System.Action<AudioClip> handler, System.Action offlineHandle = null);
        void RegisterInterface(string interfaceName, string interfaceUrl);
        void SendRequest(string interfaceName, params string[] parameter);
        void SendRequest(string interfaceName, WWWForm form);
        void SendRequest(string interfaceName, string parameter);
        void SendRequestUrl(string interfaceUrl, Action<string> handler, string parameter);
        void SendRequestUrl(string interfaceUrl, Action<Texture2D> handler);
        void SendRequestUrl(string interfaceUrl, Action<AudioClip> handler);
        void UnRegisterInterface(string interfaceName);
       
    }

}
