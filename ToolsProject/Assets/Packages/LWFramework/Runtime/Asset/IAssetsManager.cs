using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace LWFramework.Asset
{
    public interface IAssetsManager 
    {
        UniTask<Object> InitializeAsync();
         V Load<V,K>(string path) where K : UnityEngine. Object;

    }
}


