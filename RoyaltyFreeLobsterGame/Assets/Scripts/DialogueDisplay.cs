using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour {

  public Text textDisplay;
  public Text characterName;
  public Image characterImage;
  
  public void Deactivate() {
    gameObject.SetActive(false);
  }

  public void Show(string name, string text, Sprite sprite) {
    gameObject.SetActive(true);
    textDisplay.text = text;
    characterName.text = name;
    characterImage.sprite = sprite;
  }
}
