using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameOverEvent : MonoBehaviour {
	public abstract IEnumerator PlayGameOverEvent(bool win);
	
}
