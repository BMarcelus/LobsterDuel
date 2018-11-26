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
  }

	// Use this for initialization
	void OnValidate () {
    if (cardData)
      UpdateDisplay();
	}
}
