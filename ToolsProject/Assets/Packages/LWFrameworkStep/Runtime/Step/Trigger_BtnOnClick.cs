using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_BtnOnClick : Trigger_Base
{

    //private QuestionUIManager _questionUI;
    private void Start()
    {
        //_questionUI = GetComponent<QuestionUIManager>();
        // _questionUI.SetQuestion(Question, new string[] { "aaaaa", "bbbbbbbbbb", "cccccccc" }, 1, UITrigger);
       // _questionUI.SetTrigger( UITrigger);
    }
    // Update is called once per frame
    void Update()
    {

        TestTrigger();
    }
    void UITrigger() {
        Debug.Log("关闭");
        OnNextEvent(this.gameObject);
    }
    protected override void OnDestroy()
    {
       // _questionUI.Clear();
    }
   
}
