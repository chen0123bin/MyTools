using LWFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoadAB : IUILoad
{
    public Sprite GetSprite(string path)
    {
        var asset = MainManager.Instance.GetManager<IAssetsManager>().Load<UnityEngine.Sprite>(path);
        return (Sprite)asset;
    }

    public GameObject LoadUIGameObject(string path)
    {
        GameObject asset = MainManager.Instance.GetManager<IAssetsManager>().Load<GameObject>(path);
        GameObject uiGameObject = (GameObject)GameObject.Instantiate(asset);
        return uiGameObject;
    }

    public GameObject LoadUIGameObjectAsync(string path)
    {
        throw new System.NotImplementedException();
    }
}
