using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using DG.Tweening;
using System;

public class MoveStepNode : BaseStepNode
{
    [Sirenix.OdinInspector.ShowInInspector]
    public BaseC c1;
    [Sirenix.OdinInspector.ShowInInspector]
    public Action<int> m_action;
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
[Serializable]
public class MyTestClass<T> {
    public T Some;
}

public class BaseC {
    public int id;
}
public class C1 : BaseC
{
    public string abc;
    public string aa;
}
public class C2: BaseC
{
    public Transform target;
    public Vector3 v3;

}