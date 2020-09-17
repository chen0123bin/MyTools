using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepControl_Base : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void StepStart() {

    }
    public virtual void StepEnd() {
        StepModel.FinishScript();
        Destroy(this);
    }
    public virtual void StepBack()
    {
        StepModel.BackScript();
        Destroy(this);
    }
    public virtual void InitEnd(object xmlData)
    {
        Debug.Log("StepControl_Base_Init" + xmlData);
    }
}
