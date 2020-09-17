using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


public class XmlHelp{
    public static XElement GetRoot(TextAsset file)
    {
        return XElement.Parse(file.text);
    }
    public static XElement GetRoot(string fileName)
    {
        TextAsset textAsset = Resources.Load(ConfigValue.XMLDATAPATH + fileName ) as TextAsset;
        //Resources.u(ConfigValue.XMLDATAPATH + fileName + ".xml");
        return GetRoot(textAsset); ;
    }
}
