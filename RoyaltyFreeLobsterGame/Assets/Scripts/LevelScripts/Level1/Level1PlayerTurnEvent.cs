using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1PlayerTurnEvent : PlayerTurnEvents {
	public GameObject tutorialPage;
	public Text tutorialText;
	public Deck playerDeck;
	[Header("Dialogue")]
	public BattleDialogueManager dialogueManager;
	public DialogueSequence pincherDialogue;
	public DialogueSequence guardsAttackingDialogue;

	void Start()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Each turn you can place a Lobster on the Floor";
	}
	public override IEnumerator CheckTurnEvent(int turn)
	{
		switch(turn)
		{
			case 2:
				yield return Turn2Event();
				break;
      		case 3:
				yield return Turn3Event();
				break;
			case 4:
				yield return Turn4Event();
				break;
			case 6:
				yield return Turn6Event();
				break;
			case 7:
				//shuffle the deck
				playerDeck.shuffle = true;
				break;
			default:
				break;
		}
	}

	public IEnumerator Turn2Event()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Click on Lobsters to give Attack command";
		yield return new WaitForEndOfFrame();
	}
	public IEnumerator Turn3Event()
	{
		dialogueManager.StartDialogue(guardsAttackingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());

		tutorialPage.SetActive(true);
		tutorialText.text = "Lobsters drop a rock when they die\nPlayers drop a rock when they get hurted";
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Turn4Event()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Lobsters protect their Owner\nDefending Lobsters protect other Lobsters";
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Turn6Event()
	{
		GetComponent<TurnManager>().ChangeTurn(Turn.Enemy);
		dialogueManager.StartDialogue(pincherDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		tutorialPage.SetActive(true);
		tutorialText.text = "You can use level1 card as material to summon level 2 card";
		GetComponent<TurnManager>().ChangeTurn(Turn.Player);
	}
}
