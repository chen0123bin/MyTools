using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StepTriggerMouseDown : BaseStepTrigger
{
    CancellationTokenSource cts;
    public override void TriggerBegin()
    {
        base.TriggerBegin();
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
                        m_TiggerAction?.Invoke(m_TriggerResultIndex);
                        m_IsTrigger = true;
                        break;
                    }
                }
            }
        }
          

    }
    public override void TriggerEnd()
    {
        base.TriggerEnd();
        cts.Cancel();
        cts.Dispose();
    }
}
