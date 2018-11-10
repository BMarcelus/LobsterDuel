using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpot : MonoBehaviour {
	private GameObject cardInPlay;
	public float width;
	public float height;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public void SetCard(GameObject card)
	{
		if(this.cardInPlay == null)
		{
			this.cardInPlay = card;
			//temporary
			card.tag = "Untagged";
			card.transform.localScale = new Vector3(1,1,1);
			card.transform.parent = transform;
			card.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
            //turn the card to a lobster
            card.GetComponent<CardInHand>().enabled = false;
            card.GetComponent<Lobster>().enabled = true;
        }
	}

}
