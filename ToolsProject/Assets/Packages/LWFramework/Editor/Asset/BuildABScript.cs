﻿using LWFramework.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;

namespace LWFramework.Asset.Editor
{
    public static class BuildABScript
    {
        public static string overloadedDevelopmentServerURL = "";

        public static void CopyAssetBundlesTo(string outputPath)
        {
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);
            var outputFolder = GetPlatformName();
            var source = Path.Combine(Path.Combine(Environment.CurrentDirectory, LWUtility.AssetBundles), outputFolder);
            if (!Directory.Exists(source))
                Debug.Log("No assetBundle output folder, try to build the assetBundles first.");
            var destination = Path.Combine(outputPath, outputFolder);
            if (Directory.Exists(destination))
                FileUtil.DeleteFileOrDirectory(destination);
            FileUtil.CopyFileOrDirectory(source, destination);
        }

        public static string GetPlatformName()
        {
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
        }

      

        private static string[] GetLevelsFromBuildSettings()
        {
            return EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
        }

        private static string GetAssetBundleManifestFilePath()
        {
            var relativeAssetBundlesOutputPathForPlatform = Path.Combine(LWUtility.AssetBundles, GetPlatformName());
            return Path.Combine(relativeAssetBundlesOutputPathForPlatform, GetPlatformName()) + ".manifest";
        }
        //build安装包
    

        public static string CreateAssetBundleDirectory(BuildTarget buildTarget)
        {
            // Choose the output path according to the build target.
            var outputPath = Path.Combine(LWUtility.AssetBundles, GetPlatformForAssetBundles(buildTarget));
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            return outputPath;
        }

        private static Dictionary<string, string> GetVersions(AssetBundleManifest manifest)
        {
            var items = manifest.GetAllAssetBundles();          
            Dictionary<string, string> dicRet = items.ToDictionary(item => item, item => manifest.GetAssetBundleHash(item).ToString());
            /////////////////////////////////////////增加byte dll的文件///////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////增加byte dll的文件///////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////增加byte dll的文件///////////////////////////////////////////////////////////////////////////////////////////////
            dicRet.Add(LWUtility.HotfixFileName, AESTool.GetHash(Application.streamingAssetsPath + "/Hotfix/" + LWUtility.HotfixFileName));
            return dicRet;
        }

        private static void LoadVersions(string versionsTxt, IDictionary<string, string> versions)
        {
            if (versions == null)
                throw new ArgumentNullException("versions");
            if (!File.Exists(versionsTxt))
                return;
            using (var s = new StreamReader(versionsTxt))
            {
                string line;
                while ((line = s.ReadLine()) != null)
                {
                    if (line == string.Empty)
                        continue;
                    var fields = line.Split(':');
                    if (fields.Length > 1)
                        versions.Add(fields[0], fields[1]);
                }
            }
        }
        /// <summary>
        /// 保存版本文件
        /// </summary>
        /// <param name="versionsPath"></param>
        /// <param name="versions"></param>
        private static void SaveVersions(string versionsPath, Dictionary<string, string> versions)
        {
            if (File.Exists(versionsPath))
                File.Delete(versionsPath);
            using (var s = new StreamWriter(versionsPath))
            {
                foreach (var item in versions)
                    s.WriteLine(item.Key + ':' + item.Value);
                s.Flush();
                s.Close();
            }
        }

