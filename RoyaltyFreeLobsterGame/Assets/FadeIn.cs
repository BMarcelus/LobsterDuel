using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

  public Color start;
  public Color end;
  public Text text;
  public float speed;
  public float delay;
	// Use this for initialization
	void Start () {
		text.color = start;
	}
	
	// Update is called once per frame
	void Update () {
    if(delay>0) {
      delay -= Time.deltaTime;
    } else {
      text.color = Color.Lerp(text.color, end, speed);
    }
	}
}
