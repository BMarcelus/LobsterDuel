using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

  public bool oneTime;
  public DialogueSequence dialogue;
  public TopDownManager manager;
  public Transform target;
  
  void OnTriggerEnter2D(Collider2D col) {
    if(col.name!="Craig")return;
    manager.StartDialogue(dialogue, target);
    if(oneTime) {
      gameObject.SetActive(false);
    }
  }
}
