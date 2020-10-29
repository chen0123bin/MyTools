using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.WebRequest {
    /// <summary>
    /// Web请求管理器
    /// </summary>
    public class WebRequestManager : IManager, IWebRequestManager
    {

        private bool m_IsOffline = false;
        /// <summary>
        /// 当前是否是离线状态
        /// </summary>
        public bool IsOffline
        {
            get => m_IsOffline; set => m_IsOffline = value;
        }
        private IWebRequestHelper _helper;

        public void Init()
        {
            _helper = new DefaultWebRequestHelper();
            _helper.InitHelper();
            _helper.Manager = this;
        }

        public void Update()
        {
        }

        /// <summary>
        /// 注册接口（获取 string）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        /// <param name="handler">获取 string 之后的处理者</param>
        /// <param name="offlineHandle">离线模式处理者</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl, Action<string> handler, Action offlineHandle = null)
        {
            _helper.RegisterInterface(interfaceName, interfaceUrl, handler, offlineHandle);
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
            _helper.RegisterInterface(interfaceName, interfaceUrl, handler, offlineHandle);
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
            _helper.RegisterInterface(interfaceName, interfaceUrl, handler, offlineHandle);
        }
        /// <summary>
        /// 注册接口（提交 表单）
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="interfaceUrl">接口url</param>
        public void RegisterInterface(string interfaceName, string interfaceUrl)
        {
            _helper.RegisterInterface(interfaceName, interfaceUrl);
        }
        /// <summary>
        /// 通过名称获取接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>网络接口</returns>
        public BaseWebInterface GetInterface(string interfaceName)
        {
            return _helper.GetInterface(interfaceName);
        }
        /// <summary>
        /// 是否存在指定名称的接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>是否存在</returns>
        public bool IsExistInterface(string interfaceName)
        {
            return _helper.IsExistInterface(interfaceName);
        }
        /// <summary>
        /// 取消注册接口
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        public void UnRegisterInterface(string interfaceName)
        {
            _helper.UnRegisterInterface(interfaceName);
        }
        /// <summary>
        /// 清空所有接口
        /// </summary>
        public void ClearInterface()
        {
            _helper.ClearInterface();
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="parameter">可选参数（要同时传入参数名和参数值，例：name='张三'）</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, params string[] parameter)
        {
            _helper.SendRequest(interfaceName, parameter);
        }
        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="form">参数表单</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, WWWForm form)
        {
            _helper.SendRequest(interfaceName, form);
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="parameter">Json格式的字符串</param>
        /// <returns>请求的协程</returns>
        public void SendRequest(string interfaceName, string parameter)
        {
            _helper.SendRequest(interfaceName, parameter);
        }
    }
}
