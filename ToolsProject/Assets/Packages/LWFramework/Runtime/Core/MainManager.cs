﻿using LWFramework.FMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace LWFramework.Core {
    /// <summary>
    /// 非热更环境主管理器
    /// </summary>
   // [ManagerClass(ManagerType.Main)]
    public class MainManager : Singleton<MainManager>, IManager
    {
        private Type m_FirstFSMState;
        //外部设置第一个启动的状态
        public Type FirstFSMState { set => m_FirstFSMState = value; }
        //热更DLL中所有的type
        private List<Type> m_TypeHotfixArray;
        //管理热更中的所有的Type
        private Dictionary<string, List<AttributeTypeData>> m_AttrTypeHotfixDic;
        private Dictionary<string, IManager> m_ManagerDic;
        private List<IManager> m_ManagerList;
        public MainManager()
        {
            m_ManagerDic = new Dictionary<string, IManager>();
            m_ManagerList = new List<IManager>();
            Type[] typeArray = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var t in typeArray)
            {
                if (t.IsClass)
                {
                    var attr = (ManagerClass)t.GetCustomAttribute(typeof(ManagerClass), false);
                    if (attr != null && attr.managerType == ManagerType.Normal)
                    {
                        IManager manager = Activator.CreateInstance(t) as IManager;
                        ////注册
                        AddManager(manager);

                    }
                }
            }
        }
    
        public void InitHotfixManager(List<Type> p_TypeArray)
        {
            m_AttrTypeHotfixDic = new Dictionary<string, List<AttributeTypeData>>();
            this.m_TypeHotfixArray = p_TypeArray;

            //将所有带有特性的类进行字典管理
            foreach (var item in p_TypeArray)
            {
                if (item.IsClass)
                {
                    var attrs = item.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (!m_AttrTypeHotfixDic.ContainsKey(attr.ToString()))
                        {
                            m_AttrTypeHotfixDic[attr.ToString()] = new List<AttributeTypeData>();
                        }
                        AttributeTypeData classData = new AttributeTypeData { attribute = (Attribute)attr, type = item };
                        m_AttrTypeHotfixDic[attr.ToString()].Add(classData);
                    }

                }
            }
            //获取所有带ManagerHotfixClass特性的类，这些都属于管理类
            List<AttributeTypeData> normalManamgerType = GetTypeListByAttr<ManagerHotfixClass>();
            for (int i = 0; normalManamgerType!=null && i < normalManamgerType.Count; i++)
            {
                Attribute attr = normalManamgerType[i].attribute;
                if (((ManagerHotfixClass)attr).managerType == ManagerHotfixType.NormalHotfix)
                {
                    // LWDebug.Log("ManagerHotfixClass: " + attr + "______ " + "   Type:" + normalManamgerType[i].type.ToString() + "____" );
                    IManager manager = Activator.CreateInstance(normalManamgerType[i].type) as IManager;
                    ////注册
                    AddManager(manager);
                }
            }
        }
       
        public void AddManager(string name, IManager t) {
            t.Init();
            m_ManagerDic.Add(name, t);
            m_ManagerList.Add(t);
        }
        public void AddManager(IManager t)
        {
            t.Init();
            m_ManagerDic.Add(t.GetType().ToString(), t);
            m_ManagerList.Add(t);
        }
        public T GetManager<T>()
        {
            IManager manager;
            if (m_ManagerDic.TryGetValue(typeof(T).ToString(), out manager))
            {
                return (T)m_ManagerDic[typeof(T).ToString()];
            }
            else
            {
                LWDebug.LogError(typeof(T).ToString() + " 这个Manager 不存在，请先检查是否加入了ManagerClass特性或是否主动添加过Manager");
                return (T)manager;
            }
        }
        public void Init()
        {
          
        }

        public void Update()
        {
            for (int i = 0; i < m_ManagerList.Count; i++)
            {
                m_ManagerList[i].Update();
            }  
        }

       
        /// <summary>
        /// 启动流程管理
        /// </summary>
        public void StartProcedure()
        {
            GetManager<IFSMManager>().InitFSMManager();
            //找到所有的流程管理类
            List<AttributeTypeData> procedureList = GetManager< IFSMManager>().GetFsmClassDataByName(nameof(FSMName.Procedure));
            if (procedureList.Count > 0)
            {
                //创建一个流程管理状态机       
                FSMStateMachine stateMachine = new FSMStateMachine(nameof(FSMName.Procedure), procedureList);
                GetManager<IFSMManager>().RegisterFSM(stateMachine);
                if (m_FirstFSMState != null)
                {
                    GetManager<IFSMManager>().GetFSMProcedure().SwitchState(m_FirstFSMState);
                }
                else {
                    GetManager<IFSMManager>().GetFSMProcedure().StartFirst();
                }
                
            }
            else {
                LWDebug.LogWarning("未找到第一个Procedure");
            }
           
        }
        /// <summary>
        /// 根据特性去获取对应的所有type
        /// </summary>
        /// <typeparam name="T">特性</typeparam>
        /// <returns></returns>
        public List<AttributeTypeData> GetTypeListByAttr<T>()
        {
            if (m_AttrTypeHotfixDic==null || !m_AttrTypeHotfixDic.ContainsKey(typeof(T).FullName))
            {
                LWDebug.LogWarning("当前域下找不到这个包含这个特性的类" + typeof(T).FullName);
                return null;
            }
            else
            {
                return m_AttrTypeHotfixDic[typeof(T).FullName];
            }

        }
    }
    public enum ManagerType
    {
        Main, Normal
    }
    public class ManagerClass : Attribute
    {
        public ManagerType managerType;
        public ManagerClass(ManagerType managerType)
        {
            this.managerType = managerType;
        }
    }
    public enum ManagerHotfixType
    {
        MainHotfix, NormalHotfix
    }
    public class ManagerHotfixClass : Attribute
    {
        public ManagerHotfixType managerType;
        public ManagerHotfixClass(ManagerHotfixType managerType)
        {
            this.managerType = managerType;
        }
    }
    public class AttributeTypeData
    {
        public Attribute attribute;
        public Type type;
    }
}
