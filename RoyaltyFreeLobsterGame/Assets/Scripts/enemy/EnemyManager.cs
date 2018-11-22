using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMove
{
	AttackLeft = 0,
	AttackCenter = 1,
	AttackRight = 2,
	Defend,
	Idle,
	AttackPlayer
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
		//spots[1].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().DefendButton();

	}

	public void SetEnemy(CardData data, GameObject spot)
	{
		//create cards
        GameObject newCard = Instantiate(card, transform.position, Quaternion.identity);
		//give them datas
		newCard.GetComponent<Lobster>().SetData(data);
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
		//draw card?
		yield return new WaitForSeconds(0.5f);
		//get and make movement for each lobster
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
			}else if(move == EnemyMove.AttackPlayer) //attack player
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetHurt(lob.GetClaw());
				//drop a stone
				yield return battleManager.PlayerAddRock();
			}
			else//attackj player's lobsters
			{
				int targetIndex = (int)move;
				//why it's so long, sorry Brian
				Lobster target = playerFloor.GetComponent<Floor>().spots[targetIndex].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>();
				battleManager.Battle(lob, target);
			}
			yield return new WaitForSeconds(1);
		}
		//end the turn
		turnManager.SwitchToPlayer();
	}

	//=============================================================================
	//Enemy's movement in Player's Turn
	//=============================================================================
	public void PlaceRock()
	{
		//adding rock at a random spot
		GameObject spot = enemyFloor.GetComponent<Floor>().spots[Random.Range(0, 3)];
        GameObject newRock = Instantiate(battleManager.lobsterCard, Vector3.zero, Quaternion.identity);
        newRock.GetComponent<Lobster>().SetData(battleManager.rockData);
        spot.GetComponent<FloorSpot>().SetCard(newRock, enemy);
	}

}
