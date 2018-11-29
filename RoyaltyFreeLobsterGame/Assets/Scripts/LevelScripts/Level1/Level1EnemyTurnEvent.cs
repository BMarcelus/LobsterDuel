﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1EnemyTurnEvent : EnemyTurnEvents {
	public TurnManager turnManager;
	public GameObject tutorialPage;
	public EnemyHand enemyHand;
	public Floor enemyFloor;
	public GameObject playerFloor;
	public BattleManager battleManager;
	public GameObject dialogue;
	public Text dialogueText;
	private GameObject[] spots;
	public EnemyAI enemyAI;
	public CardData protestCrack;


	void Start()
	{
		dialogue.SetActive(false);
		spots = enemyFloor.spots;
	}

	public override IEnumerator CheckTurnEvent(int turn)
	{
		//tutorialPage.SetActive(false);
		switch(turn)
		{
			case 1:
				yield return Turn1Event();
				break;
			case 6:
				yield return Turn6Event();
				break;
			case 7:
				yield return Turn7Event();
				break;
			default:
				enemyManager.StartEnemyTurn();
				break;
		}
	}

	public IEnumerator Turn1Event()
	{
		//draw card
		enemyHand.AddCardToHand();
		dialogue.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		dialogue.SetActive(false);
		turnManager.SwitchToPlayer();
	}

	public IEnumerator Turn6Event()
	{
		dialogue.SetActive(true);
		dialogueText.text = "Think you are going to win? YOU IDIOT";
		yield return new WaitForSeconds(1f);
		dialogue.SetActive(false);
		//fill the spot with rocks
		foreach(GameObject spot in enemyFloor.spots)
		{
			if(!spot.GetComponent<FloorSpot>().GetCardInPlay())
			{
				spot.GetComponent<FloorSpot>().SetCardWithData(battleManager.rockData);
			}
		}
		yield return new WaitForSeconds(1.5f);
		enemyManager.StartEnemyTurn();
	}

	public IEnumerator Turn7Event()
	{
		//draw card
		enemyHand.AddCardToHand();
		yield return new WaitForSeconds(0.5f);
		//place Protest Cracker and kill Pincher
		enemyHand.RemoveCardFromhand(0);
		GameObject targetSpot = null;
		//get a spot with a rock
		foreach(GameObject spot in spots)
		{
			GameObject cardHere = spot.GetComponent<FloorSpot>().GetCardInPlay();
			if(cardHere && cardHere.GetComponent<Lobster>().GetLevel() == 1)
			{
				targetSpot = spot;
				//if target is set to a rock, no need to continue looking, it's the best solution
				if(cardHere.GetComponent<CardStats>().cardData.cardName == "Rock")
				{
					break;
				}
			}

		}
		//put Protest Craker here
		targetSpot.GetComponent<FloorSpot>().SetCardWithData(protestCrack);
		yield return new WaitForSeconds(1);
		//say sth I'm giving up on you
		dialogue.SetActive(true);
		dialogueText.text = "Oh this card has special effect. Let me see what I can do.";
		yield return new WaitForSeconds(1.5f);
		dialogue.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		//destroy Pincher
		foreach(GameObject playerSpot in playerFloor.GetComponent<Floor>().spots)
		{
			GameObject cardHere = playerSpot.GetComponent<FloorSpot>().GetCardInPlay();
			if(cardHere && cardHere.GetComponent<CardStats>().cardData.cardName == "Pincher")
			{
				playerSpot.GetComponent<FloorSpot>().SetCard(null);
				break;
			}
		}
		yield return new WaitForSeconds(1f);
		//decide move orders
		List<Lobster> lobsters = enemyAI.GetOrder(spots);
		//reset all enemies
		foreach(Lobster lob in lobsters)
		{
			if(lob.GetState() == LobsterState.Defence)
			{
				lob.ResetForNewTurn();
			}
		}
		//get and make movement for each lobster
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		foreach(Lobster lob in lobsters)
		{
			EnemyMove move = enemyAI.GetTarget(lob, playerFloor);
			if(move == EnemyMove.Idle)//do nothing
			{
				continue;
			}
			else if(move == EnemyMove.Defend) //defend
			{
				lob.DefendButton();
				yield return new WaitForSeconds(1);
			}else if(move == EnemyMove.AttackPlayer) //attack player
			{
				player.GetHurt(lob.GetClaw());
				yield return new WaitForSeconds(1);
				//if player died, stop coroutine here
				if(player.GetHealth() == 0)
					StopAllCoroutines();
				//drop a stone
				yield return battleManager.PlayerAddRock();
			}
			else//attack player's lobsters
			{
				int targetIndex = (int)move;
				//why it's so long, sorry Brian
				Lobster target = playerFloor.GetComponent<Floor>().spots[targetIndex].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>();
				int healthBeforeBattle = player.GetHealth();
				battleManager.Battle(lob, target);
				yield return new WaitForSeconds(1);
				//if player died, stop coroutine here
				if(player.GetHealth() == 0)
					StopAllCoroutines();
				//if there are damage overflow, wait for players to 
				if(player.GetHealth() < healthBeforeBattle)
					yield return battleManager.PlayerAddRock();
			}	
		}
		yield return new WaitForSeconds(0.5f);
		//end the turn
		turnManager.SwitchToPlayer();
	}

}
