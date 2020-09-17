using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StepQuestion {
    private GameObject gameObject;
    private XElement Element;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Exec(XElement x)
    {
        Element = x;
        gameObject = ObjectManager.Instance.GetGameObject(Element.Attribute("Name").Value);
        //问题集合
        List<XElement>questionList =  Element.Elements("Question").ToList();
        //随机选择问题
        int randomValue = Random.Range(0, questionList.Count);
        XElement question = questionList[randomValue];
        string questionValue = question.Attribute("QuestionValue").Value;
        int rightIndex = int.Parse(question.Attribute("RightIndex").Value);
        List<XElement> chooseList = question.Elements("Choose").ToList();
        string[] chooseArray = new string[chooseList.Count];
        for (int i = 0; i < chooseList.Count; i++)
        {
            chooseArray[i] = chooseList[i].Attribute("ChooseValue").Value;
        }
       // gameObject.GetComponent<QuestionUIManager>().SetQuestion(questionValue, chooseArray, rightIndex);
    }
    //当前步骤处理完成
    public static void FinishScript()
    {
        StepManager.GetInstance().Finish();
    }
}
