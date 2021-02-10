using Cysharp.Threading.Tasks;
using System;
using Object = UnityEngine.Object;

public interface IAssetsManager
{
   
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
    [Obsolete]
    T LoadAsync<T>(string path, Type type);
    /// <summary>
    /// UniTask异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    UniTask<T> LoadAsync<T>(string path);

    /// <summary>
    /// 获取Request,在外部处理回调    
    /// </summary>
    /// <typeparam name="T">返回异步操作的Request对象</typeparam>
    /// <typeparam name="K">返回的资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <returns></returns>
    T GetRequest<T,K>(string path);
    /// <summary>
    /// 回调式异步加载场景
    /// </summary>
    /// <param name="scenePath"></param>
    /// <param name="additive">是否为添加</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void LoadSceneAsync(string scenePath, bool additive, Action loadComplete = null);

    /// <summary>
    /// 获取SceneRequest,在外部处理回调    
    /// </summary>
    /// <typeparam name="T">返回异步操作的Request对象</typeparam>
    /// <param name="scenePath">场景的全路径</param>
    /// <param name="additive">是否为添加</param>
    /// <returns></returns>
    T GetSceneRequest<T>(string scenePath, bool additive);
    /// <summary>
    /// 更新资源及加载场景（以场景名进行资源分包）
    /// </summary>
    /// <param name="scenePath"></param>
    /// <param name="additive">是否为添加</param>
    /// <param name="loadComplete">加载完成的回调</param>
    void UpdatePatchAndLoadSceneAsync(string scenePath, bool additive, Action loadComplete = null);
    /// <summary>
    /// 更新资源
    /// </summary>
    /// <param name="patchName"></param>
    void UpdatePatchAsset(string patchName);

    /// <summary>
    /// Res释放资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="param"></param>
    void Unload<T>(T param) where T : Object;
    /// <summary>
    /// 初始化更新回调 Res模式下为空
    /// </summary>
    Action<bool> OnInitUpdateComplete {  set; }
}
