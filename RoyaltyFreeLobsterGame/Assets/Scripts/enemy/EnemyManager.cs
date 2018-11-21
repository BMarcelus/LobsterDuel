using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public GameObject enemy;
	public GameObject enemyFloor;
	public CardData[] initialCardDatas;
	private GameObject[] spots;
    public GameObject card;

    void Start () {
		spots = enemyFloor.GetComponent<Floor>().spots;
        //set up enemy's board
		SetEnemy(initialCardDatas[0], spots[0]);
		SetEnemy(initialCardDatas[1], spots[1]);
		SetEnemy(initialCardDatas[2], spots[2]);
		//set their states
		spots[1].GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().DefendButton();

	}

	public void SetEnemy(CardData data, GameObject spot)
	{
		//create cards
        GameObject newCard = Instantiate(card, transform.position, Quaternion.identity);
		//give them datas
		newCard.GetComponent<Lobster>().SetData(data);
		newCard.GetComponent<Lobster>().owner = enemy;
		//set them to correct position
        spot.GetComponent<FloorSpot>().SetCard(newCard);
	}
}
