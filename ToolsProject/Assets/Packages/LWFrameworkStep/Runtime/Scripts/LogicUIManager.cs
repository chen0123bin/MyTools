using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LogicUIManager : MonoBehaviour {
    private static LogicUIManager _instance;
    private ScrollViewContent scrollViewContent;
    public Text textTemp;
    public bool isInit = false;
    private List<Text> chapterTextList;
    private Text titleText;
    private Text helpText;
    private RectTransform _rightPanel;
    private GameObject hideBtn;
    private GameObject showBtn;
    private GameObject backBtn;
    public Text tipText;
    public static LogicUIManager GetInstance()
    {
        return _instance;
    }
    private void Awake()
    {
        _instance = this;
    }



    private void Init()
    {
        if (!isInit) {
            isInit = true;
            scrollViewContent = GetComponentInChildren<ScrollViewContent>();
            chapterTextList = new List<Text>();
            titleText = transform.Find("Title/Text").GetComponent<Text>();
            helpText = transform.Find("Help/Text").GetComponent<Text>();
            _rightPanel = transform.Find("Right").GetComponent<RectTransform>();
            hideBtn = transform.Find("Right/HideBtn").gameObject;
            showBtn = transform.Find("Right/ShowBtn").gameObject;
            backBtn = transform.Find("TopMenu/Layout/Back").gameObject;
            EventTriggerListener.Get(hideBtn).onClick += HideBtnOnClick;
            EventTriggerListener.Get(showBtn).onClick += ShowBtnOnClick;
            EventTriggerListener.Get(backBtn).onClick += BackBtnOnClick;
        }
    }

    private void BackBtnOnClick(GameObject go)
    {
        SceneManager.LoadScene("Menu_new");
    }

    private void HideBtnOnClick(GameObject go)
    {

        _rightPanel.DOAnchorPosX(184, 0.5f).SetEase(Ease.InOutBack);
        hideBtn.SetActive(false);
        showBtn.SetActive(true);
    }
    private void ShowBtnOnClick(GameObject go)
    {
        _rightPanel.DOAnchorPosX(-184, 0.5f).SetEase(Ease.OutBack);
        hideBtn.SetActive(true);
        showBtn.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        Init();
        //HideBtnOnClick(null);
    }
    public void AddChapterRemark(string title, int index) {
        GameObject go = GameObject.Instantiate(textTemp.gameObject);
        go.SetActive(true);
        scrollViewContent.AddChild(go);
        Text text = go.GetComponent<Text>();
        go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0);
        go.transform.localEulerAngles = Vector3.zero;
        chapterTextList.Add(text);
        text.text = index+"）"+ title;
    }
    public void SetRuning(string title,int index) {
        if (index != 0) {
            //设置已经完成
            chapterTextList[index-1].color = new Color(252,252,252);
            
        }
        if (index < chapterTextList.Count) {
            //设置进行中
            chapterTextList[index].color = new Color(71, 199, 46);
            titleText.text = title;
        }
    }
    public void SetEnd(int index) {
        if (index != 0)
        {
            chapterTextList[index - 1].color = new Color(252, 252, 252);
        }
    }
    public void SetHelpString(string str) {
        helpText.text = str;
        if (tipText) {
            tipText.text = str;
        }
    }
    private void ExitBtnOnClick(GameObject go)
    {
        Application.Quit();
    }

   
}
