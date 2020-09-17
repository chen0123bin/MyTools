using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepControl_Question : StepControl_Base
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void StepEnd()
    {
        StepQuestion.FinishScript();
        Destroy(this);
    }
}
