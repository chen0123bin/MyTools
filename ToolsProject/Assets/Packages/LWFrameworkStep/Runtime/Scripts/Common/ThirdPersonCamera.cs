using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    public float distToTarget = 4f;
    public float heightToTarget = 3f;
    public Vector3 centerOffset;
    public float smoothTime = 2f;
    private Quaternion defaultRot = Quaternion.identity;
    float y;
    public float rotOffset;
	// Use this for initialization
	void Start () {
        y = target.rotation.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        y = target.rotation.eulerAngles.y;
        y += rotOffset;
        Quaternion targetRot = Quaternion.Euler(0f, y, 0f);
        defaultRot = Quaternion.Lerp(defaultRot, targetRot, smoothTime * Time.deltaTime);
        transform.position = target.transform.position + centerOffset + defaultRot * new Vector3(0f, heightToTarget, -distToTarget);
        transform.LookAt(target.transform.position + centerOffset);
    }
}
