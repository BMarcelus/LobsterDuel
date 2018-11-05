using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStats : MonoBehaviour {
  [Range(0, 3)]
  public int level;
  [Range(0, 3)]
  public int attack;
  [Range(0, 3)]
  public int defense;

  public PipsDisplay levelDisplay;
  public PipsDisplay attackDisplay;
  public PipsDisplay defenseDisplay;


	// Use this for initialization
	void OnValidate () {

		levelDisplay.setLevel(level);
		attackDisplay.setLevel(attack);
		defenseDisplay.setLevel(defense);
	}
}
