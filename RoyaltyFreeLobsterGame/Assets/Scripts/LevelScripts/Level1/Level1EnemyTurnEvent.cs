using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1EnemyTurnEvent : EnemyTurnEvents {
	public TurnManager turnManager;
	public GameObject tutorialPage;
	public EnemyHand enemyHand;
	public Floor enemyFloor;
	public BattleManager battleManager;
	public GameObject dialogue;

	void Start()
	{
		dialogue.SetActive(false);
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

}
