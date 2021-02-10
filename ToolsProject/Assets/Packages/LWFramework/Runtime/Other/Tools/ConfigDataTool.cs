using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using LWFramework.Core;
using Cysharp.Threading.Tasks;

public class ConfigDataTool
{
    static string ConfigFilePath {
        get {
#if UNITY_ANDROID
           return Application.persistentDataPath;
#elif UNITY_STANDALONE_WIN
            return LWUtility.ProjectRoot;            
#elif UNITY_WEBGL 
            return Application.streamingAssetsPath;
#elif UNITY_EDITOR
            return LWUtility.ProjectRoot;
#endif
        }
    }
    /// <summary>
    /// 读取文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">文件名</param>
    /// <param name="isEnc">是否加密</param>
    /// <returns></returns>
    public static T ReadData<T>(string fileName,bool isEnc = true) {       
        try
        {
            string dataStr = FileTool.ReadFromFile(fileName, ConfigFilePath);
            if (isEnc) {
                dataStr = AESTool.AesDecrypt(dataStr);
            }           
            return JsonMapper.ToObject<T>(dataStr);
        }
        catch (System.Exception e)
        {
            LWDebug.LogWarning($"{ConfigFilePath}:路径中没用 {fileName}文件");
            LWDebug.LogWarning(e.Message);
            return default;
        }        
    }
   
    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="data">数据对象</param>
    /// <param name="isEnc">是否加密</param>
    public static void Create(string fileName,object data, bool isEnc = true) {
        string dataStr =  JsonMapper.ToJson(data);
        if (isEnc) {
            dataStr = AESTool.AesEncrypt(dataStr);
        }       
        if (FileTool.ExistsFile(fileName, ConfigFilePath)) {
            FileTool.DeleteFile(fileName, ConfigFilePath);
        }
        FileTool.CreateFile(fileName, ConfigFilePath);
        FileTool.WriteToFile(fileName, dataStr, ConfigFilePath);
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    public static void Delete(string fileName) {
        if (FileTool.ExistsFile(fileName, ConfigFilePath))
        {
            FileTool.DeleteFile(fileName, ConfigFilePath);
        }
    }
}
