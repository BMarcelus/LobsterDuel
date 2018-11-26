using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1PlayerTurnEvent : PlayerTurnEvents {
	public override IEnumerator CheckTurnEvent(int turn)
	{
		switch(turn)
		{
			case 1:
			yield return Turn1Event();
				break;
		}
	}

	public IEnumerator Turn1Event()
	{
		yield return new WaitForSeconds(1);
	}
}
