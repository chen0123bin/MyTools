using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using LWFramework.Message;
using LWFramework.Core;

public class ChapterManager : MonoBehaviour {
    public string dataName;
    //当前章节的脚标数据
    public int _currentChapterIndex = -1;
    private int chapterMax;

    private ChapterData _chapterData;
    private StepManager _stepManager;
    private bool chapterIsFinish = false;
    // Use this for initialization

    private void Awake()
    {
        StepModel.StepControlCount = 0;
        ObjectManager.Instance.LoadXML(dataName);
    }
    void Start () {
        

        _chapterData = new ChapterData(dataName);
        _stepManager = gameObject.AddComponent<StepManager>();      
        foreach (var item in _chapterData.GetXElementData())
        {
            Debug.Log(item.Attribute("Remark").Value);
          //  LogicUIManager.GetInstance().AddChapterRemark(item.Attribute("Remark").Value, chapterMax+1);
            chapterMax++;
        }
        
        //StaticValue.CurrMode = Mode.virtualMode;
        //AgainChapter();
    }
	
	// Update is called once per frame
	void Update () {
        if (_stepManager.AllStepIsFinish()  && !chapterIsFinish && StaticValue.CurrMode!= Mode.waitMode) {
            if (_stepManager.isNext)
            {
                //完成所有步骤
                if (_currentChapterIndex == chapterMax - 1)
                {
                    //开启鼠标操作模式
                    StaticValue.CurrMode = Mode.waitMode;
                    MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(MessagePool.GetMessage(CommonMessageType.GL_SetAllFinish));
                    Debug.Log("结束");
                }
                else
                {
                    NextChapter();
                }
            }
            else {
                LastChapter();
            }
            

        }
	}
    public void AgainChapter() {
        _currentChapterIndex--;
        NextChapter();
    }
    //切换至下一章节
    void NextChapter() {
        //关闭鼠标操作模式
        Debug.Log("开始一个章节");
        _currentChapterIndex++;
        if (_currentChapterIndex == chapterMax) {
            Debug.Log("所有章节结束了");
            chapterIsFinish = true;
            
            return;
        }
        DispatcherHelp();
        XElement chapterXElement = GetCurretnXElement();       
        //给步骤管理器设置当前章节的数据
        _stepManager.SetStepsXml(chapterXElement);
    }
    //切换至上一章节
    void LastChapter()
    {       
        _currentChapterIndex--;
        if (_currentChapterIndex == -1)
        {
            Debug.Log("已经是第一章节了");
            chapterIsFinish = false;
            _stepManager.isNext = true;
            return;
        }
        DispatcherHelp();
        XElement chapterXElement = GetCurretnXElement();
        //给步骤管理器设置当前章节的数据
        _stepManager.SetLastStepsXml(chapterXElement);
    }
    /// <summary>
    /// 获取当前步骤xml数据
    /// </summary>
    /// <returns></returns>
    XElement GetCurretnXElement()
    {
        return _chapterData.GetXElementData()[_currentChapterIndex];
    }
    /// <summary>
    /// 发送帮助信息
    /// </summary>
    void DispatcherHelp() {
        string remark = _chapterData.GetXElementData()[_currentChapterIndex].Attribute("Remark").Value;
        Message message = MessagePool.GetMessage(CommonMessageType.GL_SetRemarkText);
        message["RemarkText"] = remark;
        MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher(message);
    }
}
