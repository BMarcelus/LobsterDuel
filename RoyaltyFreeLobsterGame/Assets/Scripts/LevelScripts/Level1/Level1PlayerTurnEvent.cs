﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1PlayerTurnEvent : PlayerTurnEvents {
	public GameObject tutorialPage;
	public Text tutorialText;
	public Deck playerDeck;
	[Header("Dialogue")]
	public BattleDialogueManager dialogueManager;
	public DialogueSequence pincherDialogue;

	void Start()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Drag A Lobster to place them on the Floor";
	}
	public override IEnumerator CheckTurnEvent(int turn)
	{
		switch(turn)
		{
			case 2:
				yield return Turn2Event();
				break;
			case 6:
				yield return Turn6Event();
				break;
			case 7:
				//shuffle the deck
				playerDeck.shuffle = true;
				break;
			default:
				break;
		}
	}

	public IEnumerator Turn2Event()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Summon this card and attack with them";
		yield return new WaitForEndOfFrame();
	}
	public IEnumerator Turn3Event()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Summon this card";
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Turn6Event()
	{
		GetComponent<TurnManager>().ChangeTurn(Turn.Enemy);
		dialogueManager.StartDialogue(pincherDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		tutorialPage.SetActive(true);
		tutorialText.text = "You can use level1 card as material to summon level 2 card";
		GetComponent<TurnManager>().ChangeTurn(Turn.Player);
	}
}
