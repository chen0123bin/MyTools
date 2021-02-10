using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LWFramework.UI
{
    public interface IUIManager
    {
        /// <summary>
        /// 清空所有的View
        /// </summary>
        void ClearAllView();
        /// <summary>
        /// 关闭所有的View
        /// </summary>
        void CloseAllView();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <typeparam name="T">保留的View类型（转换成使用typeOf转换）</typeparam>
        void CloseOtherView<T>();
        /// <summary>
        /// 关闭其他的View
        /// </summary>
        /// <param name="viewName">保留的ViewName</param>
        void CloseOtherView(string viewName);
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <param name="viewName">View的名称</param>
        void CloseView(string viewName);
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void CloseView<T>();
        //BaseUIView CreateView<T>(Transform parent);
        /// <summary>
        /// 获取View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        /// <param name="viewName">View的名称选填</param>
        /// <returns></returns>
        T GetView<T>(string viewName = null);
        /// <summary>
        /// 获取所有的View
        /// </summary>
        /// <returns></returns>
        IUIView[] GetAllView();
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void OpenView<T>(bool isFirstSibling = false );
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型</typeparam>
        /// <param name="viewName">View的名称</param>
        /// <param name="uiGameObject">View的实体对象</param>
        void OpenView<T>(string viewName, GameObject uiGameObject = null,bool isFirstSibling = false);
        /// <summary>
        /// 使用异步的方式打开UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        UniTask<T> OpenViewAsync<T>(bool isFirstSibling = false);
        /// <summary>
        /// 绑定viewName跟UI路径，替换掉UIView种的特性路径，绑定的优先级更高
        /// </summary>
        /// <param name="viewName">ViewName</param>
        /// <param name="uiGameObjectPath">对象的路径</param>
        void BindView(string viewName, string uiGameObjectPath);
    }
}