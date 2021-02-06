using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using Cysharp.Threading.Tasks;

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
        /// 所有UI的父节点缓存，每次使用的都记录一次避免多次查找
        /// </summary>
        private Dictionary<string, Transform> m_UIParentDicCache = new Dictionary<string, Transform>();
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
        /// 根据ab路径创建一个view 实体
        /// </summary>
        /// <param name="abPath">ab的路径</param>
        /// <returns></returns>
        public async UniTask<GameObject> CreateViewEntityAsync(string abPath)
        {

            GameObject uiGameObject = await m_UILoad.LoadUIGameObjectAsync(abPath);
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
        /// 根据特性 获取父级
        /// </summary>
        /// <param name="findType">查找的类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public Transform GetParent( FindType findType, string param)
        {
            Transform ret = null;
            if (m_UIParentDicCache.ContainsKey(param))
            {
                ret = m_UIParentDicCache[param];
            }
            else if (findType == FindType.Name)
            {
                GameObject gameObject = GameObject.Find(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDicCache.Add(param, ret);
            }
            else if (findType == FindType.Tag)
            {
                GameObject gameObject = GameObject.FindGameObjectWithTag(param);
                if (gameObject == null)
                {
                    LWDebug.LogError(string.Format("当前没有找到{0}这个Tag GameObject对象", param));
                }
                ret = gameObject.transform;
                m_UIParentDicCache.Add(param, ret);
            }
            return ret;
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

