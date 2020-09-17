using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager> {

    private Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();
    //xml配置文件
    private XElement root;
    public void LoadXML(string p_dataName)
    {
        
        root = XmlHelp.GetRoot(p_dataName);
        IEnumerable<XElement> modelsXml = root.Element("Models").Elements();
        foreach (var item in modelsXml)
        {
            LoadModel(item);
        }
    }
    void LoadModel(XElement xElement) {
        string loadPath = xElement.Attribute("Path").Value;

        //GameObject goTemp = HiAssetUtil.Load<GameObject>(loadPath) as GameObject;
        GameObject goTemp = null;
        try
        {
            goTemp = Resources.Load(loadPath) as GameObject;
        }
        catch (System.Exception)
        {

            Debug.LogError("路径出错： " + loadPath);
        }
        
        if (goTemp == null) {
            Debug.LogError(xElement.Attribute("Name").Value + "当前路径没有找到对象" + loadPath);
        }
        GameObject go = GameObject.Instantiate(goTemp);
        go.name = xElement.Attribute("Name").Value;
        //添加进管理包中
        models.Add(xElement.Attribute("Name").Value, go);
        //判断是否有父物体
        if (xElement.Attribute("Parent") !=null) {
            go.transform.parent = GetGameObject(xElement.Attribute("Parent").Value).transform;
        }
        Vector3 position = VectorUtil.XmlToVector3(xElement.Element("Position"));
        go.transform.localPosition = position;

        Vector3 rotation = VectorUtil.XmlToVector3(xElement.Element("Rotation"));
        go.transform.localEulerAngles = rotation;

        Vector3 scale = VectorUtil.XmlToVector3(xElement.Element("Scale"));
        go.transform.localScale = scale;
        if (xElement.Element("Childs")!=null) {
            AddChild(xElement.Element("Childs"),go);
        }
      
    }
    void AddChild(XElement xElement,GameObject go) {
        IEnumerable<XElement> childXml = xElement.Elements("Child");
        foreach (var item in childXml)
        {
            string childName = item.Attribute("Name").Value;
            string childPath = item.Attribute("Path").Value;
            Transform t = go.transform.Find(childPath);
            if (t == null)
            {
                Debug.LogError(go.name+" 没有这个子物体" + childName);
            }
            else {
                AddModel(childName, t.gameObject);
               
                if (item.Element("Childs") != null)
                {
                   
                    AddChild(item.Element("Childs"), t.gameObject);
                }
            }
        }
    }
    public void AddModel(string name, GameObject go) {
        //添加进管理包中
        if (!models.Keys.Contains(name)) {
            models.Add(name, go);
        }
        
    }
    /// <summary>
    /// 根据名称获取对象
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetGameObject(string name) {
        GameObject go = null;
        if (models.ContainsKey(name))
        {
            go = models[name];
        }
        else {
            Debug.LogError("Models中没有包含 " + name + " 这个对象");
        }
       
        return go;
    }
    public void ShowModels() {
        for (int i = 0; i < models.Keys.Count; i++)
        {
            Debug.Log(models.Keys.ToList()[i]+ ":::" + models.Values.ToList()[i] );
        }
    }
    /// <summary>
    /// 设置对象的显示 隐藏
    /// </summary>
    /// <param name="name">对象名称</param>
    /// <param name="isActive">是否显示</param>
    public void SetGameObjectActive(string name, bool isActive) {
        GetGameObject(name).SetActive( isActive);
    }
}
