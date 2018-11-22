using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour {

  [System.Serializable]
  public struct Dialogue {
    public string character;
    public string text;
    public bool flipSide;
  }

  public Dialogue[] dialogue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
