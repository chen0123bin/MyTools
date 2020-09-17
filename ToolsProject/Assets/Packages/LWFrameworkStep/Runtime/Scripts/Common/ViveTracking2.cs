using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViveTracking2 : MonoBehaviour {
    public Transform tracking;
    public Vector3 offset;
    // Update is called once per frame
    void Update () {
        transform.position = tracking.position + offset;
        transform.eulerAngles = tracking.transform.eulerAngles;
    }

}
