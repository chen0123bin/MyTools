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
    /// ��ȡ�����ļ��ļ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">�ļ���</param>
    /// <param name="isEnc">�Ƿ����</param>
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
            LWDebug.LogWarning($"{ConfigFilePath}:·����û�� {fileName}�ļ�");
            LWDebug.LogWarning(e.Message);
            return default;
        }
    }
}
