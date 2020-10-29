using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LWFramework {
    public interface IPoolGameObject: IPoolObject
    {
        /// <summary>
        /// 是否为Active
        /// </summary>
        bool GetActive();
        void SetActive(bool active);
        void Create(GameObject gameObject);
      
    }
}

