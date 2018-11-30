using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDialogueManager : TopDownManager {
  private bool hasFinished = false;
  public void StartDialogue(DialogueSequence sequence) {
	hasFinished = false;
    dialogueController.GetComponent<DialogueController>().SetSequence(sequence);
    dialogueController.SetActive(true);
  }

  public override void EndDialogue(int sequenceIndex) {
	hasFinished = true;
    storySequence = sequenceIndex;
  }

  public bool HasFinish()
  {
	  return hasFinished;
  }
}
