using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Xml.Linq;
using System.Linq;

public class StepControl_TweenPath : StepControl_Base
{
    public Vector3[] path;
    private float _time;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }
    public override void StepStart() {
        base.StepStart();
        
    }
    public override void StepEnd() {
        transform.DOLocalPath(path,_time).OnComplete(Complete);
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
        List< XElement>list =  value.Elements("Position").ToList();
        path = new Vector3[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            path[i]= VectorUtil.XmlToVector3(list[i]);
        }
        _time =float.Parse( value.Attribute("Time").Value);
    }
}
