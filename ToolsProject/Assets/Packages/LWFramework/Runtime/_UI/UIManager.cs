using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LWFramework.UI {
    /// <summary>
    /// 所有的UI管理器
    /// </summary>
    //[ManagerClass(ManagerType.Normal)]
    public class UIManager : IManager, IUIManager
    {
        /// <summary>
        /// 所有的view字典
        /// </summary>
        private Dictionary<string, IUIView> m_UIViewDic;
        /// <summary>
        /// 所有UI的父节点
        /// </summary>
        private Dictionary<string, Transform> m_UIParentDic;

        #region 获取Canvas编辑节点

        private Transform _editTransform;
        private Transform EditTransform
        {
            get
            {
                if (_editTransform == null)
                {
                    _editTransform = GameObject.Find("LWFramework/Canvas/Edit").transform;
                }
                return _editTransform;
            }
        }

        #endregion
        public void Init()
        {
            m_UIViewDic = new Dictionary<string, IUIView>();
            m_UIParentDic = new Dictionary<string, Transform>();
            //启动之后隐藏编辑层
            EditTransform.gameObject.SetActive(false);
        }
        /// <summary>
        /// 更新所有的View
        /// </summary>
        public void Update()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                if (item.IsOpen)
                {
                    item.UpdateView();
                }
            }
        }
        ///// <summary>
        ///// 创建VIEW
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="parent">父节点，当创建子View的时候为必填</param>
        ///// <returns></returns>
        //public BaseUIView CreateView<T>(Transform parent)
        //{

        //    BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;
        //    //获取UIViewDataAttribute特性
        //    var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
        //    if (attr != null)
        //    {
        //        GameObject uiGameObject = uiGameObject = parent.Find(attr.loadPath).gameObject;
        //        if (uiGameObject == null)
        //        {
        //            LWDebug.LogError("没有找到这个UI对象" + attr.loadPath);
        //        }

        //        //初始化UI
        //        uiView.CreateView(uiGameObject);

        //    }
        //    else
        //    {
        //        LWDebug.Log("没有找到UIViewDataAttribute这个特性");
        //    }

        //    LWDebug.Log("UIManager：" + typeof(T).ToString());
        //    return uiView;
        //}

        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void OpenView<T>()
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
            {
                uiViewBase = CreateView<T>();
                m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
            }
            if (!uiViewBase.IsOpen)
                uiViewBase.OpenView();
        }
        public void OpenView<T>(string viewName, GameObject uiGameObject = null)
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                uiViewBase = CreateView<T>(uiGameObject);
                m_UIViewDic.Add(viewName, uiViewBase);
            }
            if (!uiViewBase.IsOpen)
                uiViewBase.OpenView();
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView<T>()
        {
            CloseView(typeof(T).ToString());
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView(string viewName)
        {
            IUIView uiViewBase;
            if (m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                uiViewBase.CloseView();
            }
        }
        /// <summary>
        /// 获取VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetView<T>(string viewName = null)
        {
            if (viewName == null)
                return (T)m_UIViewDic[typeof(T).ToString()];
            else
                return (T)m_UIViewDic[viewName];
        }

        /// <summary>
        /// 关闭其他所有的View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseOtherView<T>()
        {
            CloseOtherView(typeof(T).ToString());
        }
        public void CloseOtherView(string viewName) {
            foreach (var item in m_UIViewDic.Keys)
            {
                if (item != viewName)
                {
                    m_UIViewDic[item].CloseView();
                }
            }
        }
        /// <summary>
        /// 关闭所有的view
        /// </summary>
        public void CloseAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                item.CloseView();
            }
        }
        /// <summary>
        /// 清理所有的view
        /// </summary>
        public void ClearAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                item.ClearView();
            }
            m_UIViewDic.Clear();
        }

        /// <summary>
        /// 创建一个VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private BaseUIView CreateView<T>(GameObject uiGameObject = null)
        {
            BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;
            //获取UIViewDataAttribute特性
            var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                //GameObject uiGameObject = null;
                //创建UI对象
                if (uiGameObject == null)
                {
                    uiGameObject = UIUtility.Instance.CreateViewEntity(attr.m_LoadPath);
                }
                Transform parent = GetParent(attr.m_FindType, attr.m_Param);
                if (uiGameObject == null)
                {
                    LWDebug.LogError("没有找到这个UI对象" + attr.m_LoadPath);
                }
                if (parent == null)
                {
                    LWDebug.LogError("没有找到这个UI父节点" + attr.m_Param);
                }
                if (parent != null)
                {
                    uiGameObject.transform.SetParent(parent, false);
                }
                //初始化UI
                uiView.CreateView(uiGameObject);
            }
            else
            {
                LWDebug.Log("没有找到UIViewDataAttribute这个特性");
            }
            LWDebug.Log("UIManager：" + typeof(T).ToString());
            return uiView;
        }
        /// <summary>
        /// 根据特性 获取父级
        /// </summary>
        /// <param name="findType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private Transform GetParent(FindType findType, string param)
        {
            Transform ret = null;
            if (m_UIParentDic.ContainsKey(param))
            {
                ret = m_UIParentDic[param];
            }
            else if (findType == FindType.Name)
            {
                GameObject gameObject = GameObject.Find(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDic.Add(param, ret);
            }
            else if (findType == FindType.Tag)
            {
                GameObject gameObject = GameObject.FindGameObjectWithTag(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个Tag GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDic.Add(param, ret);
            }
            return ret;
        }
    }
}

