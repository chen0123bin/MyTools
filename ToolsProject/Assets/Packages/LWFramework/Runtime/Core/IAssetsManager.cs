using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAssetsManager
{
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="scenePath"></param>
    /// <param name="additive">是否为添加</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void LoadScene(string scenePath,bool additive,Action loadComplete=null);
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    T Load<T>(string path);
    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="type">资源类型</param>
    /// <returns></returns>
    T LoadAsync<T>(string path, Type type);
    /// <summary>
    /// UniTask异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    UniTask<T> LoadAsync<T>(string path);
    /// <summary>
    /// Res释放资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="param"></param>
    void Unload<T>(T param) where T : Object;
    /// <summary>
    /// 更新资源
    /// </summary>
    /// <param name="patchName"></param>
    void UpdatePatchAsset(string patchName);

    /// <summary>
    /// 初始化更新回调 Res模式下为空
    /// </summary>
    Action<bool> OnInitUpdateComplete {  set; }
}
