﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Lobster,
    Attachment
}

public class FloorSpot : MonoBehaviour {
	private GameObject cardInPlay;
    private CardType cardType;
	public float width;
	public float height;


	public bool InBound(Vector2 pos)
	{
		return (pos.x < transform.position.x + width/2) && (pos.x > transform.position.x - width/2) 
		&& (pos.y < transform.position.y + height/2) && (pos.y > transform.position.y - height/2);
	}

	public GameObject GetCardInPlay()
	{
		return cardInPlay;
	}
    
    //return what is the type of card in this spot, null there is no card here
    public CardType GetCardType()
    {
            return cardType;
    }

    public void SetCard(GameObject card)
	{
		//if null is set, no need to do extra work.
		if(card == null)
		{
			cardInPlay = null;
			return;
		}else if(this.cardInPlay == null)
		{
			this.cardInPlay = card;
			//temporary
			card.tag = "Untagged";
			card.transform.localScale = new Vector3(1.5f, 1.5f, 1);
			card.transform.parent = transform;
			card.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            //turn the card to a lobster
            card.GetComponent<CardInHand>().enabled = false;
            //if the card set here is a lobster
            if (card.GetComponent<Lobster>() != null)
            {
                card.GetComponent<Lobster>().enabled = true;
				card.GetComponent<Lobster>().floorAssigned = gameObject;
                cardType = CardType.Lobster;
            }
            //if the card set here is an attachment
        }
	}

	public void SetCard(GameObject card, GameObject owner)
	{
        card.GetComponent<Lobster>().owner = owner;
		SetCard(card);
	}

	public void ResetCardForNewTurn()
	{
		if(cardInPlay && cardInPlay.GetComponent<Lobster>())
		{
			cardInPlay.GetComponent<Lobster>().ResetForNewTurn();
		}
	}
}
