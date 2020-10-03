using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEditorHelp : MonoBehaviour
{
    public Vector3 posi;
    public Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posi = transform.localPosition;
        rot = transform.localEulerAngles;
    }
}
