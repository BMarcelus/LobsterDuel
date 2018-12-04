using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

  public DialogueDisplay yourBox;
  public DialogueDisplay theirBox;
  public DialogueSequence sequence;

  private TopDownManager manager;

  private int sequenceIndex;

	// Use this for initialization
	void Start () {
		yourBox.Deactivate();
		theirBox.Deactivate();
    manager = GetComponentInParent<TopDownManager>();
    UpdateDisplay();
	}

  void UpdateDisplay() {
		yourBox.Deactivate();
		theirBox.Deactivate();
    if(sequenceIndex >= sequence.dialogue.Length) {
      yourBox.Deactivate();
      theirBox.Deactivate();
      manager.EndDialogue(sequence.storySequence);
      gameObject.SetActive(false);
      return;
    }
    DialogueSequence.Dialogue current = sequence.dialogue[sequenceIndex];
    DialogueSequence.Character character = sequence.GetCharacter(current.characterIndex);
    DialogueDisplay box = character.flipSide ? theirBox : yourBox;
    box.Show(current.character, current.text, character.characterSprite);
    if(character.gameObject && current.target) {
      movement mv = character.gameObject.GetComponent<movement>();
      if(mv) {
        mv.SetTarget(current.target, false);
      }
    }
  }

  void Next() {
    ++sequenceIndex;
    UpdateDisplay();
  }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")) {
      Next();
    }
	}

  public void SetSequence(DialogueSequence seq) {
    sequence = seq;
    sequenceIndex = 0;
    UpdateDisplay();
  }
}
