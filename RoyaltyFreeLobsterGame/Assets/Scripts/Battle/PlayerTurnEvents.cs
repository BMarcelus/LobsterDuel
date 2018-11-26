using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnEvents : MonoBehaviour {
	public virtual IEnumerator CheckTurnEvent(int turn)
	{
		yield return new WaitForEndOfFrame();
	}
}
