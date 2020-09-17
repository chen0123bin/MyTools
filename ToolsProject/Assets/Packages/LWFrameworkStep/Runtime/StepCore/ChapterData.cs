using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;

public class ChapterData  {
    
    //xml配置文件
    private XElement root;
    private List<XElement> chapters;
    // Use this for initialization
    public ChapterData(string fileName)
    {
        root = XmlHelp.GetRoot(fileName);
        chapters = root.Element("Chapters").Elements("Chapter").ToList();
        
        //foreach (var item in chapters)
        //{
        //    Debug.Log(item.Attribute("Remark").Value);
        //}   

    }

    public List<XElement> GetXElementData() {
        return chapters;
    }
}
