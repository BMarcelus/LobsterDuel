using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateCredits : MonoBehaviour {

  public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.up * speed * Time.deltaTime;
	}
}
