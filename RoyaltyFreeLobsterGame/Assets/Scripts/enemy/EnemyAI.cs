using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	//========================================================================
	//Make movements for cards
	//========================================================================
	public List<Lobster> GetOrder(GameObject[] spots)
	{
		List<Lobster> result = new List<Lobster>(3);
		//add all enemy's lobsters to the order list
		foreach(GameObject spot in spots)
		{
			GameObject enemyCard = spot.GetComponent<FloorSpot>().GetCardInPlay();
			if(enemyCard != null && enemyCard.GetComponent<Lobster>() != null)
				result.Add(enemyCard.GetComponent<Lobster>());
		}
		//switch orders
		//have 0/1 item, no need to switch
		//have two lobsters
		if(result.Count == 2)
		{
			CheckSwitch(ref result,0,1);
		}else if (result.Count == 3)
		{
			CheckSwitch(ref result,0,1);
			CheckSwitch(ref result,1,2);
			CheckSwitch(ref result,0,1);
		}
		return result;
	}

	private void CheckSwitch(ref List<Lobster> list, int smallerIndex, int largerIndex)
	{
		//if lobster at higher order has higher attack, move it up
		if(list[smallerIndex].GetClaw() < list[largerIndex].GetClaw())
		{
			Lobster temp = list[smallerIndex];
			list[smallerIndex] = list[largerIndex];
			list[largerIndex] = temp;
		}
	}

	//get attacker enemy and player's floor, return which player lobster enemy should attack
	public EnemyMove GetTarget(Lobster attacker, GameObject playerFloor)
	{
		if(attacker.GetClaw() == 0)
			return EnemyMove.Defend;
		else//attack is not 0, check if there are anything it can attack
		{
			int targetIndex = -1;
			int targetAttack = 0;
			int targetLevel = 0;
			List<Lobster> attackable = playerFloor.GetComponent<Floor>().GetAttackableLobsters();
			//player has no lobsters, let's attack the poor player
			if(attackable.Count == 0) return EnemyMove.AttackPlayer;
			for(int index = 0; index < attackable.Count; ++index)
			{
				Lobster playerLob = attackable[index];
				int playerClaw = playerLob.GetClaw();
				int playerShell = playerLob.GetShell();
				int playerLevel = playerLob.GetLevel();
				//check if able to attack
				if(attacker.GetClaw() >= playerLob.GetShell())
				{
					//beat lobster with highest level
					if(playerLevel > targetLevel)
					{
						targetIndex = index;
						targetLevel = playerLevel;
					//with the same level, beat the one with highest attack
					}else if(playerLevel == targetLevel && playerClaw > targetAttack){
						targetIndex = index;
						targetAttack = playerClaw;
					//if can only attack 1 level card, attack rock first
					}else if(playerLevel == 1 && playerLob.data.cardName == "Rock")
					{
						targetAttack = index;
					}
				}
			}
			//if has a target to attack
			if(targetIndex >= 0)
				return (EnemyMove)System.Array.IndexOf(playerFloor.GetComponent<Floor>().spots, attackable[targetIndex].floorAssigned);
			else//have nothing to attack
			{
				if(attacker.GetClaw() >=2) 
					return EnemyMove.Idle;
				else 
					return EnemyMove.Defend;
			}
		}
	}
	//========================================================================
	//Place cards from hand
	//========================================================================
	
	//get instruction for how to put card
	public PlaceCardInfo GetCardPlaceInfo(List<GameObject> hand, GameObject[] spots)
	{
		PlaceCardInfo levelThreeCard = CheckPlaceLevelThreeCard(hand, spots);
		if(levelThreeCard != null) return levelThreeCard;
		PlaceCardInfo levelTwoCard = CheckPlaceLevelTwoCard(hand, spots);
		if(levelTwoCard != null) return levelTwoCard;
		return CheckPlaceLevelOneCard(hand, spots);
	}
	
	//check if able to place a level-3 card at the board, return null if unable to
	public PlaceCardInfo CheckPlaceLevelThreeCard(List<GameObject> hand, GameObject[] spots)
	{
		//get all level 3 cards
		List<GameObject> cards = new List<GameObject>();
		foreach(GameObject card in hand)
		{
			if(card.GetComponent<CardStats>().cardData.level == 3)
				cards.Add(card);
		}
		//if don't have level3 card
		if(cards.Count == 0) return null;
		//if have level 2 card as material
		List<int> possibleSpots = new List<int>();
		for(int x = 0; x< spots.Length; ++x)
		{
			//have one level card as material
			if(spots[x].GetComponent<FloorSpot>().GetCardInPlay() && 
			   spots[x].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<CardStats>().cardData.level == 2)
				possibleSpots.Add(x);
		}
		//return if have level2 card material
		if(possibleSpots.Count > 0)
			return new PlaceCardInfo(cards[Random.Range(0, cards.Count)], possibleSpots[Random.Range(0, possibleSpots.Count)]);
		//if have 2 level 1 cards
		//get all level 1 card
		for(int x = 0; x< spots.Length; ++x)
		{
			//have one level card as material
			if(spots[x].GetComponent<FloorSpot>().GetCardInPlay() && 
			   spots[x].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<CardStats>().cardData.level == 1)
				possibleSpots.Add(x);
		}
		//if have enough seat
		if(possibleSpots.Count >= 2)
		{
			int targetSpotIndex = Random.Range(0, possibleSpots.Count);
			spots[targetSpotIndex].GetComponent<FloorSpot>().SetCard(null);
			possibleSpots.RemoveAt(targetSpotIndex);
			spots[Random.Range(0, possibleSpots.Count)].GetComponent<FloorSpot>().SetCard(null);
			return new PlaceCardInfo(cards[Random.Range(0, cards.Count)], targetSpotIndex);
		}else
			return null;
	
	}
	//check if able to place a level-2 card at the board, return null if unable to
	public PlaceCardInfo CheckPlaceLevelTwoCard(List<GameObject> hand, GameObject[] spots)
	{
		//list of all possible spots
		List<int> possibleSpots = new List<int>();
		for(int x = 0; x< spots.Length; ++x)
		{
			//have one level card as material
			if(spots[x].GetComponent<FloorSpot>().GetCardInPlay() && 
			   spots[x].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<CardStats>().cardData.level == 1)
				possibleSpots.Add(x);
		}
		if(possibleSpots.Count == 0) return null;
		//get all level 2 cards
		List<GameObject> cards = new List<GameObject>();
		foreach(GameObject card in hand)
		{
			if(card.GetComponent<CardStats>().cardData.level == 2)
				cards.Add(card);
		}
		//return if possible to place 1 level card at a random spot
		if(cards.Count > 0)
			return new PlaceCardInfo(cards[Random.Range(0, cards.Count)], possibleSpots[Random.Range(0, possibleSpots.Count)]);
		else
			return null;
	}	

	//check if able to place a level-1 card at the board, return null if unable to
	public PlaceCardInfo CheckPlaceLevelOneCard(List<GameObject> hand, GameObject[] spots)
	{
		//list of all possible spots
		List<int> emptySpots = new List<int>();
		for(int x = 0; x< spots.Length; ++x)
		{
			if(!spots[x].GetComponent<FloorSpot>().GetCardInPlay())
				emptySpots.Add(x);
		}
		if(emptySpots.Count == 0) return null;
		//get all one level cards
		List<GameObject> cards = new List<GameObject>();
		foreach(GameObject card in hand)
		{
			if(card.GetComponent<CardStats>().cardData.level == 1)
				cards.Add(card);
		}
		//return if possible to place 1 level card at a random spot
		if(cards.Count > 0)
			return new PlaceCardInfo(cards[Random.Range(0, cards.Count)], emptySpots[Random.Range(0, emptySpots.Count)]);
		else
			return null;
	}

}
