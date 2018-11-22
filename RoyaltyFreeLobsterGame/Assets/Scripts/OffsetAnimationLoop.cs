using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetAnimationLoop : MonoBehaviour {

  public string animationName;
  private float offset;
	// Use this for initialization
	void Start () {
    offset = Random.Range(0f,1f);
		GetComponent<Animator>().Play(animationName, -1, offset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
