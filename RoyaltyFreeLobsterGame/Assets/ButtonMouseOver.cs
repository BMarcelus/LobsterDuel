using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMouseOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  void OnMouseEnter() {
    transform.position += Vector3.up * 0.1f;
  }
  void OnMouseExit() {
    transform.position -= Vector3.up * 0.1f;
  }
}
