using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {
	private List<GameObject> cardsInHand = new List<GameObject>();
	public float cardInterval;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		TestCardDraging();
	}

	//========================================================================
	// Temporary for Test
	//========================================================================

	//========================================================================
	// Hand Cards Manipulate
	//========================================================================
	private void ResetCardPositions()
	{
		//calculate the area of hand
		float width = cardsInHand.Count * cardInterval;
		float startX = - width/2 + cardInterval/2;
		//reset cards position
		for(int index  = 0; index < cardsInHand.Count; ++index)
		{
			cardsInHand[index].transform.localPosition = new Vector3(startX + index * cardInterval , 0, -0.1f * index);
		}
	}
	public void AddCardToHand(GameObject card)
	{
		GameObject newCard = GameObject.Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
		newCard.transform.parent = transform;
		cardsInHand.Add(newCard);
		ResetCardPositions();
	}

	public void RemoveCardFromhand(int index)	
	{
		GameObject cardToDestroy = cardsInHand[index];
		cardsInHand.RemoveAt(index);
		Destroy(cardToDestroy);
		ResetCardPositions();
	}

	//========================================================================
	// Move Cards
	//========================================================================
	private bool dragingCard = false;
	private int dragingCardIndex;

	public void TestCardDraging()
	{
		TestChoseCard();
		TestDragCard();
		TestReleaseCard();
	}
	private void TestChoseCard()
	{
		if(Input.GetMouseButtonDown(0))
		{
			
		}
	}

	private void TestDragCard()
	{

	}

	private void TestReleaseCard()
	{

	}

}
