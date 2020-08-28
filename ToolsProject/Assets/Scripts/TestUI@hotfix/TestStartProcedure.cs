using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LWFramework.FMS;
using LWFramework.UI;
using LWFramework.Core;
using LWFramework.Asset;
using System;

[FSMTypeAttribute(nameof(FSMName.Procedure), true)]
public class TestStartProcedure : BaseFSMState
{
    public override void OnEnter(BaseFSMState lastState)
    {
        LWDebug.Log("进入流程 状态机OnEnterOnEnterOnEnterOnEnterOnEnter");
        MainManager.Instance.GetManager<UIManager>().OpenView<TestHotfixView>();
        GameObject.Find("Canvas").AddComponent(typeof(TestHotfixMono));
       // MainManager.Instance.GetManager<AssetsManager>().LoadScene("Assets/Res/Runtime/Scenes/PolygonSamurai.unity", true, true);
        var asset =  MainManager.Instance.GetManager<AssetsManager>().Load<GameObject>("Assets/Res/Runtime/Prefabs/CubeRigidbody.prefab");
        GameObject cube =  GameObject.Instantiate(asset.asset, Vector3.zero, Quaternion.identity) as GameObject;
        PhysicsEventListener.Get(cube).onTriggerEnter = OnMonoEventAction2;
        MonoEventListener.Get(cube).onMonoEventAction = OnMonoEventAction;       
    }

    private void OnMonoEventAction2(Collider obj)
    {
        LWDebug.Log( obj);
    }

    private void OnMonoEventAction(string monoFun, UnityEngine.Object obj)
    {
        LWDebug.Log(monoFun +"  "+obj);

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

   
}
