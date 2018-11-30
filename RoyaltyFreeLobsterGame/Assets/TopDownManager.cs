using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownManager : MonoBehaviour {

  public int storySequence;
  public GameObject player;
  public GameObject dialogueController;

  public virtual void StartDialogue(DialogueSequence sequence, Transform target) {
    player.GetComponent<movement>().SetTarget(target, false);
    dialogueController.GetComponent<DialogueController>().SetSequence(sequence);
    dialogueController.SetActive(true);
    // dialogue.SetActive(true);
  }

  public virtual void EndDialogue(int sequenceIndex) {
    player.GetComponent<movement>().SetCanMove(true);
    storySequence = sequenceIndex;
  }
}
