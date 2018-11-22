using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

  public DialogueDisplay yourBox;
  public DialogueDisplay theirBox;
  public DialogueSequence sequence;

  private int sequenceIndex;

	// Use this for initialization
	void Start () {
		yourBox.Deactivate();
		theirBox.Deactivate();
    UpdateDisplay();
	}

  void UpdateDisplay() {
		yourBox.Deactivate();
		theirBox.Deactivate();
    if(sequenceIndex >= sequence.dialogue.Length) {
      yourBox.Deactivate();
      theirBox.Deactivate();
      gameObject.SetActive(false);
      return;
    }
    DialogueSequence.Dialogue current = sequence.dialogue[sequenceIndex];
    DialogueDisplay box = current.flipSide ? theirBox : yourBox;
    box.Show(current.character, current.text);
  }

  void Next() {
    ++sequenceIndex;
    UpdateDisplay();
  }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
      Next();
    }
	}
}
