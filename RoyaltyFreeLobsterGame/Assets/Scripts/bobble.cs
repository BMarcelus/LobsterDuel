using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobble : MonoBehaviour {

  private Vector3 start;
	// Use this for initialization
	void Start () {
		start = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    // if(transform.position.y <= start.y) {
    //   transform.position += Vector3.up * 0.1f;
    // } else {
    //   transform.position -= Vector3.up * 1;
    // }
    transform.position = start + Vector3.up * 0.2f * Mathf.Cos(Time.time*100);
	}
}
