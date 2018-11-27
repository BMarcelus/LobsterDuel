using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOutlineController : MonoBehaviour {

  public Color canAttackColor;
  public Color canPlaceColor;
  public Color disabledColor;
  private Outline outline;
  private Lobster lobster;
  private PlayerHand hand;
	// Use this for initialization
	void Start () {
		outline = GetComponentInChildren(typeof(Outline), false) as Outline;
    lobster = GetComponent<Lobster>();
    hand = GetComponentInParent(typeof(PlayerHand)) as PlayerHand;
    if(!hand && (lobster && lobster.floorAssigned && lobster.floorAssigned.tag != "PlayerSpot")) {
      this.enabled = false;
    }
	}
	
	// Update is called once per frame
	void Update () {
		if(lobster.enabled && lobster.canAttack) {
      outline.effectColor = Color.Lerp(outline.effectColor, canAttackColor, 0.1f);
    } else if(!lobster.enabled && GetCanPlaceCard()) {
      outline.effectColor = Color.Lerp(outline.effectColor, canPlaceColor, 0.1f);
    } else {
      outline.effectColor = Color.Lerp(outline.effectColor, disabledColor, 0.5f);
    }
	}

  private bool GetCanPlaceCard() {
    if(hand) return hand.canPlaceCard;
    return false;
  }

}
