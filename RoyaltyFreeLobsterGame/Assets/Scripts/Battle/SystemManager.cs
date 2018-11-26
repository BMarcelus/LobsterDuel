using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour {
	public static Vector3 cardNormalScale = new Vector3(1.5f, 1.5f, 1);
	public static Vector3 cardSelectedScale = new Vector3(2f, 2f, 1);
	public PlayerHand playerHand;
	private BattleManager battleManager;
	private TurnManager turnManager;
	public GameObject materialSelectionPanel;

	// Use this for initialization
	void Start () {
		battleManager = GetComponent<BattleManager>();
		turnManager = GetComponent<TurnManager>();
		materialSelectionPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		//in user's turn, they can make movements of their lobsters when the selection panel is off
        if(GetComponent<TurnManager>().IsPlayerTurn() && !materialSelectionPanel.activeSelf)
        {
            battleManager.CheckLobsterClick();
            battleManager.CheckChoosingTarget();
        }
		//player put card from hand
		if(!materialSelectionPanel.activeSelf && !GetComponent<TurnManager>().IsGameOver())
        	playerHand.TestCardDraging();
	}
}
