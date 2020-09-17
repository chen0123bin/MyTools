using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LWFramework.Message;
using System;
using LWFramework.Core;

public class MainCameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MainManager.Instance.GetManager<GlobalMessageManager>().AddListener(CommonMessageType.Common_CameraMove, MainCameraMove);
	}

    private void MainCameraMove(Message msg)
    {
        Vector3 posi = msg.Get<Vector3>("Position");
        Vector3 rotation = msg.Get<Vector3>("Rotation");
        transform.DOMove(posi, 0.5f);
        transform.DORotate(rotation, 0.5f);
    }

    
}
