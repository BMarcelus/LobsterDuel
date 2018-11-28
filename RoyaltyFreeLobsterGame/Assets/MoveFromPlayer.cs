using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromPlayer : MonoBehaviour {

  private Lobster lobster;
  private Vector3 target;
	// Use this for initialization
	void Start () {
		lobster = GetComponent<Lobster>();
    if(!lobster || lobster.data.name != "Rock") {
      Destroy(this);
      return;
    }
    target = transform.position;
    transform.position = lobster.owner.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, target, 0.1f);
	}
}
