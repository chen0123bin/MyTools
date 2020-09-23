using LWFramework.Core;
using LWFramework.Message;
using LWFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStepGraphStartup : MonoBehaviour
{
    public StepGraph m_StepGraph;
    // Start is called before the first frame update
    void Start()
    {
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        MainManager.Instance.AddManager(typeof(IHighlightingManager).ToString(), new HighlightingPlusManager());
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        m_StepGraph.JumpNode(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher("Jump0");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MainManager.Instance.GetManager<GlobalMessageManager>().Dispatcher("Jump1");
        }
    }
}
