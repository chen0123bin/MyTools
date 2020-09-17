using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trigger_MouseDownMultObject2 : Trigger_Base
{
    private List<string> childNameList;
    private int count;
    private float time;
    private bool canTrigger = false;
    private int triggerCount = 0;
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


                        if (i < 5)
                        {
                            //rayHit.collider.gameObject.GetComponent<Other_TweenPath>().StepEnd();
                            // childNameList.Remove(childNameList[i]);
                            Remove1(rayHit.collider.transform.parent);
                        }
                        else {
                            Remove2(rayHit.collider.transform.parent);
                        }

                        if (triggerCount >= 2)
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
    public void Remove1(Transform parent) {
        for (int i = 0; i < 5; i++)
        {
            parent.Find(childNameList[i]).GetComponent<Other_TweenPath>().StepEnd();
            //childNameList.Remove(childNameList[i]);
        }
        triggerCount++;
    }
    public void Remove2(Transform parent)
    {
        for (int i = 5; i < 8; i++)
        {
            parent.Find(childNameList[i]).GetComponent<Other_TweenPath>().StepEnd();
            //childNameList.Remove(childNameList[i]);
        }
        triggerCount++;
    }
    //IEnumerator TriggerEventDeley() {
    //    yield return new WaitForSeconds(2);
    //    OnTriggerEvent(gameObject);
    //}
}
