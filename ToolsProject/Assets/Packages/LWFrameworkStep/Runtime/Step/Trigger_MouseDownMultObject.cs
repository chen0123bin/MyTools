using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger_MouseDownMultObject : Trigger_Base
{
    private List<string> childNameList;
    private int count;
    private float time;
    private bool canTrigger = false;
    private void Start()
    {
        //gameObject.layer = LayerMask.NameToLayer("StepObject");
        childNameList = TriggerName.Split('|').ToList();
    }
    // Update is called once per frame
    void Update()
    {
        TestTrigger();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 10))
            {
                for (int i = 0; i < childNameList.Count; i++)
                {
                    if (childNameList[i] == rayHit.collider.gameObject.name)
                    {

                        rayHit.collider.gameObject.GetComponent<Other_TweenPath>().StepEnd();
                        childNameList.Remove(childNameList[i]);
                        if (childNameList.Count == 0)
                        {
                            Debug.Log("全部完成");
                           // canTrigger = true;
                            OnNextEvent(gameObject);
                            // Invoke("TriggerEventDeley", 2);
                            //StartCoroutine(TriggerEventDeley());
                        }
                    }
                }
            }


        }
        
        if (canTrigger) {
            time += Time.deltaTime;
            if (time > 2) {
                OnNextEvent(gameObject);
            }
        }
    }
    //IEnumerator TriggerEventDeley() {
    //    yield return new WaitForSeconds(2);
    //    OnTriggerEvent(gameObject);
    //}
}
