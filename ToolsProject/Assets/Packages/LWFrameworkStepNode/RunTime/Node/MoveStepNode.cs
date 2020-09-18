using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using DG.Tweening;
using System;

public class MoveStepNode : BaseStepNode
{
    public List<MoveStepData> _dataList;
    // Use this for initialization
    protected override void Init() {
		base.Init();		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

    public override void OnEnter()
    {
        base.OnEnter();
        foreach (var item in _dataList)
        {
            item._target.position = item.enterPosi;
        }
       
    }
    public override void OnExit()
    {
        base.OnExit();
        foreach (var item in _dataList)
        {
            item._target.DOMove(item.exitPosi, item.moveExitTime);
        }
    }
}
[Serializable]
public class MoveStepData {
    public Transform _target;
    public float moveExitTime;
    public Vector3 enterPosi;
    public Vector3 exitPosi;
}