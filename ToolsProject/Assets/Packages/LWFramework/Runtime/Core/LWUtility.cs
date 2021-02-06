using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LWFramework.Core
{
    public delegate Object LoadDelegate(string path, Type type);

    public delegate string GetPlatformDelegate();

    public static class LWUtility
    {
        public static string AssetBundles { get; private set; } = "AssetBundles";
        /// <summary>
        ///Manifestλ��
        /// </summary>
        public static string AssetsManifestAsset { get; private set; } = "Assets/Manifest.asset";

        /// <summary>
        /// �ȸ�dll������
        /// </summary>
        public static string HotfixFileName { get; private set; }= "hotfix.dll.byte";

        public static LoadDelegate loadDelegate = null;
        public static GetPlatformDelegate getPlatformDelegate = null;
        /// <summary>
        /// ��Ŀ��Ŀ¼
        /// </summary>
        public static string ProjectRoot { get; private set; } = Application.dataPath.Replace("/Assets", "");
        /// <summary>
        /// Library
        /// </summary>
        public static string Library { get; private set; } = ProjectRoot + "/Library";
        /// <summary>
        /// ��Դ·�� ����ʱһ��ΪStreamingAssets �༭���²��Է�ABģʽ��ΪSystem.Environment.CurrentDirectory����ʹ�� AssetDatabase.LoadAssetAtPathģ��AB����
        /// </summary>
        public static string dataPath { get; set; }
        /// <summary>
        /// ����·�� �༭���¶�ȡAssets/Manifest.asset�����Ϊ�ն�ȡab�е�Manifest   
        /// </summary>
        public static string downloadURL { get; set; }

        private static LWGlobalConfig _lwGlobalConfig;

        /// <summary>
        /// Resources�µ�ȫ�������ļ�
        /// </summary>
        public static LWGlobalConfig GlobalConfig { get {
                if (_lwGlobalConfig == null) {
                    //���Ȼ�ȡ�ⲿ��������
                    _lwGlobalConfig = ConfigDataTool.ReadData<LWGlobalConfig>("config");
                    if (_lwGlobalConfig == null) {
                        _lwGlobalConfig = Resources.Load<LWGlobalAsset>("LWGlobalAsset").GetLWGlobalConfig();
                    }                    
#if UNITY_EDITOR
                    if (_lwGlobalConfig == null) {
                        FileTool.CheckCreateDirectory(Application.dataPath + "/Resources");
                        var asset = ScriptableObject.CreateInstance(typeof(LWGlobalAsset));
                        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/Resources/LWGlobalAsset.asset");
                        UnityEditor.AssetDatabase.Refresh();
                    }
#endif
                }
                return _lwGlobalConfig;
            } 
        }
        /// <summary>
        /// ��ȡ��ǰƽ̨������
        /// </summary>
        /// <returns></returns>
        public static string GetPlatform()
        {
            return getPlatformDelegate != null
                ? getPlatformDelegate()
                : GetPlatformForAssetBundles(Application.platform);
        }

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXEditor:
                    return "OSX";
                default:
                    return null;
            }
        }
        /// <summary>
        /// persistentDataPath+ƽ̨���� +/
        /// </summary>
        public static string updatePath
        {
            get
            {
                return Path.Combine(Application.persistentDataPath, Path.Combine(AssetBundles, GetPlatform())) +
                       Path.DirectorySeparatorChar;
            }
        }
//        /// <summary>
//        /// persistentDataPath  persistentData+ƽ̨����+path
//        /// </summary>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        public static string GetRelativePath4Update(string path)
//        {
//            return updatePath + path;
//        }
//        /// <summary>
//        /// ��ȡ�ļ����ص�ַ   ��������ַ+ƽ̨+filename
//        /// </summary>
//        /// <param name="filename"></param>
//        /// <returns></returns>
//        public static string GetDownloadURL(string filename)
//        {
//            return Path.Combine(Path.Combine(LWUtility.downloadURL, GetPlatform()), filename);
//        }
//        /// <summary>
//        /// ��ȡ����ƽ̨����Դ·��������ʱһ��ΪStreamingAssets+AssetBundles+ƽ̨+�ļ���
//        /// </summary>
//        /// <param name="filename"></param>
//        /// <returns></returns>
//        public static string GetWebUrlFromDataPath(string filename)
//        {
//            var path = Path.Combine(dataPath, Path.Combine(AssetBundles, GetPlatform())) + Path.DirectorySeparatorChar + filename;
//#if UNITY_IOS || UNITY_EDITOR
//            path = "file://" + path;
//#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
//            path = "file:///" + path;
//#endif
//            return path;
//        }

//        public static string GetWebUrlFromStreamingAssets(string filename)
//        {
//            var path = updatePath + filename;
//            if (!File.Exists(path))
//            {
//                path = Application.streamingAssetsPath + "/" + filename;
//            }
//#if UNITY_IOS || UNITY_EDITOR
//			path = "file://" + path;
//#elif UNITY_STANDALONE_WIN
//            path = "file:///" + path;
//#endif
//            return path;
//        }
    }
}