﻿using libx;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetBuildTool", menuName = "LWFramework/AssetBuildTool", order = 0)]
public class AssetBuildTool : ScriptableObject
{
    [FoldoutGroup("打包"), Button("打包AB", ButtonSizes.Medium)]
    private static void BuildBundles()
    {
        EditorApplication.delayCall += () =>
        {
            var watch = new Stopwatch();
            watch.Start();
            BuildScript.BuildRules();
            BuildScript.BuildAssetBundles();
            watch.Stop();
            LWDebug.Log("Bundles " + watch.ElapsedMilliseconds + " ms.");
        };
    }
    [FoldoutGroup("打包"), Button("打包程序", ButtonSizes.Medium)]
    private static void BuildPlayer()
    {     
        //避免报错
        EditorApplication.delayCall += () => {
            var watch = new Stopwatch();
            watch.Start();
            BuildScript.BuildPlayer();
            watch.Stop();
            LWDebug.Log("Player " + watch.ElapsedMilliseconds + " ms.");
        };       
       
    }

    [FoldoutGroup("打包"), Button("打包Rules")]
    private static void BuildRules()
    {
        var watch = new Stopwatch();
        watch.Start();
        BuildScript.BuildRules();
        watch.Stop();
        LWDebug.Log("BuildRules " + watch.ElapsedMilliseconds + " ms.");
    }
    [FoldoutGroup("查看"), Button("查看Versions")]
    private static void ViewVersions()
    {
        var path = EditorUtility.OpenFilePanel("OpenFile", Environment.CurrentDirectory, "");
        if (string.IsNullOrEmpty(path)) return;
        BuildScript.ViewVersions(path);
    }
    [FoldoutGroup("查看"), Button("打开Bundles文件夹")]
    private static void ViewBundles()
    {
        EditorUtility.OpenWithDefaultApp(Assets.Bundles);
    }

    [FoldoutGroup("查看"), Button("打开Download文件夹")]
    private static void ViewDownload()
    {
        EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
    }

    [FoldoutGroup("查看"), Button("Temp")]
    private static void ViewTemp()
    {
        EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
    }

    [FoldoutGroup("查看"), Button("CRC")]
    private static void GetCRC()
    {
        var path = EditorUtility.OpenFilePanel("OpenFile", Environment.CurrentDirectory, "");
        if (string.IsNullOrEmpty(path)) return;

        using (var fs = File.OpenRead(path))
        {
            var crc = Utility.GetCRC32Hash(fs);
            LWDebug.Log(crc);
        }
    }

    [FoldoutGroup("查看"), Button("MD5")]
    private static void GetMD5()
    {
        var path = EditorUtility.OpenFilePanel("OpenFile", Environment.CurrentDirectory, "");
        if (string.IsNullOrEmpty(path)) return;

        using (var fs = File.OpenRead(path))
        {
            var crc = Utility.GetMD5Hash(fs);
            LWDebug.Log(crc);
        }
    }
    [FoldoutGroup("其他"), Button("Dump Assets")]
    private static void DumpAssets()
    {
        var path = EditorUtility.SaveFilePanel("DumpAssets", null, "dump", "txt");
        if (string.IsNullOrEmpty(path)) return;
        var s = Assets.DumpAssets();
        File.WriteAllText(path, s);
        EditorUtility.OpenWithDefaultApp(path);
    }
    [FoldoutGroup("其他"), Button("Take Screenshot")]
    private static void Screenshot()
    {
        var path = EditorUtility.SaveFilePanel("截屏", null, "screenshot_", "png");
        if (string.IsNullOrEmpty(path)) return;

        ScreenCapture.CaptureScreenshot(path);
    }
    [FoldoutGroup("其他"), Button("复制Bundle到StreamingAssetPath")]
    private static void CopyBundles()
    {
        BuildScript.CopyAssets();
    }
}
