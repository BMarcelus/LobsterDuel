using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {
	private List<GameObject> cardsInHand = new List<GameObject>();
	public float cardInterval;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		playerFloor = GameObject.FindObjectOfType<PlayerFloor>().GetComponent<PlayerFloor>();
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
	private Camera mainCamera;
	private PlayerFloor playerFloor;
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
		//Debug.Log(mousePosition);
		if(Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			//use raycast to test if mouse is over a card
			RaycastHit result;
			if(Physics.Raycast(new Vector3(mousePosition.x, mousePosition.y, -10), new Vector3(0,0,1), out result) && result.collider.tag == "CardInHand")
			{
				dragingCard = true;
				dragingCardIndex = cardsInHand.IndexOf(result.collider.gameObject);
			}
		}
	}

	//draging a card in hand
	private void TestDragCard()
	{
		if(dragingCard)
		{
			Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			cardsInHand[dragingCardIndex].transform.position = new Vector3(mousePosition.x, mousePosition.y,cardsInHand[dragingCardIndex].transform.position.z);
		}
	}

	private void TestReleaseCard()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			dragingCard = false;
			//check if the card is inside a floor spot
			GameObject spot = playerFloor.SpotTouched(mousePosition);
			//touch a spot, see if able to put card there
			if(spot)
			{
				//Debug.Log(spot.GetComponent<FloorSpot>().GetCardInPlay());
				if(spot.GetComponent<FloorSpot>().GetCardInPlay() == null)
				{
					spot.GetComponent<FloorSpot>().SetCard(cardsInHand[dragingCardIndex]);
					cardsInHand.RemoveAt(dragingCardIndex);
				}
			}
			ResetCardPositions();
		}	
	}

}
