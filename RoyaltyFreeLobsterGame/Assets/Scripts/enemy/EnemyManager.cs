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
	public EnemyHand enemyHand;


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
		//draw card
		enemyHand.AddCardToHand();
		yield return new WaitForSeconds(0.5f);
		//place card
		PlaceCardInfo placeCardInfo = enemyAI.GetCardPlaceInfo(enemyHand.GetCardList(), spots);
		if(placeCardInfo != null)
		{
			spots[placeCardInfo.spotIndex].GetComponent<FloorSpot>().SetCard(placeCardInfo.card);
			enemyHand.RemoveCardFromhand(placeCardInfo.card);
			yield return new WaitForSeconds(0.5f);
		}
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

	//=============================================================================
	//Enemy's movement in Player's Turn
	//=============================================================================
	public void PlaceRock()
	{
		//adding rock at a random empty spot
		//get all random spot
		List<FloorSpot> emptySpots = new List<FloorSpot>();
		foreach(GameObject spot in enemyFloor.GetComponent<Floor>().spots)
		{
			if(!spot.GetComponent<FloorSpot>().GetCardInPlay())
				emptySpots.Add(spot.GetComponent<FloorSpot>());
		}
		if(emptySpots.Count > 0)
        	emptySpots[Random.Range(0, emptySpots.Count)].SetCardWithData(battleManager.rockData);
	}



}

public class PlaceCardInfo
{
	public int spotIndex;
	public GameObject card;

	public PlaceCardInfo(GameObject card, int spotIndex)
	{
		this.card = card;
		this.spotIndex = spotIndex;
	}

}