        public static void RemoveUnusedAssetBundleNames()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
        /// <summary>
        /// 对指定的文件夹进行标记
        /// </summary>
        /// <param name="ResPath">文件夹路径</param>
        /// <param name="isClear">是否为清空标记</param>
        public static void MarkAssets(string ResPath,bool isClear= false) {
            var assetsManifest = BuildABScript.GetManifest();
            //1.环境准备
            string rootPath = Path.Combine(Application.dataPath, ResPath.Substring(7)); //去掉ResPath的Assets
                                                                                        //扫描所有文件
            var allFiles = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);
            var fileList = new List<string>(allFiles);
            //剔除不打包的部分
            for (int i = fileList.Count - 1; i >= 0; i--)
            {
                var fi = allFiles[i];
                var extension = Path.GetExtension(fi.ToLower());
                //
                if (extension.ToLower() == ".meta" || extension.ToLower() == ".cs" || extension.ToLower() == ".js")
                {
                    fileList.RemoveAt(i);
                }
            }
            for (int i = 0; i < fileList.Count; i++)
            {

                //替换路径中的反斜杠为正斜杠       
                string strTempPath = fileList[i].Replace(@"\", "/");
                //截取我们需要的路径
                string path = strTempPath.Substring(strTempPath.IndexOf("Assets"));
                //根据路径加载资源
                if (Directory.Exists(path) || path.EndsWith(".cs", System.StringComparison.CurrentCulture))
                    continue;
                if (EditorUtility.DisplayCancelableProgressBar("标记资源", path, i * 1f / fileList.Count))
                    break;
                var dir = Path.GetDirectoryName(path);
                var name = Path.GetFileNameWithoutExtension(path);
                if (dir == null)
                    continue;
                dir = dir.Replace("\\", "/") + "/";
                if (name == null)
                    continue;

                var assetBundleName = dir + name;//TrimedAssetBundleName(Path.Combine(dir, name),ResPath);
                if (!isClear)
                {
                    SetAssetBundleNameAndVariant(path, assetBundleName.ToLower(), null);
                }
                else {
                    SetAssetBundleNameAndVariant(path, null, null);
                }
                
            }
            EditorUtility.SetDirty(assetsManifest);
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();

        }
        public static void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variant)
        { 
            var importer = AssetImporter.GetAtPath(assetPath);
            if(importer == null) return;
            importer.assetBundleName = bundleName;
            if(bundleName!=null)
                importer.assetBundleVariant = variant; 
        }
        //生成Manifest配置文件
        public static void BuildManifest()
        {
            //获取manifest文件
            var manifest = GetManifest();          
            AssetDatabase.RemoveUnusedAssetBundleNames();
            var bundles = AssetDatabase.GetAllAssetBundleNames();

            List<string> dirs = new List<string>();
            List<AssetRef> assets = new List<AssetRef>();  

            for (int i = 0; i < bundles.Length; i++)
            {
                var paths = AssetDatabase.GetAssetPathsFromAssetBundle(bundles[i]);
                foreach(var path in paths) 
                {
                    var dir = Path.GetDirectoryName(path);
                    dir = dir.Replace("\\", "/");
                    var index = dirs.FindIndex((o)=>o.Equals(dir));
                    if(index == -1) 
                    {
                        index = dirs.Count;
                       
                        dirs.Add(dir);
                    }  

                    var asset = new AssetRef();
                    asset.bundle = i;
                    asset.dir = index;
                    asset.name = Path.GetFileName(path);

                    assets.Add(asset);
                }
            }

            manifest.bundles = bundles;
            manifest.dirs = dirs.ToArray();
            manifest.assets = assets.ToArray();

            var assetPath = AssetDatabase.GetAssetPath(manifest);
            var bundleName = Path.GetFileNameWithoutExtension(assetPath).ToLower();
            SetAssetBundleNameAndVariant(assetPath, bundleName, null);

            EditorUtility.SetDirty(manifest);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 打包AB资源
        /// </summary>
        /// <param name="buildTarget">打包的平台</param>
        public static void BuildAssetBundles(BuildTarget buildTarget)
        {
            // Choose the output path according to the build target.
            var outputPath = CreateAssetBundleDirectory(buildTarget);

            const BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
            //获取manifest文件
            var manifest = BuildPipeline.BuildAssetBundles(outputPath, options,buildTarget);

            var versionsPath = outputPath + "/versions.txt";
            var versions = new Dictionary<string, string>();
            LoadVersions(versionsPath, versions);

            var buildVersions = GetVersions(manifest);

            var updates = new List<string>();

            foreach (var item in buildVersions)
            {
                string hash;
                var isNew = true;
                if (versions.TryGetValue(item.Key, out hash))
                    if (hash.Equals(item.Value))
                        isNew = false;
                if (isNew)
                    updates.Add(item.Key);
            }
           
            if (updates.Count > 0)
            {
                using (var s = new StreamWriter(File.Open(outputPath + "/updates.txt", FileMode.Append)))
                {
                    s.WriteLine(DateTime.Now.ToFileTime() + ":");
                    foreach (var item in updates)
                        s.WriteLine(item);
                    s.Flush();
                    s.Close();
                }

                SaveVersions(versionsPath, buildVersions);
            }
            else
            {
                Debug.Log("不需要打包");
            }
            
            string[] ignoredFiles = { GetPlatformName(), "versions.txt", "updates.txt", "manifest" };

            var files = Directory.GetFiles(outputPath, "*", SearchOption.AllDirectories);

            var deletes = (from t in files
                           let file = t.Replace('\\', '/').Replace(outputPath.Replace('\\', '/') + '/', "")
                           where !file.EndsWith(".manifest", StringComparison.Ordinal) && !Array.Exists(ignoredFiles, s => s.Equals(file))
                           where !buildVersions.ContainsKey(file)
                           select t).ToList();

            foreach (var delete in deletes)
            {
                if (!File.Exists(delete))
                    continue;
                File.Delete(delete);
                File.Delete(delete + ".manifest");
            }
            Debug.Log("AB打包完成");
            //最终将bytes 文件复制到AB文件夹
            //最终将bytes 文件复制到AB文件夹
            //最终将bytes 文件复制到AB文件夹
            //最终将bytes 文件复制到AB文件夹
            //最终将bytes 文件复制到AB文件夹
            FileTool.CopyFile(Application.streamingAssetsPath + "/Hotfix/" + LWUtility.HotfixFileName, outputPath + "/" + LWUtility.HotfixFileName, true);
            deletes.Clear();
        }
        #region   打包安装包
       
        public static void BuildStandalonePlayer()
        {
            var outputPath = EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            var levels = GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Debug.Log("Nothing to build.");
                return;
            }

            var targetName = GetBuildTargetName(EditorUserBuildSettings.activeBuildTarget);
            if (targetName == null)
                return;
#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels,
                locationPathName = outputPath + targetName,
                assetBundleManifestPath = GetAssetBundleManifestFilePath(),
                target = EditorUserBuildSettings.activeBuildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
#endif
        }
        //获取安装包名称
        private static string GetBuildTargetName(BuildTarget target)
        {
            var name = PlayerSettings.productName + "_" + PlayerSettings.bundleVersion;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (target)
            {
                case BuildTarget.Android:
                    return "/" + name + PlayerSettings.Android.bundleVersionCode + ".apk";

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "/" + name + PlayerSettings.Android.bundleVersionCode + ".exe";

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "/" + name + ".app";

#else
                    case BuildTarget.StandaloneOSXIntel:
                    case BuildTarget.StandaloneOSXIntel64:
                    case BuildTarget.StandaloneOSXUniversal:
                                        return "/" + name + ".app";

#endif

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Debug.Log("Target not implemented.");
                    return null;
            }
        }
        #endregion
        private static T GetAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                //asset = ScriptableObject.CreateInstance<T>();
                //AssetDatabase.CreateAsset(asset, path);
                //AssetDatabase.SaveAssets();
            }

            return asset;
        }

       

        public static Manifest GetManifest()
        {
            return GetAsset<Manifest>(LWUtility.AssetsManifestAsset);
        }

        public static string GetServerURL()
        {
            string downloadURL;
            if (string.IsNullOrEmpty(overloadedDevelopmentServerURL) == false)
            {
                downloadURL = overloadedDevelopmentServerURL;
            }
            else
            {
                IPHostEntry host;
                string localIP = "";
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }

                downloadURL = "http://" + localIP + ":7888/";
            }

            return downloadURL;
        }


        public static BuildTarget GetBuildTarget(ABBuildTarget aBBuildTarget)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            switch (aBBuildTarget)
            {
                case ABBuildTarget.Android:
                    buildTarget = BuildTarget.Android;
                    break;
                case ABBuildTarget.iOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case ABBuildTarget.WebGL:
                    buildTarget = BuildTarget.WebGL;
                    break;
                case ABBuildTarget.Windows:
                    buildTarget = BuildTarget.StandaloneWindows;
                    break;
                case ABBuildTarget.OSX:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
                default:
                    break;
            }
            return buildTarget;
        }
        public static ABBuildTarget GetABBuildTarget()
        {
            ABBuildTarget ab_buildTarget = ABBuildTarget.Windows;
#if UNITY_ANDROID
            ab_buildTarget = ABBuildTarget.Android;
#elif UNITY_STANDALONE_WIN
            ab_buildTarget = ABBuildTarget.Windows;
#endif

            return ab_buildTarget;
        }
        public static string GetPlatformForAssetBundles(BuildTarget target)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "OSX";
#else
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "OSX";
#endif
                default:
                    return null;
            }
        }
        public static string TrimedAssetBundleName(string assetBundleName,string ResPath)
        {
            if (string.IsNullOrEmpty(ResPath))
                return assetBundleName;
            return assetBundleName.Replace(ResPath + "/", "");
        }
        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorUtility.ClearProgressBar();
            if (LWUtility.GlobalConfig.assetMode == AssetMode.Resources)
            {
               
                LWUtility.dataPath = string.Empty;
            }
            else
            {
                LWUtility.dataPath = LWUtility.ProjectRoot;//System.Environment.CurrentDirectory;
            }
            //LWUtility.downloadURL = GetManifest().downloadURL;
           // LWUtility.assetBundleMode = settings.runtimeMode;
            LWUtility.getPlatformDelegate = GetPlatformName;
            LWUtility.loadDelegate = AssetDatabase.LoadAssetAtPath;
        }
    }
}
public enum ABBuildTarget
{
    Windows, Android, iOS, WebGL, OSX
}