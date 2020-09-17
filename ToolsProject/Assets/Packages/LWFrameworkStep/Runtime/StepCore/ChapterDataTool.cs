using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ChapterDataTool : MonoBehaviour {
    //xml配置文件
    private XElement root;
    public bool isLoadData = false;
     

    public string dataName;
    public int int_data;
    public float float_data;
    public Vector3 vector3_data;
    public bool bool_data;
    public Texture2D texture2d_data;

    public void PrintData()
    {
        Debug.Log("int_data=" + int_data);
        Debug.Log("float_data=" + float_data);
        Debug.Log("Vector3_data=" + vector3_data);
        Debug.Log("bool_data=" + bool_data);
    }
    public void LoadXml() {
        if (dataName != "") {
            root = XmlHelp.GetRoot(dataName);
            isLoadData = true;
            //Debug.Log(dataName);
        }
        
    }

    public void GetChapter() {
       List<XElement>list =  root.Element("Chapters").Elements("Chapter").ToList();
        foreach (var item in list)
        {
            Debug.Log(item );
        }
    }
}
