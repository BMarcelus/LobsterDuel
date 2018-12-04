using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

  public float speed;
  public bool canMove = true;
  private Transform targetPosition;
  private bool targetting = false;
  private Rigidbody2D rb;
  public Vector3 input;
	// Use this for initialization
	void Start () {
    input = Vector3.zero;
		rb = GetComponent<Rigidbody2D>();
	}

	
	// Update is called once per frame
	protected virtual void Update () {
    if(!canMove) {
      input = Vector3.zero;
      if(targetting) {
        input = targetPosition.position - transform.position;
        if(input.magnitude < speed*Time.deltaTime) {
          input = Vector2.zero;
          transform.rotation = Quaternion.Slerp(transform.rotation, targetPosition.rotation, 0.2f);
        }
      }
    }
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

  public void SetTarget(Transform target, bool movable) {
    canMove = movable;
    targetPosition = target;
    targetting = true;
  }

  public void SetCanMove(bool movable) {
    targetting = false;
    canMove = movable;
  }
}
