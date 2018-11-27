using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardStats : MonoBehaviour {
  public CardData cardData;

  public PipsDisplay levelDisplay;
  public PipsDisplay attackDisplay;
  public PipsDisplay defenseDisplay;
  public Text nameDisplay;
  public Text descriptionDisplay;
  public Image characterDisplay;
  public bool update;


  public void UpdateDisplay() {
		levelDisplay.setLevel(cardData.level);
		attackDisplay.setLevel(cardData.attack);
		defenseDisplay.setLevel(cardData.defense);
    nameDisplay.text = cardData.name;
    descriptionDisplay.text = "";
    if(cardData.trigger != "") {
      descriptionDisplay.text = "On " + cardData.trigger + ": ";      
    }
    descriptionDisplay.text += cardData.description;
    if(cardData.sprite) {
      characterDisplay.sprite = cardData.sprite;
      characterDisplay.color = cardData.spriteColor;
    }
  }

	// Use this for initialization
	void OnValidate () {
    if (cardData)
      UpdateDisplay();
	}
}
