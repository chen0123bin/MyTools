using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_MouseDownObject : Trigger_Base
{

    RaycastHit[] hits;
    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("StepObject");
    }
    // Update is called once per frame
    void Update()
    {
        TestTrigger();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            LayerMask mask = 1 << LayerMask.NameToLayer("StepObject");
            hits = Physics.RaycastAll(ray, 10, mask.value);

            for (int i = 0; i < hits.Length; i++)
            {
                if (TriggerName == hits[i].collider.name) {
                    OnNextEvent(hits[i].collider.gameObject);
                }
                
            }
        }


    }
}
