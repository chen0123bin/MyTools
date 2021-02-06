using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUILoad
{
    GameObject LoadUIGameObject(string path);
    UniTask<GameObject> LoadUIGameObjectAsync(string path);
  
    Sprite GetSprite(string path);
}
