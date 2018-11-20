using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

  public float speed;
  private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3(
      Input.GetAxisRaw("Horizontal"),
      Input.GetAxisRaw("Vertical"),
      0
    );
    bool moving = input.x != 0 || input.y != 0;
    input.Normalize();
    rb.velocity = input*speed;
    if(moving) {
      float angle = Mathf.Atan2(input.y, input.x);
      Quaternion targetAngle = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 0.2f);
      transform.Rotate(Mathf.Sin(Time.time*30)*Vector3.forward*input.magnitude*3);
    }
	}
}
