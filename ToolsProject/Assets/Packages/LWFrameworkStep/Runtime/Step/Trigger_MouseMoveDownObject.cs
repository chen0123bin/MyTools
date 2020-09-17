using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_MouseMoveDownObject : Trigger_Base
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
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 posi = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 20));
            Vector3 dir = posi - transform.position;
            Ray ray = new Ray(Camera.main.transform.position, dir);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
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
