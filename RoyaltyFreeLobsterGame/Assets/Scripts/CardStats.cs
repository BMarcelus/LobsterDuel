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
  public AudioSource moveMenuSound;
  public Image cardBack;


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
    if(cardData.moveMenuSelect) {
      moveMenuSound.clip = cardData.moveMenuSelect;
    }
    moveMenuSound.pitch = 1.2f - 0.3f * (cardData.level-1)/2;
    if(cardData.cardBackColor.a!=0) {
      cardBack.color = cardData.cardBackColor;
    }
  }

	// Use this for initialization
	void OnValidate () {
    if (cardData)
      UpdateDisplay();
	}
}
