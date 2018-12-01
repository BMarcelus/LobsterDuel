﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Data")]
public class CardData : ScriptableObject {
    [Header("Edit card data in the CSV!")]
    public string cardName;
    [Range(0, 3)]
    public int level;
    [Range(0, 4)]
    public int attack;
    [Range(0, 4)]
    public int defense;

    public string trigger;
    public string description;
    public Sprite sprite;
    public Color spriteColor;
    public AudioClip moveMenuSelect;
    public bool isRock;
    public Color cardBackColor;
}
