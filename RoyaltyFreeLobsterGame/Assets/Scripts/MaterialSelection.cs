using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelection : MonoBehaviour {
	public Text levelNeededText;
	public GameObject playerFloor;
	public PlayerHand playerHand;
	public Camera mainCamera;
	//private GameObject[] spots;
	private List<FloorSpot> selectedSpots;
	private int levelSum;
	private GameObject card;
	private GameObject targetSpot;
	private int neededLevel;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
		//spots = playerFloor.GetComponent<Floor>().spots;
	}
	
	// Update is called once per frame
	void Update () {
		CheckMaterialSelection();
		CheckSelectionFinish();
	}

	//get the level needed and start selection
	public void StartMaterialSelection(GameObject card, GameObject spot)
	{
		targetSpot = spot;
		this.card = card;
		gameObject.SetActive(true);
		levelSum = 0;
		neededLevel = card.GetComponent<CardStats>().cardData.level - 1;
		selectedSpots = new List<FloorSpot>(3);
		UpdateText();
	}

	private void CheckMaterialSelection()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2 cameraPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			GameObject spotObject = playerFloor.GetComponent<Floor>().SpotTouched(cameraPos);
			//when touched sth
			if(spotObject)
			{
				FloorSpot spot = spotObject.GetComponent<FloorSpot>();
				//see if player is selecting a new spot or unselected one
				if(selectedSpots.Contains(spot))
				{
					UnSelectSpot(spot);
				}else{
					SelectSpot(spot);
				}
			}
		}
	}

	//get enough materials, destroy them and put card
	private void CheckSelectionFinish()
	{
		if(levelSum >= neededLevel)
		{
			//destroy materials
			foreach(FloorSpot spot in selectedSpots)
			{
				spot.SetCard(null);
			}
			//put new card
			playerHand.PlaceCard(card, targetSpot);
			gameObject.SetActive(false);
		}
	}

	public void GiveUpLevelingUp()
	{
		//unselected all spots
		foreach(FloorSpot spot in selectedSpots)
			UnSelectSpot(spot);
		gameObject.SetActive(false);
	}
	private void SelectSpot(FloorSpot spot)
	{
		//only lobsters who have not attacked can be used as material
		if(spot.GetCardInPlay() && spot.GetCardInPlay().GetComponent<Lobster>() && spot.GetCardInPlay().GetComponent<Lobster>().canAttack)
		{
			selectedSpots.Add(spot);
			SetGlobalScale(spot.GetCardInPlay(), SystemManager.cardSelectedScale);
			levelSum += spot.GetCardData().level;
			UpdateText();
		}

	}

	private void UnSelectSpot(FloorSpot spot)
	{
		selectedSpots.Remove(spot);
		SetGlobalScale(spot.GetCardInPlay(), SystemManager.cardNormalScale);
		levelSum -= spot.GetCardData().level;
		UpdateText();
	}

	private void UpdateText()
	{
		levelNeededText.text = "Still need materials of " + (neededLevel - levelSum) +" levels";
	}

	private void SetGlobalScale(GameObject ob, Vector3 scale)
	{
			Transform parentTrans = ob.transform.parent;
			ob.transform.SetParent(null);
			ob.transform.localScale = scale;
			ob.transform.SetParent(parentTrans);
	}

}
