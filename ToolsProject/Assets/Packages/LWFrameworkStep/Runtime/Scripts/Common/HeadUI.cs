using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUI : MonoBehaviour {
    public Transform head;
    public Transform headTracking;
    public float disMax=1;
    public float offsetValue;
    public float speed=1;
    public LayerMask layer;
    private Vector3 scale = new Vector3(0.1f, 0.1f, 0.01f);
    private Vector3 newPosi;
	// Use this for initialization
	void Start () {
        headTracking = transform;
        InvokeRepeating("CheckHit", 1, 0.2f);
    }
    void CheckHit() {
        RaycastHit hit;
        if (Physics.BoxCast(head.position, scale, head.forward, out hit, head.localRotation, disMax, layer))
        {

            newPosi = new Vector3(headTracking.localPosition.x, headTracking.localPosition.y, hit.distance - offsetValue);
        }
        else
        {
            newPosi = new Vector3(headTracking.localPosition.x, headTracking.localPosition.y, disMax - offsetValue);
        }
    }
	// Update is called once per frame
	void LateUpdate() {
        if (head==null)
        {
            if (Camera.main) {
                head = Camera.main.gameObject.transform;
            }
            return;
        }

        headTracking.localPosition = Vector3.Lerp(headTracking.localPosition, newPosi, speed * Time.deltaTime);

        //Ray ray = new Ray(head.position, head.forward);
        //if (Physics.Raycast(ray, out hit, disMax, layer))
        //{
        //    headTracking.localPosition = new Vector3(headTracking.localPosition.x, headTracking.localPosition.y, hit.distance- offsetValue);
        //}
        //else {
        //    headTracking.localPosition = new Vector3(headTracking.localPosition.x, headTracking.localPosition.y, disMax - offsetValue);
        //}
    }
   
}
