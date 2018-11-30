using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDefenseBlockDisplay : MonoBehaviour {

  private SpriteRenderer[] spriteRenderers;
  private Canvas[] canvases;
  private GameObject defenseBlockDisplay;
  private bool defending = false;
	// Use this for initialization
	void Start () {
    spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    canvases = GetComponentsInChildren<Canvas>();
    if(transform.parent && transform.parent.name == "playerHand") {
      defenseBlockDisplay = GameObject.Find("DefenseBlockDisplayPlayer");
    } else {
      defenseBlockDisplay = GameObject.Find("DefenseBlockDisplayEnemy");
    }
	}
	
	public void OnDefenseStart () {
    return;
		if(defending) return;
    defending = true;
    if(defenseBlockDisplay) {
      defenseBlockDisplay.GetComponent<Animator>().Play("DefenseBlock", -1, 0);
    } else {
      Debug.Log("HELP");
    }
    foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
      spriteRenderer.sortingOrder += 20;
    }
    foreach(Canvas canvas in canvases) {
      canvas.sortingOrder += 20;
    }
	}

  public void OnDefenseEnd() {
    return;
		if(!defending) return;
    defending = false;
    foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
      spriteRenderer.sortingOrder -= 20;
    }
    foreach(Canvas canvas in canvases) {
      canvas.sortingOrder -= 20;
    }
  }
}
