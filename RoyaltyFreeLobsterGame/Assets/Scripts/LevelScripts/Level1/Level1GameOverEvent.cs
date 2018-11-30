using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1GameOverEvent : GameOverEvent {
	public BattleDialogueManager dialogueManager;
	public DialogueSequence guardsRetreatDialogue; 
	public override IEnumerator PlayGameOverEvent(bool win)
	{
		if(win)
		{
			yield return new WaitForSeconds(2);
			dialogueManager.StartDialogue(guardsRetreatDialogue);
			yield return new WaitUntil(() => dialogueManager.HasFinish());
			UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
		}else{
			yield return new WaitForSeconds(2);
			UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
		}
	}
}
