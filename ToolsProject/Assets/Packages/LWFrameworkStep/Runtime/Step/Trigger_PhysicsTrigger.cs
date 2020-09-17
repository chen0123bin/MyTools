using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_PhysicsTrigger : Trigger_Base
{
    public string TriggerTime = "0.5";
    private bool isTigger = false;
    private float stayTime = 0;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!isTigger) {

    //        OnTriggerEvent(other.name);
    //        isTigger = true;
    //        Debug.Log(TriggerName);
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (!isTigger&& other.name==TriggerName)
        {
            stayTime += Time.deltaTime;
            if (stayTime > float.Parse(TriggerTime)) {
                OnNextEvent(other.gameObject);
                isTigger = true;
            }           
        }
    }
    private void Update()
    {
        TestTrigger();
    }
}
