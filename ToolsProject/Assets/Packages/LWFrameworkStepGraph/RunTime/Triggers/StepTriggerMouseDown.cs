using Cysharp.Threading.Tasks;
using LWFramework.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StepTriggerMouseDown : BaseStepTrigger
{
    [LabelText("触发对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList")]
    public string m_ObjName;
    CancellationTokenSource cts;
    public override void TriggerBegin()
    {
        base.TriggerBegin();
        MainManager.Instance.GetManager<IHighlightingManager>().AddFlashingHighlighting(StepRuntimeData.Instance.FindGameObject(m_ObjName),new Color[] { new Color(0 , 0.17f , 1,1), new Color(0, 0.96f,  0.99f, 1) });
        cts = new CancellationTokenSource();
        _ = WaitUpdate();
    }
    /// <summary>
    //使用Task处理射线点击
    /// </summary>
    async UniTaskVoid WaitUpdate() {
        while (true && !m_IsTrigger)
        {
            await UniTask.Yield(PlayerLoopTiming.Update,cancellationToken: cts.Token);
            if (Input.GetMouseButtonDown(0) )
            {
                //从摄像机发出射线的点
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits;               
                hits = Physics.RaycastAll(ray, 30);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject == StepRuntimeData.Instance.FindGameObject(m_ObjName)) {
                        CallTiggerAction();
                        break;
                    }
                }
            }
        }
          

    }
    public override void TriggerEnd()
    {
        base.TriggerEnd();
        if (cts != null) {
            cts.Cancel();
            cts.Dispose();
            MainManager.Instance.GetManager<IHighlightingManager>().RemoveHighlighting(StepRuntimeData.Instance.FindGameObject(m_ObjName));
            cts = null;
        }
       
    }
}
