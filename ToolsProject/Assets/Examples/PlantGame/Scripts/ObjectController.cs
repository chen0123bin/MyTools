using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public enum ObjectType {
   //按钮，抓取物体两种类型
    button,grab
}
public class ObjectController : MonoBehaviour {
    public ObjectType objectType;
    //默认大小
    private Vector3 defaultScale;
    public float scaleValue = 1.2f;
    public UnityEvent btnEvent;
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;
    //抓取的工具类型 0-铲子 1-水壶 2-肥料
    public int grabToolType;
	// Use this for initialization
	void Start () {
        defaultScale = transform.localScale;

    }
    public void OnEnter()
    {
        transform.DOScale(defaultScale * scaleValue, 0.5f);
        if (enterEvent != null)
        {
            enterEvent.Invoke();
        }
    }
    public void OnExit() {
        transform.DOScale(defaultScale , 0.5f);
        if (exitEvent != null)
        {
            exitEvent.Invoke();
        }
    }
    public void OnClick() {
        if (btnEvent != null) {
            btnEvent.Invoke();
        }
    }
    public int OnGrab() {
        return grabToolType;
    }
}
