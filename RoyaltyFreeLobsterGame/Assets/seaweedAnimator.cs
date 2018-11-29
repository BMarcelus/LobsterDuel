using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seaweedAnimator : MonoBehaviour {

  private Transform[] transforms;
  private float time;
  private float timeScalar = 1;
	// Use this for initialization
	void Start () {
		transforms = GetComponentsInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    int i = 0;
    timeScalar += (1 - timeScalar) / 50;
    time += Time.deltaTime * timeScalar;
    foreach(Transform transform in transforms) {
      float angle = Mathf.Sin(time+i) * (20+timeScalar/5);
      transform.rotation = Quaternion.Euler(0,0,angle+90);
      ++i;
    }
	}

  void OnTriggerEnter2D(Collider2D collider) {
    timeScalar = 25;
  }
}
