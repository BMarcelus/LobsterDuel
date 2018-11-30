using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour {
	private List<GameObject> cardsInHand = new List<GameObject>();
	public float cardInterval;
	public GameObject card;
	public Deck deck;
  	public AudioSource cardSelectSound;
  	public AudioSource cardPlaceSound;
	public bool canPlaceCard = true;
	public GameObject manager;
	public MaterialSelection materSelectionManager;
	[Header("differ between levels")]
	public int initialCardNumbers;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerFloor = GameObject.FindGameObjectWithTag("PlayerFloor").GetComponent<Floor>();
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
	public void AddCardToHand()
	{
		GameObject newCard = GameObject.Instantiate(card, new Vector3(0,0,0), Quaternion.identity);
		newCard.transform.parent = transform;
		CardData newCardData = deck.DrawACard();
		if(newCardData == null)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LoseGame();
		}
		newCard.GetComponent<Lobster>().SetData(newCardData);
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
				TryToPlaceCard(spot);
			}
            cardClicking = -1;
			UnselectCard();
			ResetCardPositions();
		}	
	}

	private void SelectCard(int cardIndex)
	{
		if(selectedCardIndex != cardIndex) {
			cardSelectSound.Play();
		}
		UnselectCard();
		if(cardIndex < cardsInHand.Count && cardIndex >= 0)
		{
			selectedCardIndex = cardIndex;
			selectingCard = true;
			//make the selected card larger
			cardsInHand[selectedCardIndex].transform.localScale = SystemManager.cardSelectedScale;
			//make the card above all others
			Vector3 temp = cardsInHand[selectedCardIndex].transform.position;
			temp.z = -9;
			cardsInHand[selectedCardIndex].transform.position = temp;
		}
	}

	private void UnselectCard()
	{
        //is selecting somecard -> reset its size
        //the card may have just been used and is not in list
        if (selectedCardIndex != -1 && selectedCardIndex < cardsInHand.Count)
		{
			cardsInHand[selectedCardIndex].transform.localScale = SystemManager.cardNormalScale;
		}
		selectingCard = false;
		selectedCardIndex = -1;
	}

	private void TryToPlaceCard(GameObject spot)
	{
		if(manager.GetComponent<TurnManager>().IsPlayerTurn() && canPlaceCard && selectingCard)
		{
			if(CanPlaceDirectly(cardsInHand[selectedCardIndex], spot))
			{
				PlaceCard(cardsInHand[selectedCardIndex], spot);
			}
			//for 1 level card don't open material selection panel since will cause problem
			//(level 1 need 0 material and will succeed level up)
			else if(cardsInHand[selectedCardIndex].GetComponent<CardStats>().cardData.level > 1){
				materSelectionManager.StartMaterialSelection(cardsInHand[selectedCardIndex], spot);
			}
			
		}
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

	//========================================================================
	//Card Level Up
	//========================================================================
	
	//if the card at this spot is enough to be used as material to use the new card
	private bool CanPlaceDirectly(GameObject card, GameObject spot)
	{
		GameObject cardInPlay = spot.GetComponent<FloorSpot>().GetCardInPlay();
		int cardlevel = card.GetComponent<CardStats>().cardData.level;
		//level 1 card can be placed at an empty space
		if(cardInPlay == null)
			return cardlevel == 1;
		//if there is a card in this spot which has not move, and it is enough to fit the material requirement
		else if(cardInPlay.GetComponent<Lobster>() && cardInPlay.GetComponent<Lobster>().canAttack)
			return cardInPlay.GetComponent<CardStats>().cardData.level >= cardlevel - 1;
		else return false;
	}

}
