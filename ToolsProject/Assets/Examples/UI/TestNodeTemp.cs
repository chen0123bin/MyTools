﻿using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TestNodeTemp : BaseUINode 
{
    [UIElement("Button")]
    private Button m_Btn;
    [UIElement("Text")]
    private Text m_Text;

    private Action m_OnClick;
    public Action OnClick { set => m_OnClick = value; get => m_OnClick; }
	public string Text {
		set => m_Text.text = value;
	}
	public override  void Create(GameObject gameObject)
	{
		base.Create(gameObject);
        EventTriggerListener.Get(m_Btn.gameObject).onClick= (go) => {
            OnClick?.Invoke();
        };
	}
	public override void OnUnSpawn()
	{
	}
	public override void Release()
	{
		base.Release();
	}
}
