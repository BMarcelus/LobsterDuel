using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

  private float shakeValue = 0;
  private Vector3 initialPosition;
  private Quaternion initialRotation;
  private float maxMovement = 0.2f;
  private float maxRotation = 30;

  void Start() {
    initialPosition = transform.position;
    initialRotation = transform.rotation;
  }
  public void setShakeValue(float value) {
    shakeValue = value;
  }
	
	// Update is called once per frame
	void Update () {
		shakeValue *= 0.9f;
    if(shakeValue>0.01f) {
      transform.position = initialPosition;
      transform.position += Mathf.Sin(Time.time*40) * Vector3.up * shakeValue * maxMovement;
      transform.position += Mathf.Cos(Time.time*120) * Vector3.right * shakeValue * maxMovement;
      transform.rotation = initialRotation * Quaternion.Euler(0,0, Mathf.Cos(Time.time*50) * shakeValue*maxRotation);
    }
	}
}
