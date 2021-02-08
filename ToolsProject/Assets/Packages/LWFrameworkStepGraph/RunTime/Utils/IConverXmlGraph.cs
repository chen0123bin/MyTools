using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public interface IConverXmlGraph 
{
    /// <summary>
    /// 转换成xml文件
    /// </summary>
    /// <returns></returns>
    XElement ToXml();
    /// <summary>
    /// 导入xml文件初始化
    /// </summary>
    /// <param name="xElement"></param>
    void InputXml(XElement xElement);
}
