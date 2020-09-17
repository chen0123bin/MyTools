using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManamger : MonoBehaviour {
    public Text timeText;
    private  float timeValue;
    private float fen=29;
    private float miao=60;
    public GameObject baogaoShuUI;
    // Use this for initialization
    void Start () {
        //设置时间（10分钟）
        if (StaticValue.CurrMode == Mode.testMode) {
            timeText.gameObject.SetActive(true);
        }
    }
    public void ShowBaoGao() {
        if (StaticValue.CurrMode == Mode.testMode)
        {
            baogaoShuUI.SetActive(true);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        timeValue += Time.deltaTime;
        if (timeValue >= 1)
        {
            timeValue = 0;
            miao--;
            if (miao == 0) {
                miao = 59;
                fen--;
            }
            if (fen < 0) {
                timeText.color = Color.red;
            }
        }
        timeText.text = fen + ":" + miao;
    }
}
