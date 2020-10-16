using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepRuntimeData : MonoSingleton<StepRuntimeData>
{
    [OnValueChanged("AddSceneObject")]
    public GameObject m_AddObj;
    [TableList]
    public List<SceneObject> m_SceneObjectList;
    public List<string> m_SceneObjectNameList ;


    private Vector3[] m_GizmosVector3Array;

    public Vector3[] GizmosVector3Array {
        set => m_GizmosVector3Array = value;
    }
    public List<string> SceneObjectNameList {
        get => m_SceneObjectNameList;
    }
    /// <summary>
    /// 根据名称获取对象
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public GameObject FindGameObject(string goName) {
        SceneObject sceneObject =  m_SceneObjectList.Find((item) =>
        {
            return item.m_ObjName == goName;
        });
        if (sceneObject == null)
        {
            LWDebug.LogError(string.Format("当前未包含了这个对象：{0}", goName));
            return null;
        }
        else {
            return sceneObject.Obj;
        }
    }
    void AddSceneObject()
    {
        if (m_AddObj != null)
        {
            string objName = m_AddObj.name;
            if (m_SceneObjectNameList.Contains(objName))
            {
                LWDebug.LogError(string.Format("当前已经包含了这个对象:{0}", objName));
            }
            else {
                string objPath = m_AddObj.GetHierarchyPath();
                SceneObject sceneObject = new SceneObject() { m_ObjName = objName, m_ObjPath = objPath };
                m_SceneObjectList.Add(sceneObject);
                m_SceneObjectNameList.Add(objName);
            }
           
        }
        m_AddObj = null;
    }
    [Button("刷新数据")]
    void RefreshSceneObject()
    {
        m_SceneObjectNameList.Clear();
        for (int i = 0; i < m_SceneObjectList.Count; i++)
        {
            //查找当前路径是否有对象
            GameObject findGo = GameObject.Find(m_SceneObjectList[i].m_ObjPath);
            if (findGo == null)
            {
                m_SceneObjectList.RemoveAt(i);
                i--;
            }
            else {
                m_SceneObjectNameList.Add(m_SceneObjectList[i].m_ObjName);
            }
        }
    }
    [Button("添加帮助脚本")]
    void AddHelp() {
        for (int i = 0; i < m_SceneObjectList.Count; i++)
        {
            //查找当前路径是否有对象
            GameObject findGo = GameObject.Find(m_SceneObjectList[i].m_ObjPath);
            if (findGo != null)
            {
                var help = findGo.GetComponent<StepEditorHelp>();
                if (help == null)
                {
                    findGo.AddComponent<StepEditorHelp>();
                }
                
            }
        }
    }
    [Button("移除帮助脚本")]
    void RemoveHelp() {
        for (int i = 0; i < m_SceneObjectList.Count; i++)
        {
            //查找当前路径是否有对象
            GameObject findGo = GameObject.Find(m_SceneObjectList[i].m_ObjPath);
            if (findGo != null)
            {
               var help =  findGo.GetComponent<StepEditorHelp>();
                if (help != null) {
                    GameObject.DestroyImmediate(help);
                }
            }
           
        }
    }

    

    private void OnDrawGizmos()
    {

        if (m_GizmosVector3Array == null || m_GizmosVector3Array.Length == 0)
        {

            return;
        }
        for (int i = 0; i < m_GizmosVector3Array.Length; i++)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(m_GizmosVector3Array[i], 0.1f);
            if (i + 1 < m_GizmosVector3Array.Length)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(m_GizmosVector3Array[i], m_GizmosVector3Array[i + 1]);
            }
        }

    }
}

