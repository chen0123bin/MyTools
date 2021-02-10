using Cysharp.Threading.Tasks;
using libx;
using LitJson;
using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDataToolWeb
{
    static string ConfigFilePath
    {
        get
        {
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
    /// 读取网络文件文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">文件名</param>
    /// <param name="isEnc">是否加密</param>
    /// <returns></returns>
    public static async UniTask<T> ReadDataAsync<T>(string fileName, string url = null, bool isEnc = true)
    {
        try
        {
            string fullPath = url == null ? ConfigFilePath + "/" + fileName : url + fileName;
            WebAssetRequest request = MainManager.Instance.GetManager<IAssetsManager>().GetRequest<WebAssetRequest,TextAsset>(fullPath);
            await UniTask.WaitUntil(() => request.isDone);
            string dataStr = request.text;
            if (isEnc)
            {
                dataStr = AESTool.AesDecrypt(dataStr);
            }
            return JsonMapper.ToObject<T>(dataStr);
        }
        catch (System.Exception e)
        {
            LWDebug.LogWarning($"{ConfigFilePath}:路径中没有 {fileName}文件");
            LWDebug.LogWarning(e.Message);
            return default;
        }
    }
}
