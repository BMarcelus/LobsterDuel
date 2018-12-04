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
  public Floor playerFloor;
  private bool hasPlayedReplacementText;

	void Start()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Each turn you can place one Lobster on the Floor";
    hasPlayedReplacementText = false;
	}
	public override IEnumerator CheckTurnEvent(int turn)
	{
    if(!hasPlayedReplacementText) {
      bool full = true;
      foreach(GameObject spot in playerFloor.spots)
      {
        if(!spot.GetComponent<FloorSpot>().GetCardInPlay())
        {
          full = false;
        }
      }
      if(full) {
        yield return ReplacementEvent();
        hasPlayedReplacementText = true;
      }
    }
		switch(turn)
		{
			case 2:
				yield return Turn2Event();
				break;
      		case 3:
				yield return Turn3Event();
				break;
			case 5:
				yield return Turn5Event();
				break;
			case 6:
				yield return Turn6Event();
				break;
			case 10:
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
		tutorialText.text = "Play a lobster\nclick each lobster to attack";
		yield return new WaitForEndOfFrame();
	}
	public IEnumerator Turn3Event()
	{
		dialogueManager.StartDialogue(guardsAttackingDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());

		tutorialPage.SetActive(true);
		tutorialText.text = "Lobsters drop a rock when they die\nDefending cards Must be attacked first";
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator ReplacementEvent()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "You can play a card on top of another\nto replace it";
    yield return new WaitForSeconds(0.5f);
    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
	}

	public IEnumerator Turn5Event()
	{
		tutorialPage.SetActive(true);
		tutorialText.text = "Overflow damage deals direct damage to the player";
		yield return new WaitForEndOfFrame();
	}

	public IEnumerator Turn6Event()
	{
		GetComponent<TurnManager>().ChangeTurn(Turn.Enemy);
		dialogueManager.StartDialogue(pincherDialogue);
		yield return new WaitUntil(() => dialogueManager.HasFinish());
		tutorialPage.SetActive(true);
		tutorialText.text = "You can replace level1 cards to summon level 2 cards";
		GetComponent<TurnManager>().ChangeTurn(Turn.Player);
	}
}
