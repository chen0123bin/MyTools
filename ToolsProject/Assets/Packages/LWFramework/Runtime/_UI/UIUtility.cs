﻿using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace LWFramework.UI {
    public class UIUtility :Singleton<UIUtility>
    {
        private IUILoad m_UILoad;
        /// <summary>
        /// 设置自定义的UI加载类
        /// </summary>
        public IUILoad CustomUILoad {
            set {
                m_UILoad = value;
            }
        }
        public UIUtility() {
            m_UILoad = new UILoadAB();
        }
      
        private  int m_ViewId;
        public int ViewId {
            get => m_ViewId++;
        }      
        /// <summary>
        /// 根据ab路径创建一个view 实体
        /// </summary>
        /// <param name="abPath">ab的路径</param>
        /// <returns></returns>
        public GameObject CreateViewEntity(string abPath) {
            
            GameObject uiGameObject = m_UILoad.LoadUIGameObject(abPath);        
            return uiGameObject;
        }       
        /// <summary>
        /// 根据ab路径获取精灵图片
        /// </summary>
        /// <param name="abPath">ab的路径</param>
        /// <returns></returns>
        public Sprite GetSprite(string abPath)
        {        
            return m_UILoad.GetSprite(abPath);
        }
        /// <summary>
        /// 根据特性获取UI对象
        /// </summary>
        /// <param name="entity"></param>
        public void SetViewElement(object entity,GameObject uiGameObject)
        {
            Type type = entity.GetType();
            //获取字段属性
            FieldInfo[] objectFields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //遍历字段属性
            for (int i = 0; i < objectFields.Length; i++)
            {
                //获取属性上的特性
                object[] attributes = objectFields[i].GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is UIElementAttribute)
                    {
                        UIElementAttribute uiElement = attribute as UIElementAttribute;
                        try
                        {
                            UnityEngine.Object obj = uiGameObject.transform.Find(uiElement.m_RootPath).GetComponent(objectFields[i].FieldType);
                            //给当前的字段赋值
                            objectFields[i].SetValue(entity, obj);
                            //处理初始化动态图片
                            if (uiElement.m_ResPath != "")
                            {
                                if (objectFields[i].FieldType == typeof(UnityEngine.UI.Image))
                                {
                                    ((UnityEngine.UI.Image)obj).sprite = GetSprite( uiElement.m_ResPath);
                                }
                                else if (objectFields[i].FieldType == typeof(UnityEngine.UI.Button))
                                {
                                    ((UnityEngine.UI.Button)obj).GetComponent<UnityEngine.UI.Image>().sprite = GetSprite( uiElement.m_ResPath);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(string.Format("当前: {0} 路径上没有找到对应的物体   {1}", uiElement.m_RootPath, e.StackTrace));
                        }


                    }
                }
            }
        }
    }

}

