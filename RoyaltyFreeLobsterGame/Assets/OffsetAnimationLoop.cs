using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimationLoop : MonoBehaviour {

  public string animationName;
  public float offset;
	// Use this for initialization
	void Start () {
		GetComponent<Animator>().Play(animationName, -1, offset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
