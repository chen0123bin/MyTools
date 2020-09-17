using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;

public class CreateXmlUtil : EditorWindow
{
    string inputStr="";
    [MenuItem("LWFramework/XmlUtil")]
    static void SavePrefabs_()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 340, 220);
        CreateXmlUtil window = (CreateXmlUtil)EditorWindow.GetWindowWithRect(typeof(CreateXmlUtil), wr, true, "XmlUtil");
        window.Show();
    }
    [MenuItem("GameObject/XMLUtil/生成模型坐标xml配置文件", false, -10)]
    private static void GetObjectXXml()
    {
        string str = "";
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            str += LogMode(Selection.gameObjects[i]);
            
        }
        Debug.Log(str);
        GUIUtility.systemCopyBuffer = str;

    }
    [MenuItem("GameObject/XMLUtil/生成Transform_xml配置文件", false, -10)]
    private static void GetPositionXml()
    {
        string str = "";
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            str += LogMode2(Selection.gameObjects[i]);

        }
        Debug.Log(str);
        GUIUtility.systemCopyBuffer = str;

    }
    [MenuItem("GameObject/XMLUtil/打开工具栏", false, -10)]
    private static void OpenWindows()
    {
        SavePrefabs_();

    }
    // Update is called once per frame
    void OnGUI()
    {
        //if (GUILayout.Button("生成配置文件", GUILayout.Width(200)))
        //{
        //    for (int i = 0; i < Selection.gameObjects.Length; i++)
        //    {
        //        Debug.Log(Selection.gameObjects[i]);
        //    }
          
               

        //}
        if (GUILayout.Button("生成坐标xml配置文件", GUILayout.Width(200)))
        {
            GetObjectXXml();
        }
        if (GUILayout.Button("生成碰撞xml配置文件", GUILayout.Width(200)))
        {
            BoxCollider boxCollider = Selection.activeGameObject.GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                Debug.LogError("当前物体没有盒子碰撞");
            }
            else {
                XElement collider = new XElement("Script");
                collider.SetAttributeValue("OperateType", "Add");
                collider.SetAttributeValue("Name", "BoxCollider");
                collider.SetAttributeValue("InitValue", "");
                collider.SetAttributeValue("IsTrigger", boxCollider.isTrigger);
                collider.Add(VectorUtil.Vector3ToXml("Size", boxCollider.size));
                Debug.Log(collider);
                GUIUtility.systemCopyBuffer = collider.ToString();
            }

          
        }

        inputStr =   TextField(inputStr, GUILayout.Width(300));
        if (GUILayout.Button("赋值坐标", GUILayout.Width(200)))
        {

            if (inputStr != "") {
                GameObject obj = Selection.activeGameObject;
                XElement xElement = XElement.Parse(inputStr);
                Vector3 position = VectorUtil.XmlToVector3(xElement.Element("Position"));
                obj.transform.localPosition = position;

                Vector3 rotation = VectorUtil.XmlToVector3(xElement.Element("Rotation"));
                obj.transform.localEulerAngles = rotation;

                Vector3 scale = VectorUtil.XmlToVector3(xElement.Element("Scale"));
                obj.transform.localScale = scale;
            }

        }
    }

    static string LogMode(GameObject selectGameObject) {
        XElement model = new XElement("Model");
        model.SetAttributeValue("Name", selectGameObject.name);
        model.SetAttributeValue("Path", "");
        if (selectGameObject.transform.parent)
            model.SetAttributeValue("Parent", selectGameObject.transform.parent.gameObject.name);
        model.Add(VectorUtil.Vector3ToXml("Position", selectGameObject.transform.localPosition));
        model.Add(VectorUtil.Vector3ToXml("Rotation", selectGameObject.transform.localEulerAngles));
        model.Add(VectorUtil.Vector3ToXml("Scale", selectGameObject.transform.localScale));

        return model.ToString();
    }

    static string LogMode2(GameObject selectGameObject)
    {
        XElement model = new XElement("X");
        model.Add(VectorUtil.Vector3ToXml("Position", selectGameObject.transform.localPosition));
        model.Add(VectorUtil.Vector3ToXml("Rotation", selectGameObject.transform.localEulerAngles));
        model.Add(VectorUtil.Vector3ToXml("Scale", selectGameObject.transform.localScale));

        return model.ToString().Substring(4, model.ToString().Length-8);
    }
    /// <summary>
    /// Add copy-paste functionality to any text field
    /// Returns changed text or NULL.
    /// Usage: text = HandleCopyPaste (controlID) ?? text;
    /// </summary>
    public static string HandleCopyPaste(int controlID)
    {
        if (controlID == GUIUtility.keyboardControl)
        {
            if (Event.current.type == EventType.KeyUp && (Event.current.modifiers == EventModifiers.Control || Event.current.modifiers == EventModifiers.Command))
            {
                if (Event.current.keyCode == KeyCode.C)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                    editor.Copy();
                }
                else if (Event.current.keyCode == KeyCode.V)
                {
                    Event.current.Use();
                    TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                    editor.Paste();
                    return editor.text;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// TextField with copy-paste support
    /// </summary>
    public static string TextField(string value, params GUILayoutOption[] options)
    {
        int textFieldID = GUIUtility.GetControlID("TextField".GetHashCode(), FocusType.Keyboard) + 1;
        if (textFieldID == 0)
            return value;

        // Handle custom copy-paste
        value = HandleCopyPaste(textFieldID) ?? value;

        return GUILayout.TextField(value);
    }
}
