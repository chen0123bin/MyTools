using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_WaitTime : Trigger_Base
{
    public string WaitTime;
    private bool isTigger = false;
    private float stayTime = 0;
    private void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        TestTrigger();
        if (!isTigger )
        {
            stayTime += Time.deltaTime;
            if (stayTime > float.Parse(WaitTime))
            {
                OnNextEvent(gameObject);
                isTigger = true;
            }
        }

    }
}
