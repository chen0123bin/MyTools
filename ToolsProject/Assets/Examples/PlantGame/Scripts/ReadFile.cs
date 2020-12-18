using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ReadFile  {
    public static string GetStreamAsset(string fileName) {
        string path = Application.streamingAssetsPath +"\\"+ fileName;
        string ret = File.ReadAllText(path);
        return ret;
    }
    //Assets外部文件
    public static string GetRoot(string fileName)
    {
        string dataPath = Application.dataPath;
        string rootPath = dataPath.Substring(0, dataPath.LastIndexOf("/") + 1);
        string path = rootPath + "\\" + fileName;
        string ret = File.ReadAllText(path);
        return ret;
    }
}
