using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace LWFramework.WebRequest {
    /// <summary>
    /// Web请求管理器
    /// </summary>
    public class WebRequestManager : IManager, IWebRequestManager
    {
        /// <summary>
        /// 所有网络接口
        /// </summary>
        private Dictionary<string, BaseWebInterface> WebInterfaces { get; set; } = new Dictionary<string, BaseWebInterface>();
        private bool m_IsOffline = false;
        /// <summary>
        /// 当前是否是离线状态
        /// </summary>
        public bool IsOffline
        {
            get => m_IsOffline; set => m_IsOffline = value;
        }

        private ObjectPool<WebInterfaceGetString> m_StrPool;
        private ObjectPool<WebInterfaceGetTexture2D> m_TexPool;
        private ObjectPool<WebInterfaceGetAudioClip> m_AudioPool;

        public void Init()
        {
            m_StrPool = new ObjectPool<WebInterfaceGetString>(5);
            m_TexPool = new ObjectPool<WebInterfaceGetTexture2D>(5);
            m_AudioPool = new ObjectPool<WebInterfaceGetAudioClip>(5);
        }

        public void Update()
        {
        }
        #region 注册接口
        /// <summary>
        /// 注册接口（获取 string）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        /// <param name="handler">获取 string 之后的处理者</param>
        /// <param name="offlineHandle">离线模式处理者</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl, Action<string> handler, Action offlineHandle = null)
        {
            if (!WebInterfaces.ContainsKey(interfaceName))
            {
                WebInterfaceGetString wi = m_StrPool.Spawn(); // new WebInterfaceGetString();
                wi.Name = interfaceName;
                wi.Url = interfaceUrl;
                wi.OfflineHandler = offlineHandle;
                wi.Handler = handler;
                WebInterfaces.Add(interfaceName, wi);
            }
            else
            {
                LWDebug.LogError("添加接口失败：已存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        /// <summary>
        /// 注册接口（获取 Texture2D）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        /// <param name="handler">获取 Texture2D 之后的处理者</param>
        /// <param name="offlineHandle">离线模式处理者</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl, Action<Texture2D> handler, Action offlineHandle = null)
        {
            if (!WebInterfaces.ContainsKey(interfaceName))
            {
                WebInterfaceGetTexture2D wi = m_TexPool.Spawn();//new WebInterfaceGetTexture2D();
                wi.Name = interfaceName;
                wi.Url = interfaceUrl;
                wi.OfflineHandler = offlineHandle;
                wi.Handler = handler;
                WebInterfaces.Add(interfaceName, wi);
            }
            else
            {
                LWDebug.LogError("添加接口失败：已存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        /// <summary>
        /// 注册接口（获取 AudioClip）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        /// <param name="handler">获取 AudioClip 之后的处理者</param>
        /// <param name="offlineHandle">离线模式处理者</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl, Action<AudioClip> handler, Action offlineHandle = null)
        {
            if (!WebInterfaces.ContainsKey(interfaceName))
            {
                WebInterfaceGetAudioClip wi = m_AudioPool.Spawn(); ;
                wi.Name = interfaceName;
                wi.Url = interfaceUrl;
                wi.OfflineHandler = offlineHandle;
                wi.Handler = handler;
                WebInterfaces.Add(interfaceName, wi);
            }
            else
            {
                LWDebug.LogError("添加接口失败：已存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        /// <summary>
        /// 注册接口（提交 表单）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl)
        {
            if (!WebInterfaces.ContainsKey(interfaceName))
            {
                WebInterfacePost wi = new WebInterfacePost();
                wi.Name = interfaceName;
                wi.Url = interfaceUrl;
                WebInterfaces.Add(interfaceName, wi);
            }
            else
            {
                LWDebug.LogError("添加接口失败：已存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        #endregion
        /// <summary>
        /// 通过名称获取接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>网络接口</returns>
        public BaseWebInterface GetInterface(string interfaceName)
        {
            if (WebInterfaces.ContainsKey(interfaceName))
            {
                return WebInterfaces[interfaceName];
            }
            else
            {
                LWDebug.LogError("获取接口失败：不存在名为 " + interfaceName + " 的网络接口！");
                return null;
            }
        }
        /// <summary>
        /// 是否存在指定名称的接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>是否存在</returns>
        public bool IsExistInterface(string interfaceName)
        {
            return WebInterfaces.ContainsKey(interfaceName);
        }
        /// <summary>
        /// 取消注册接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        public void UnRegisterInterface(string interfaceName)
        {
            if (WebInterfaces.ContainsKey(interfaceName))
            {
                BaseWebInterface baseWebInterface = WebInterfaces[interfaceName];
                WebInterfaces.Remove(interfaceName);

                if (baseWebInterface is WebInterfaceGetString)
                {
                    m_StrPool.Unspawn(baseWebInterface as WebInterfaceGetString);
                }
                else if (baseWebInterface is WebInterfaceGetTexture2D)
                {
                    m_TexPool.Unspawn(baseWebInterface as WebInterfaceGetTexture2D);
                }
                else if (baseWebInterface is WebInterfaceGetAudioClip)
                {
                    m_AudioPool.Unspawn(baseWebInterface as WebInterfaceGetAudioClip);
                }
                else
                {
                    baseWebInterface = null;
                }
            }
            else
            {
                LWDebug.LogError("移除接口失败：不存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        /// <summary>
        /// 清空所有接口
        /// </summary>
        public void ClearInterface()
        {
            WebInterfaces.Clear();
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="parameter">可选参数（要同时传入参数名和参数值，例：name='张三'）</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, params string[] parameter)
        {
            if (WebInterfaces.ContainsKey(interfaceName))
            {
                if (IsOffline || WebInterfaces[interfaceName].IsOffline)
                {
                    WebInterfaces[interfaceName].OfflineHandler?.Invoke();
                }
                else
                {
                    _ = SendRequestGet(interfaceName, parameter);
                }
            }
            else
            {
                LWDebug.LogError("发起网络请求失败：不存在名为 " + interfaceName + " 的网络接口！");
            }
        }
        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="form">参数表单</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, WWWForm form)
        {
            if (WebInterfaces.ContainsKey(interfaceName))
            {
                if (IsOffline || WebInterfaces[interfaceName].IsOffline)
                {
                    WebInterfaces[interfaceName].OfflineHandler?.Invoke();
                }
                else
                {
                    _ = SendRequestPostForm(interfaceName, form);
                }
            }
            else
            {
                LWDebug.LogError("发起网络请求失败：不存在名为 " + interfaceName + " 的网络接口！");
            }
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="parameter">Json格式的字符串</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, string parameter)
        {
            if (WebInterfaces.ContainsKey(interfaceName))
            {
                if (IsOffline || WebInterfaces[interfaceName].IsOffline)
                {
                    WebInterfaces[interfaceName].OfflineHandler?.Invoke();
                }
                else
                {
                    SendRequestPostJson(interfaceName, parameter);
                }
            }
            else
            {
                LWDebug.LogError("发起网络请求失败：不存在名为 " + interfaceName + " 的网络接口！");
            }
        }
       /// <summary>
       /// 发起网络数据请求，当前方法不需要注册
       /// </summary>
       /// <param name="interfaceUrl">请求地址</param>
       /// <param name="handler">回调函数</param>
       /// <param name="parameter"></param>
        public void SendRequestUrl(string interfaceUrl, Action<string> handler, string parameter)
        {
            WebInterfaceGetString wi = m_StrPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            _ = SendRequestPostJson(wi, parameter);
            m_StrPool.Unspawn(wi);
        }

        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        public void SendRequestUrl(string interfaceUrl, Action<Texture2D> handler)
        {
            WebInterfaceGetTexture2D wi = m_TexPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            _ = SendRequestPostJson(wi,"");
            m_TexPool.Unspawn(wi);
        }
        /// <summary>
        /// 发起网络数据请求，当前方法不需要注册
        /// </summary>
        /// <param name="interfaceUrl">请求地址</param>
        /// <param name="handler">回调函数</param>
        public void SendRequestUrl(string interfaceUrl, Action<AudioClip> handler)
        {
            WebInterfaceGetAudioClip wi = m_AudioPool.Spawn();
            wi.Url = interfaceUrl;
            wi.Handler = handler;
            _ = SendRequestPostJson(wi, "");
            m_AudioPool.Unspawn(wi);
        }
        private async UniTaskVoid SendRequestGet(string interfaceName, params string[] parameter)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(WebInterfaces[interfaceName].Url);
            if (parameter.Length > 0)
            {
                builder.Append("?");
                builder.Append(parameter[0]);
            }
            for (int i = 1; i < parameter.Length; i++)
            {
                builder.Append("&");
                builder.Append(parameter[i]);
            }
            string url = builder.ToString();

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                DateTime begin = DateTime.Now;

                WebInterfaces[interfaceName].OnSetDownloadHandler(request);
                await request.SendWebRequest();

                DateTime end = DateTime.Now;

                if (!request.isNetworkError && !request.isHttpError)
                {
                    LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                        , begin.ToString("mm:ss:fff"), interfaceName, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, WebInterfaces[interfaceName].OnGetDownloadString(request.downloadHandler)));

                    WebInterfaces[interfaceName].OnRequestFinished(request.downloadHandler);
                }
                else
                {
                    LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), interfaceName, url, end.ToString("mm:ss:fff"), request.error));

                    WebInterfaces[interfaceName].OnRequestFinished(null);
                }
            }
        }

        private async UniTaskVoid SendRequestPostForm(string interfaceName, WWWForm form)
        {
            string url = WebInterfaces[interfaceName].Url;

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                DateTime begin = DateTime.Now;

                WebInterfaces[interfaceName].OnSetDownloadHandler(request);
                await request.SendWebRequest();

                DateTime end = DateTime.Now;

                if (!request.isNetworkError && !request.isHttpError)
                {
                    LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                        , begin.ToString("mm:ss:fff"), interfaceName, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, WebInterfaces[interfaceName].OnGetDownloadString(request.downloadHandler)));

                    WebInterfaces[interfaceName].OnRequestFinished(request.downloadHandler);
                }
                else
                {
                    LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), interfaceName, url, end.ToString("mm:ss:fff"), request.error));

                    WebInterfaces[interfaceName].OnRequestFinished(null);
                }
            }

        }
        

        private void SendRequestPostJson(string interfaceName, string jsonData)
        {

            _ = SendRequestPostJson(WebInterfaces[interfaceName], jsonData);
        }
        private async UniTaskVoid SendRequestPostJson(BaseWebInterface wi, string jsonData)
        {
            string url = wi.Url;

            using (UnityWebRequest request = UnityWebRequest.Post(url, jsonData))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                DateTime begin = DateTime.Now;

                wi.OnSetDownloadHandler(request);
                await request.SendWebRequest();

                DateTime end = DateTime.Now;

                if (!request.isNetworkError && !request.isHttpError)
                {
                    LWDebug.Log(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 收到回复：{4}字节  string:{5}"
                        , begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.downloadHandler.data.Length, wi.OnGetDownloadString(request.downloadHandler)));

                    wi.OnRequestFinished(request.downloadHandler);
                }
                else
                {
                    LWDebug.LogError(string.Format("[{0}] 发起网络请求：[{1}] {2}\r\n[{3}] 网络请求出错：{4}", begin.ToString("mm:ss:fff"), wi.Name, url, end.ToString("mm:ss:fff"), request.error));

                    wi.OnRequestFinished(null);
                }
            }
        }
    }
}
