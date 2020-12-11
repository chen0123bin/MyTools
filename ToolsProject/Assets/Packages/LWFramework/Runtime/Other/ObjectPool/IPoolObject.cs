using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework {
    public interface IPoolObject
    {
        /// <summary>
        /// 回收进入池内
        /// </summary>
        void UnSpawn();
        /// <summary>
        /// 释放掉，完全删除
        /// </summary>
        void Release();
    }
}

