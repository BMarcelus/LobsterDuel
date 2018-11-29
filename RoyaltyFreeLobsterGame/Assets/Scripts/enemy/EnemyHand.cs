using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour {
	private List<GameObject> cardsInHand = new List<GameObject>();
	public float cardInterval;
	public GameObject card;
	public Deck deck;
  	public AudioSource cardPlaceSound;
	public bool canPlaceCard = true;
	public GameObject manager;
	[Header("differ between levels")]
	public int initialCardNumbers;

	void Start()
	{
		StartCoroutine(DrawMultipleCards(initialCardNumbers));
	}

	public void ResetForNewTurn()
	{
		canPlaceCard = true;
		AddCardToHand();
	}

	//========================================================================
	// Hand Cards Manipulate
	//========================================================================
	private IEnumerator DrawMultipleCards(int num)
	{
		yield return new WaitForSeconds(0.01f);
		for(int x = 0; x < num; ++x)
		{
			AddCardToHand();
			yield return new WaitForSeconds(0.2f);
		}
	}
	public void ResetCardPositions()
	{
		//calculate the area of hand
		float width = cardsInHand.Count * cardInterval;
		float startX = - width/2 + cardInterval/2;
		//reset cards position
		for(int index  = 0; index < cardsInHand.Count; ++index)
		{
			cardsInHand[index].transform.localPosition = new Vector3(startX + index * cardInterval , 0, -0.1f * index);
			//flip the cards back
			cardsInHand[index].transform.eulerAngles = new Vector3(0 , 180, 0);
		}
	}
	public void AddCardToHand()
	{
		GameObject newCard = GameObject.Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
		//don't flip enemy's card, players can see it
		newCard.GetComponent<Animator>().Play("idle");
		newCard.transform.parent = transform;
		newCard.transform.localScale = SystemManager.cardNormalScale;
		CardData newCardData = deck.DrawACard();
		if(newCardData == null)
		{
			GameObject.FindGameObjectWithTag("Enemy").GetComponent<Player>().LoseGame();
		}
		newCard.GetComponent<Lobster>().SetData(newCardData);
		cardsInHand.Add(newCard);
		ResetCardPositions();
	}

	public void RemoveCardFromhand(GameObject card)	
	{
		cardsInHand.Remove(card);
		ResetCardPositions();
	}

	public void RemoveCardFromhand(int index)	
	{
		cardsInHand.RemoveAt(index);
		ResetCardPositions();
	}

	public void PlaceCard(GameObject card, GameObject spot)
	{
		if(cardsInHand.Contains(card))
		{
			spot.GetComponent<FloorSpot>().SetCard(card);
			cardsInHand.Remove(card);	
			canPlaceCard = false;
			cardPlaceSound.Play();
			ResetCardPositions();
		}
	}

	public List<GameObject> GetCardList()
	{
		return cardsInHand;
	}

}
