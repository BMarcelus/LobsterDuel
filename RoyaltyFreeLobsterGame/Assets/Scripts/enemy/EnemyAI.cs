using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

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
			int targetDefense = 0;
			List<Lobster> attackable = playerFloor.GetComponent<Floor>().GetAttackableLobsters();
			//player has no lobsters, let's attack the poor player
			if(attackable.Count == 0) return EnemyMove.AttackPlayer;
			for(int index = 0; index < attackable.Count; ++index)
			{
				Lobster playerLob = attackable[index];
				int playerShell = playerLob.GetShell();
				//check if it is suitable to be the target(able to attack && has higher defense)
				if(attacker.GetClaw() >= playerShell && playerShell > targetDefense)
				{
					targetIndex = index;
					targetDefense = playerShell;
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

	
}
