using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LWFramework.UI
{
    public  class BaseUIView: IUIView
    {
        /// <summary>
        /// UIGameObject
        /// </summary>
        protected GameObject m_Entity;
        protected CanvasGroup m_CanvasGroup;
        /// <summary>
        /// View的数据
        /// </summary>
        protected ViewData m_ViewData;
        public ViewData ViewData { get => m_ViewData; set => m_ViewData = value; }
        /// <summary>
        /// ViewId 动态生成
        /// </summary>
        private int m_ViewId;
        public int ViewId { get => m_ViewId; set => m_ViewId = value; }
        private bool m_IsOpen = false;
        public bool IsOpen {
            get => m_IsOpen;
            set => m_IsOpen = value;
        }
        public virtual void CreateView(GameObject gameObject) {
            m_Entity = gameObject;
            //view上的组件
            UIUtility.Instance.SetViewElement(this, gameObject);
            m_CanvasGroup = m_Entity.GetComponent<CanvasGroup>();
            if (m_CanvasGroup == null) {
                LWDebug.LogError(string.Format("{0}上没有CanvasGroup这个组件", m_Entity.name));
            }
            ViewId = UIUtility.Instance.ViewId;        
            m_ViewData = new ViewData();
            m_ViewData.OnDataChange = OnDataChange;
            OnCreateView();
        }
        public virtual void OnCreateView() { 
        
        }
        public virtual void OnDataChange(string dataName) { }
        /// <summary>
        /// 打开view
        /// </summary>
        public virtual void OpenView() {
            //_entity.SetActive(true);
            m_CanvasGroup.SetActive(true);
            m_IsOpen = true;
        }
        /// <summary>
        ///关闭view 
        /// </summary>
        public virtual void CloseView() {
            //_entity.SetActive(false);
            m_CanvasGroup.SetActive(false);
            m_IsOpen = false;
        }
       
        //更新VIEW
        public virtual void UpdateView()
        {
            
        }             
        //删除VIEW
        public virtual void ClearView()
        {
            m_ViewData.Clear();
            GameObject.Destroy(m_Entity);
        }
    }    
}

