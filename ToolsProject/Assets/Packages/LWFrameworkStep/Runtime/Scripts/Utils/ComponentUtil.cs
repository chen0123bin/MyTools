using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using UnityEngine;

public class ComponentUtil  {

    
    //添加所有的Component
    public static void AddComponents(IEnumerable<XElement> components, GameObject gameObject)
    {
        foreach (var item in components)
        {
            Debug.Log(item.Name);
            string componentName = item.Attribute("Name").Value;
            Type type = Type.GetType(componentName);
            object target = gameObject.AddComponent(type);
            switch (componentName)
            {
                case "Animator":
                    Debug.Log("添加动画");
                    break;
                default:
                    InitValue(target, item.Attribute("InitValue").Value);
                    break;
            }
        }
        
    }

    //添加所有的Component
    public static void AddComponents(XElement component, GameObject gameObject)
    {
        foreach (var item in component.Elements())
        {
            string componentName = item.Attribute("Name").Value;
            if (item.Attribute("OperateType").Value == "Add")
            {
                Type type = Type.GetType(componentName);
                object target = gameObject.AddComponent(type);
                
                switch (componentName)
                {
                    case "Animator":
                        Debug.Log("添加动画");
                        break;
                    case "BoxCollider":
                        AddBoxCollider(gameObject, item);
                        break;
                    case "Rigidbody":
                        AddRigidbody(gameObject, item);
                        break; 
                    case "StepControl_TweenMove":
                        //////1.GetMethod(需要调用的方法名称)  
                        //MethodInfo method = type.GetMethod("Init");
                        //////2.调用的实例化方法（非静态方法）需要创建类型的一个实例  
                        //object obj = Activator.CreateInstance(type);
                        //////3.方法需要传入的参数  
                        //object[] parameters = new object[] { item.Element("Tween") };
                        //////4.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）  
                        //////相应地调用静态方法时，Invoke的第一个参数为null  
                        //method.Invoke(obj, parameters);
                        gameObject.GetComponent<StepControl_TweenMove>().InitEnd(item.Element("End"));
                        if (item.Element("Start")!=null) {
                            gameObject.GetComponent<StepControl_TweenMove>().InitStart(item.Element("Start"));
                        }
                        break;
                    case "StepControl_ChangeTexture":                     
                        gameObject.GetComponent<StepControl_ChangeTexture>().InitEnd(item.Element("End"));
                        if (item.Element("Start") != null)
                        {
                            gameObject.GetComponent<StepControl_ChangeTexture>().InitStart(item.Element("Start"));
                        }
                        break;
                    case "StepControl_ContentMessage":
                        gameObject.GetComponent<StepControl_ContentMessage>().InitEnd(item.Element("End"));
                        if (item.Element("Start") != null)
                        {
                            gameObject.GetComponent<StepControl_ContentMessage>().InitStart(item.Element("Start"));
                        }
                        break;
                    case "StepControl_Animator":
                        gameObject.GetComponent<StepControl_Animator>().InitEnd(item.Element("End"));
                        if (item.Element("Start") != null)
                        {
                            gameObject.GetComponent<StepControl_Animator>().InitStart(item.Element("Start"));
                        }
                        break;
                    case "StepControl_AnimatorDynamic":
                        gameObject.GetComponent<StepControl_AnimatorDynamic>().InitEnd(item.Element("End"));
                        if (item.Element("Start") != null)
                        {
                            gameObject.GetComponent<StepControl_AnimatorDynamic>().InitStart(item.Element("Start"));
                        }
                        break;
                    case "StepControl_ChildTweenMove":
                        gameObject.GetComponent<StepControl_ChildTweenMove>().InitEnd(item.Element("End"));
                        InitValue(target, item.Attribute("InitValue").Value);
                        break;
                    case "StepControl_TweenPath":
                        gameObject.GetComponent<StepControl_TweenPath>().InitEnd(item.Element("TweenPath"));
                        break;
                    case "StepControl_Rotate":
                        gameObject.GetComponent<StepControl_Rotate>().InitEnd(item.Element("Rotate"));
                        break;
                    case "Other_TweenPath":
                        gameObject.GetComponent<Other_TweenPath>().Init(item.Element("TweenPath"));
                        break;
                    case "RunTimeAddObject":
                        gameObject.GetComponent<RunTimeAddObject>().Add(item.Attribute("ChildName").Value);
                        break;
                    default:
                        InitValue(target, item.Attribute("InitValue").Value);
                        break;
                }
            }
            else if (item.Attribute("OperateType").Value == "Remove") {
                GameObject.Destroy(gameObject.GetComponent(componentName));
            }
           
        }
    }
    static void AddBoxCollider(GameObject go, XElement xElement) {
        BoxCollider boxCollider = go.GetComponent<BoxCollider>();
        if (!boxCollider) {
            boxCollider = go.AddComponent<BoxCollider>();
        }
        boxCollider.size = VectorUtil.XmlToVector3(xElement.Element("Size"));
        if (xElement.Attribute("IsTrigger").Value == "true") {
            boxCollider.isTrigger = true;
        }
    }
    static void AddRigidbody(GameObject go, XElement xElement)
    {
        Rigidbody rigidbody = go.GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            rigidbody = go.AddComponent<Rigidbody>();
        }
        if (xElement.Attribute("IsKinematic").Value == "true")
        {
            rigidbody.isKinematic = true;
        }
    }
    public static void AddComponent(string scriptName, GameObject gameObject) {
        Type type = Type.GetType(scriptName);
        object target = gameObject.AddComponent(type);       
    }
    public static void AddComponent(string scriptName,string scriptValue, GameObject gameObject)
    {
        Type type = Type.GetType(scriptName);
        object target = gameObject.AddComponent(type);
        InitValue(target, scriptValue);
    }
    /// <summary>
    /// 解析InitValue的字符串数据
    /// </summary>
    /// <param name="target"></param>
    /// <param name="xmlStr">字符串数据</param>
    static void InitValue(object target, string xmlStr)
    {
        if (xmlStr.Length < 1) return;
        string[] xmlValues = xmlStr.Split(';');
        for (int i = 0; i < xmlValues.Length; i++)
        {

            string[] filed = xmlValues[i].Split('=');
            SetValue(target, filed[0], filed[1]);
        }
    }
    /// <summary>
    /// 通过反射设置参数
    /// </summary>
    /// <param name="target">Type对象</param>
    /// <param name="name">属性名称</param>
    /// <param name="value">值</param>
    static void SetValue(object target, String name, object value)
    {
        FieldInfo fInfo = target.GetType().GetField(name);
        if (fInfo == null) {
            Debug.LogError("不存在该属性" + name);
        }

        fInfo.SetValue(target, Convert.ChangeType(value, fInfo.FieldType));
    }
}
