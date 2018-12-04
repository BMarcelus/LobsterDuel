using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSequence : MonoBehaviour {

  [System.Serializable]
  public struct Dialogue {
    public string character;
    public int characterIndex;
    
    [TextArea]
    public string text;
    public Transform target;
  }
  [System.Serializable]
  public struct Character {
    public string name;
    public Sprite characterSprite;
    public bool flipSide;
    public GameObject gameObject;
  }

  public Character[] characters;
  public Dialogue[] dialogue;
  public int storySequence;

  public Character GetCharacter(string name) {
    foreach (Character character in characters) {
      if(character.name == name) {
        return character;
      }
    }
    return new Character();
  }

  public int GetCharacterIndex(string name) {
    for(int i=0;i<characters.Length;++i) {
      if(characters[i].name == name) return i;
    }
    return -1;
  }

  public Character GetCharacter(int index) {
    return characters[index];
  }

  void OnValidate() {
    for(int i=0;i<dialogue.Length;++i) {
      dialogue[i].characterIndex = GetCharacterIndex(dialogue[i].character);
    }
  }

}
