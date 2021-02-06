using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using LWFramework.Core;

public class ConfigDataTool
{
    static string ConfigFilePath {
        get {
#if UNITY_ANDROID
           return Application.persistentDataPath;
#elif UNITY_STANDALONE_WIN
            return LWUtility.ProjectRoot;
#elif UNITY_EDITOR
            return LWUtility.ProjectRoot;
#else 
            return "";
#endif
        }
    }
    public static T ReadData<T>(string fileName) {       
        try
        {
            string encDataStr = FileTool.ReadFromFile(fileName, ConfigFilePath);
            string dataStr = AESTool.AesDecrypt(encDataStr);
            return JsonMapper.ToObject<T>(dataStr);
        }
        catch (System.Exception e)
        {
            LWDebug.LogWarning(ConfigFilePath+ " 中没有config文件，继续使用LWGlobalAsset配置文件");
            return default;
        }        
    }
    public static void Create(string fileName,object data) {
        string dataStr =  JsonMapper.ToJson(data);
        string encDataStr = AESTool.AesEncrypt(dataStr);
        if (FileTool.ExistsFile(fileName, ConfigFilePath)) {
            FileTool.DeleteFile(fileName, ConfigFilePath);
        }
        FileTool.CreateFile(fileName, ConfigFilePath);
        FileTool.WriteToFile(fileName, encDataStr, ConfigFilePath);
    }
}
