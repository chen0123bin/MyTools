using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework.UI {
    
    public class ViewData 
    {           
        private Hashtable m_Data = new Hashtable();
        private Action<object> m_OnDataChange;
        /// <summary>
        /// 数据发生变化的处理
        /// </summary>
        public Action<object> OnViewDataChange {
            set {
                m_OnDataChange = value;
            }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="key">数据的key</param>
        /// <returns></returns>
        public T Get<T>(object key)
        {
            return (T)this[key];
        }
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key]
        {
            get
            {
                return m_Data != null && m_Data.ContainsKey(key) ? m_Data[key] : null;
            }
            set
            {
                if (m_Data != null)
                {
                    m_Data[key] = value;
                    m_OnDataChange?.Invoke(key);                  
                }
            }
        }
        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear() {
            m_Data.Clear();
        }
       
    }

}
