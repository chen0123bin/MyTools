using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.FMS;
using LWFramework.UI;
using LWFramework.Core;
using LWFramework.Asset;
using System;

[FSMTypeAttribute(nameof(FSMName.Procedure), true)]
public class PlantStartProcedure : BaseFSMState
{
    public override void OnEnter(BaseFSMState lastState)
    {
        LWDebug.Log("进入流程植物");
        MainManager.Instance.GetManager<IAssetsManager>().LoadScene("Assets/@Resources/PlantGame/Scenes/PlantStart.unity", false, LoadStartScene);
    }

    
    public override void OnInit()
    {
    }

    public override void OnLeave(BaseFSMState nextState)
    {
    }

    public override void OnTermination()
    {
    }

    public override void OnUpdate()
    {
    }

    public void EnterGameScene() {
        MainManager.Instance.GetManager<IAssetsManager>().LoadScene("Assets/@Resources/PlantGame/Scenes/PlantGame.unity", false, LoadGameScene);
    }
    private void LoadStartScene()
    {
        MainManager.Instance.GetManager<IUIManager>().OpenView<StartView>();
     
    }

    private void LoadGameScene()
    {
        LWDebug.Log("aaaaa");
    }
}
 
