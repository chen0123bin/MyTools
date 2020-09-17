using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;

public class StepControl_ChildTweenMove : StepControl_Base
{
    public string childName;
    private Transform child;
    public Vector3 _postion;
    public Vector3 _rotation;
    public Vector3 _scale;
    private float _time;
    // Use this for initialization
    void Start () {
        child = transform.Find(childName);
        if (!child) {
            Debug.LogError("没有找到这个子物体" + childName);
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
    public override void StepStart() {
        base.StepStart();
        
    }
    public override void StepEnd() {
       
        child.DOLocalMove(_postion, _time + 0.04f).OnComplete(Complete);
        child.DOLocalRotate(_rotation, _time);
        child.DOScale(_scale, _time);
    }

    private void Complete()
    {
        base.StepEnd();
    }

    private void WayPointChange(int value)
    {
        
    }

    public override void InitEnd(object xmlData) {
        
        base.InitEnd(xmlData);
        XElement value = (XElement)xmlData;
        _postion = VectorUtil.XmlToVector3(value.Element("Position"));
      
        _rotation = VectorUtil.XmlToVector3(value.Element("Rotation"));
        _scale = VectorUtil.XmlToVector3(value.Element("Scale"));
        _time =float.Parse( value.Attribute("Time").Value);
    }
}
