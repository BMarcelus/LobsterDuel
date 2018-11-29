using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackToStart : MonoBehaviour {

  private Vector3 startPosition;
  private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
    startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
    Vector2 diff = startPosition - transform.position;
    if(diff.sqrMagnitude>0.5) {
      diff.Normalize();
      rb.velocity += (diff*10 - rb.velocity)/10;
    }
	}
}
