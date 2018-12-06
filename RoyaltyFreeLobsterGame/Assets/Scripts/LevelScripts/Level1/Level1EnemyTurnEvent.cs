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
	public PlayerHand playerHand;
	public Deck newDeck;
	[Header("Dialogue")]
	public BattleDialogueManager dialogueManager;
	public DialogueSequence guardsThreateningDialogue;
	public DialogueSequence guardsAttackingDialogue;
	public DialogueSequence pincherDefendingDialogue;
	public DialogueSequence pincherDyingDialogue;
	public DialogueSequence pincherDiedDialogue;
	public DialogueSequence royalDefenderDialogue;
	private bool pincherHasDefended = false;

  public CardData royalDefender;
  public CardData executionerData;


	void Start()
	{
		dialogue.SetActive(false);
		spots = enemyFloor.spots;
	}

	public override IEnumerator CheckTurnEvent(int turn)
	{
		
		// if(turn >= 10)
		// {
		// 	//if have any material and player has pincher, play protest craker and DESTROY pincher
		// 	GameObject materialSpot = null;
		// 	//get a spot with a rock or other material
		// 	foreach(GameObject spot in spots)
		// 	{
		// 		GameObject cardHere = spot.GetComponent<FloorSpot>().GetCardInPlay();
		// 		if(cardHere && cardHere.GetComponent<Lobster>().GetLevel() == 1)
		// 		{
		// 			materialSpot = spot;
		// 			//if target is set to a rock, no need to continue looking, it's the best solution
		// 			if(cardHere.GetComponent<CardStats>().cardData.cardName == "Rock")
		// 			{
		// 				break;
		// 			}
		// 		}
		// 	}
		// 	//get Pincher
		// 	GameObject pincherSpot = null;
		// 	foreach(GameObject playerSpot in playerFloor.GetComponent<Floor>().spots)
		// 	{
		// 		GameObject cardHere = playerSpot.GetComponent<FloorSpot>().GetCardInPlay();
		// 		if(cardHere && cardHere.GetComponent<CardStats>().cardData.cardName == "Pincher")
		// 		{
		// 			pincherSpot = playerSpot;
		// 			break;
		// 		}
		// 	}
		// 	if(materialSpot && pincherSpot)
		// 	{
		// 		yield return Turn7Event(materialSpot, pincherSpot);
		// 		yield break;
		// 	}
		// }
		
		//other cases
		switch(turn)
		{
			case 1:
				yield return Turn1Event();
				break;
			// case 2:
			// 	yield return Turn2Event();
			// 	break;
			case 4:
				yield return Turn6Event();
				break;
			case 5:
				yield return DuringPincherEvent();
				break;
      case 10:
        yield return ExecutionerKillPincher();
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
		dialogueManager.StartDialogue(guardsThreateningDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		turnManager.SwitchToPlayer();
	}

	public IEnumerator Turn2Event()
	{
		dialogueManager.StartDialogue(guardsAttackingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		enemyManager.StartEnemyTurn();
	}

	public IEnumerator Turn6Event()
	{
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

	public IEnumerator DuringPincherEvent()
	{
		dialogueManager.StartDialogue(royalDefenderDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		//fill the spot with royal defenders
		foreach(GameObject spot in enemyFloor.spots)
		{
      // GameObject active = spot.GetComponent<FloorSpot>().GetCardInPlay();
			// if(!active || active.GetComponent<Lobster>().data.level<2)
			// {
				spot.GetComponent<FloorSpot>().SetCardWithData(royalDefender);
			// }
		}
		yield return new WaitForSeconds(1.5f);
		enemyManager.StartEnemyTurn();
	}

	public IEnumerator PincherDefend()
	{
		dialogueManager.StartDialogue(pincherDefendingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
	}

  public IEnumerator ExecutionerKillPincher() {
		//fill the spot with royal defenders
    int i = 0;
		foreach(GameObject spot in enemyFloor.spots)
		{
      if(i==1) {
        spot.GetComponent<FloorSpot>().SetCardWithData(executionerData);
      } else {
        spot.GetComponent<FloorSpot>().SetCard(null);
      }
      ++i;
		}
		yield return new WaitForSeconds(1f);
		dialogueManager.StartDialogue(pincherDyingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		yield return new WaitForSeconds(0.5f);
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		enemyManager.StartEnemyMoveOrders();
  }

	public IEnumerator Turn7Event(GameObject materialSpot, GameObject pincherSpot)
	{
		//draw card
		Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		enemyHand.AddCardToHand();
		if(turnManager.IsGameOver())
			StopAllCoroutines();
		yield return new WaitForSeconds(0.5f);
		//place Protest Cracker and kill Pincher
		enemyHand.RemoveCardFromhand(0);
		//put Protest Craker here
		materialSpot.GetComponent<FloorSpot>().SetCardWithData(protestCrack);
		yield return new WaitForSeconds(1);
		dialogueManager.StartDialogue(pincherDyingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		yield return new WaitForSeconds(0.5f);
		//destroy pincher
		pincherSpot.GetComponent<FloorSpot>().SetCardWithData(battleManager.rockData);
    player.GetHurt(-2);
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
				yield return new WaitForSeconds(0.5f);
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
				{
					yield return battleManager.PlayerAddRock();
					yield return new WaitForSeconds(0.5f);
				}
			}	
		}
		yield return new WaitForSeconds(0.5f);
		//pincher died
		dialogueManager.StartDialogue(pincherDiedDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		playerHand.deck = newDeck;
		//end the turn
		turnManager.SwitchToPlayer();
	}

}
