using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public Lobster[] GetOrder(GameObject[] spots)
	{

		return new Lobster[1]{null};
	}

	//get attacker enemy and player's floor, return which player lobster enemy should attack
	public EnemyMove GetTarget(Lobster attacker, GameObject playerFloor)
	{
		return EnemyMove.Idle;
	}

}
