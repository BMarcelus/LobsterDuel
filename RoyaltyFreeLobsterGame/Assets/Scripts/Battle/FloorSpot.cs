﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Lobster,
    Attachment
}

public class FloorSpot : MonoBehaviour {
	public GameObject card;
	private GameObject cardInPlay;
    private CardType cardType;
	private TurnManager turnManager;
	public float width;
	public float height;

    void Start()
	{
		turnManager = GameObject.FindObjectOfType<TurnManager>();
	}
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

	public CardData GetCardData()
	{
		if (cardInPlay != null)
		{
			return cardInPlay.GetComponent<CardStats>().cardData;
		}
		else
		{
			return null;
		}
	}

    public void SetCard(GameObject card)
	{
		//destroy the card here first
		if(cardInPlay != null)
		{
			Destroy(cardInPlay);
		}
		//if null is set, no need to do extra work.
		if(card == null)
		{
			cardInPlay = null;
			return;
		}else
		{
			this.cardInPlay = card;
			card.tag = "Untagged";
			//change cards transform
			card.transform.parent = null;
			card.transform.localScale = new Vector3(1.5f, 1.5f, 1);
			card.transform.parent = transform;
			card.transform.eulerAngles = Vector3.zero;
			card.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            //turn the card to a lobster
            card.GetComponent<CardInHand>().enabled = false;
            //if the card set here is a lobster
            if (card.GetComponent<Lobster>() != null)
            {
                card.GetComponent<Lobster>().enabled = true;
				card.GetComponent<Lobster>().floorAssigned = gameObject;
                cardType = CardType.Lobster;
				//set owner
				if(tag == "PlayerSpot")
				{
					card.GetComponent<Lobster>().owner = GameObject.FindGameObjectWithTag("Player");
					if(turnManager.turnNumber == 1 && card.GetComponent<Lobster>())
						card.GetComponent<Lobster>().canAttack = false;
				}else{
					card.GetComponent<Lobster>().owner = GameObject.FindGameObjectWithTag("Enemy");
				}
            }
            //if the card set here is an attachment
        }
	}

	//create a card with the data and set
	public void SetCardWithData(CardData data)
	{
		GameObject newCard = GameObject.Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
		newCard.GetComponent<Lobster>().SetData(data);
		SetCard(newCard);
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
