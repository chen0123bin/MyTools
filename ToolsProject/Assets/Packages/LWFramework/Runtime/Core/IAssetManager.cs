using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAssetManager
{
    T Load<T>(string path);
    T LoadAsync<T>(string path, Type type);
    void Unload<T>(T param) where T : Object;
}
