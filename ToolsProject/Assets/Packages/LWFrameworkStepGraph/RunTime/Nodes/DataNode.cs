using LWNode;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
public class DataNode : BaseStepNode
{

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
   // [OnValueChanged("AddSceneObject")]
    private GameObject m_AddObj;
    [ListDrawerSettings(CustomAddFunction = "AddSceneObject", OnTitleBarGUI = ("RefreshSceneObject"))]
    public List<SceneObject> m_SceneObjectList;
    public List<string> m_SceneObjectNameList;
    public List<string> SceneObjectNameList
    {
        get => m_SceneObjectNameList;
    }
    /// <summary>
    /// 根据名称获取对象
    /// </summary>
    /// <param name="goName"></param>
    /// <returns></returns>
    public GameObject FindGameObject(string goName)
    {
        SceneObject sceneObject = m_SceneObjectList.Find((item) =>
        {
            return item.m_ObjName == goName;
        });
        if (sceneObject == null)
        {
            LWDebug.LogError(string.Format("当前未包含了这个对象：{0}", goName));
            return null;
        }
        else
        {
            return sceneObject.Obj;
        }
    }
    public void AddSceneObject()
    {
#if UNITY_EDITOR      
        m_AddObj = UnityEditor.Selection.activeGameObject;
        if (m_AddObj != null)
        {
            string objName = m_AddObj.name;
            if (m_SceneObjectNameList.Contains(objName))
            {
                LWDebug.LogError(string.Format("当前已经包含了这个对象:{0}", objName));
            }
            else
            {
                string objPath = m_AddObj.GetHierarchyPath();
                SceneObject sceneObject = new SceneObject() { m_ObjName = objName, m_ObjPath = objPath };
                m_SceneObjectList.Add(sceneObject);
                m_SceneObjectNameList.Add(objName);
            }

        }
        m_AddObj = null;
#endif
    }
    void RefreshSceneObject()
    {
#if UNITY_EDITOR
        if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
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
                else
                {
                    m_SceneObjectNameList.Add(m_SceneObjectList[i].m_ObjName);
                }
            }
        }
#endif
    }

    public override void StartTriggerList()
    {
       
    }

    public override void StopTriggerList()
    {
    }
}
[Serializable]
public class SceneObject
{
    public string m_ObjName;
    public string m_ObjPath;
    private GameObject m_Obj;
    public GameObject Obj
    {
        get
        {
            if (m_Obj == null)
            {
                m_Obj = GameObject.Find(m_ObjPath);
            }
            return m_Obj;
        }
    }
}
