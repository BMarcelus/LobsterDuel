using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRunAway : MonoBehaviour {

  private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
  void OnTriggerStay2D(Collider2D collider) {
    Vector2 diff = transform.position - collider.transform.position;
    diff.Normalize();
    rb.velocity += (diff*3-rb.velocity)/2;
  }
}
