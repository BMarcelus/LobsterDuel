using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMove
{
	AttackLeft = 0,
	AttackCenter = 1,
	AttackRight = 2,
	Defend,
	Idle
}

public class EnemyManager : MonoBehaviour {
	public GameObject enemy;
	public GameObject enemyFloor;
	public GameObject playerFloor;
	public CardData[] initialCardDatas;
    public GameObject card;
	public BattleManager battleManager;
	public TurnManager turnManager;
	public EnemyAI enemyAI;
	private GameObject[] spots;

    void Start () {
		spots = enemyFloor.GetComponent<Floor>().spots;
        //set up enemy's board
		for(int x = 0; x < initialCardDatas.Length; ++x)
		{
			if(initialCardDatas[x] != null)
				SetEnemy(initialCardDatas[x], spots[x]);
		}
		//set their states
		spots[1].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().DefendButton();

	}

	public void SetEnemy(CardData data, GameObject spot)
	{
		//create cards
        GameObject newCard = Instantiate(card, transform.position, Quaternion.identity);
		//give them datas
		newCard.GetComponent<Lobster>().SetData(data);
		newCard.GetComponent<Lobster>().owner = enemy;
		//set them to correct position
        spot.GetComponent<FloorSpot>().SetCard(newCard);
	}

	//=============================================================================
	//Enemy's turn
	//=============================================================================
	public void StartEnemyTurn()
	{
		StartCoroutine(EnemyTurn());
	}

	private IEnumerator EnemyTurn()
	{
		//draw card?
		yield return new WaitForSeconds(0.5f);
		//decide move orders
		Lobster[] lobsters = enemyAI.GetOrder(spots);
		//get and make movement for each
		foreach(Lobster lob in lobsters)
		{
			EnemyMove move = enemyAI.GetTarget(lob, playerFloor);
			if(move == EnemyMove.Idle)//do nothing
			{
				yield return new WaitForSeconds(1);
				continue;
			}
			else if(move == EnemyMove.Defend) //defend
			{
				lob.DefendButton();
			}else //attack player
			{
				int targetIndex = (int)move;
				battleManager.Battle(lob, playerFloor.GetComponent<Floor>().spots[targetIndex].GetComponent<Lobster>());
			}
			yield return new WaitForSeconds(1);
		}
		//end the turn
		turnManager.SwitchToPlayer();
	}

}
