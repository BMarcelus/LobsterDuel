using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardOutlineController : MonoBehaviour {

  public Color canAttackColor;
  public Color canPlaceColor;
  public Color disabledColor;
  public Color greyOutColor;

  public SpriteRenderer greyOut; 
  private Outline outline;
  private Lobster lobster;
  private PlayerHand hand;
  private bool active = true;
	// Use this for initialization
	void Start () {
		outline = GetComponentInChildren(typeof(Outline), false) as Outline;
    lobster = GetComponent<Lobster>();
    hand = GetComponentInParent(typeof(PlayerHand)) as PlayerHand;
    if(!hand && (!lobster || !lobster.floorAssigned || lobster.floorAssigned.tag != "PlayerSpot")) {
      Destroy(this);
    }
	}
	
	// Update is called once per frame
	void Update () {
		if(lobster.enabled && lobster.canAttack) {
      outline.effectColor = Color.Lerp(outline.effectColor, canAttackColor, 0.1f);
      greyOut.color = Color.Lerp(greyOut.color, disabledColor, 0.1f);
    } else if(!lobster.enabled && GetCanPlaceCard()) {
      outline.effectColor = Color.Lerp(outline.effectColor, canPlaceColor, 0.1f);
      greyOut.color = Color.Lerp(greyOut.color, disabledColor, 0.1f);
    } else {
      outline.effectColor = Color.Lerp(outline.effectColor, disabledColor, 0.5f);
      greyOut.color = Color.Lerp(greyOut.color, greyOutColor, 0.1f);
    }
	}

  private bool GetCanPlaceCard() {
    if(hand) return hand.canPlaceCard;
    return false;
  }

}
