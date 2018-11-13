using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {
	private List<GameObject> cardsInHand = new List<GameObject>();
	public float cardInterval;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerFloor = GameObject.FindGameObjectWithTag("PlayerFloor").GetComponent<Floor>();
	}
	
	// Update is called once per frame
	void Update () {
        TestCardDraging();
	}

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
	// Move/Selecting Cards
	//========================================================================
	private enum MoveMode
	{
		Drag,
		Click
	}
	
	private Camera mainCamera;
	private Floor playerFloor;
    private Vector2 lastMousePosition;
    private bool selectingCard = false;
    private int cardClicking = -1;
	private int selectedCardIndex = -1;
    private MoveMode moveMode = MoveMode.Drag;
    private float dragDistance;
    private float clickAllowedDistance = 0.2f;
    //private float clickTimer = 0; //how much is the time from click to release, to test if player is click on card or draging
    //public float humansClickTime;

	public void TestCardDraging()
	{
		TestChoseCard();
        //clickTimer += Time.deltaTime;
		TestDragCard();
		TestReleaseCard();
	}

	private void TestChoseCard()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lastMousePosition = mousePosition; //set lastMousePosition for dragging
			//use raycast to test if mouse is over a card
			RaycastHit result;
			if(Physics.Raycast(new Vector3(mousePosition.x, mousePosition.y, -10), new Vector3(0,0,1), out result) && result.collider.tag == "CardInHand")
			{
                int index = cardsInHand.IndexOf(result.collider.gameObject);
                SelectCard(index);
                moveMode = MoveMode.Drag;
                //clickTimer = 0;
                dragDistance = 0;
			}
		}
	}

	//draging a card in hand
	private void TestDragCard()
	{
        if (selectingCard && moveMode == MoveMode.Drag)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition != lastMousePosition)
            {
                cardsInHand[selectedCardIndex].transform.position = new Vector3(mousePosition.x, mousePosition.y, cardsInHand[selectedCardIndex].transform.position.z);
                dragDistance += Vector2.Distance(lastMousePosition, mousePosition);
                lastMousePosition = mousePosition;
                //if the card ever moves, does not consider it a click. We set timer to a large number so it cannot be considered a click
                //clickTimer = 10;
            }
        }
	}

	private void TestReleaseCard()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //if player release in 0.05s, they are selecting instead of dragging
            if (dragDistance < clickAllowedDistance && moveMode == MoveMode.Drag)//(clickTimer < humansClickTime && moveMode == MoveMode.Drag)
            {
                //if player clicked the selected card again, unselect the card
                if (cardClicking == selectedCardIndex)
                {
                    UnselectCard();
                    cardClicking = -1;
                    ResetCardPositions();
                }
                else
                {
                    moveMode = MoveMode.Click;
                    ResetCardPositions();
                    SelectCard(selectedCardIndex);
                    cardClicking = selectedCardIndex;
                }                
                return;
            }
			//check if the card is inside a floor spot
			GameObject spot = playerFloor.SpotTouched(mousePosition);
			//touch a spot, see if able to put card there
			if(spot)
			{
				if(spot.GetComponent<FloorSpot>().GetCardInPlay() == null && selectingCard)
				{
					spot.GetComponent<FloorSpot>().SetCard(cardsInHand[selectedCardIndex]);
					cardsInHand.RemoveAt(selectedCardIndex);	
				}
			}
            cardClicking = -1;
			UnselectCard();
			ResetCardPositions();
		}	
	}

	private void SelectCard(int cardIndex)
	{
		UnselectCard();
        selectedCardIndex = cardIndex;
        selectingCard = true;
        //make the selected card larger
		cardsInHand[selectedCardIndex].transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		//make the card above all others
		Vector3 temp = cardsInHand[selectedCardIndex].transform.position;
		temp.z = -9;
		cardsInHand[selectedCardIndex].transform.position = temp;
	}

	private void UnselectCard()
	{
        //is selecting somecard -> reset its size
        //the card may have just been used and is not in list
        if (selectedCardIndex != -1 && selectedCardIndex < cardsInHand.Count)
		{
			cardsInHand[selectedCardIndex].transform.localScale = new Vector3(1, 1, 1);
		}
		selectingCard = false;
		selectedCardIndex = -1;
	}

}
