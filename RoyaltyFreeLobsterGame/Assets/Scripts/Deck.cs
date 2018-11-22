using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
	public DeckCardInfo[] cards;
	[SerializeField]
	private List<CardData> cardsInDeck;
	public bool shuffle;
	// Use this for initialization
	void Start () {
		cardsInDeck = new List<CardData>(10);
		foreach(DeckCardInfo cardInfo in cards)
		{
			for(int x = 0; x < cardInfo.number; ++x)
				cardsInDeck.Add(cardInfo.type);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public CardData DrawACard()
	{
		if(cardsInDeck.Count == 0)
			return null;
		int cardIndex = 0;
		//return a random card if the deck is shuffled
		if(shuffle)
			cardIndex = Random.Range(0, cardsInDeck.Count - 1);
		//return the card and remove the card from the deck
		CardData card = cardsInDeck[cardIndex];
		cardsInDeck.RemoveAt(cardIndex);
		return card;
	}

}

[System.Serializable]
public class DeckCardInfo
{
	public CardData type;
	public int number; 
}