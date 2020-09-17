using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeAddObject : MonoBehaviour {
   // public string childName;

    // Use this for initialization
    public void Add (string childName) {
        Debug.Log(childName);
        GameObject child = transform.Find(childName).gameObject;
        ObjectManager.Instance.AddModel(childName, child);
        Destroy(this);
	}

    public static void Add(string childName,GameObject gameObject) {
        ObjectManager.Instance.AddModel(childName, gameObject);
    }
	
}
