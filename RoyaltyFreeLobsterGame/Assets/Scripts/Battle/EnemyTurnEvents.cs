using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnEvents : MonoBehaviour {
	public EnemyManager enemyManager;
	public virtual IEnumerator CheckTurnEvent(int turn)
	{
		yield return new WaitForEndOfFrame();
		enemyManager.StartEnemyTurn();
	}
}
