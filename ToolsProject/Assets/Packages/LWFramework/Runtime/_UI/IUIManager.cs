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
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型（转换成使用typeOf转换）</typeparam>
        void OpenView<T>();
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">View的类型</typeparam>
        /// <param name="viewName">View的名称</param>
        /// <param name="uiGameObject">View的实体对象</param>
        void OpenView<T>(string viewName, GameObject uiGameObject = null);
    }
}