using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStats : MonoBehaviour {
  public CardData cardData;

  public PipsDisplay levelDisplay;
  public PipsDisplay attackDisplay;
  public PipsDisplay defenseDisplay;


  // Use this for initialization
  void OnValidate () {
    if (!cardData)
      return;

    levelDisplay.setLevel(cardData.level);
    attackDisplay.setLevel(cardData.attack);
    defenseDisplay.setLevel(cardData.defense);
  }
}
