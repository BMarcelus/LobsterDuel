using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour {
    private Floor playerFloor;
    private Floor enemyFloor;
    private Camera mainCamera;
    // Use this for initialization
    void Start () {
        playerFloor = GameObject.FindGameObjectWithTag("PlayerFloor").GetComponent<Floor>();
        enemyFloor = GameObject.FindGameObjectWithTag("EnemyFloor").GetComponent<Floor>();
        mainCamera = GameObject.FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckLobsterClick();
    }

    //=======================================================================================
    //User interaction with the spot
    //=======================================================================================
    private int lastSpotIndex = -1;
    public void CheckLobsterClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ResetLobsters();
            //if mouse is down on any spot which has lobster, open the move menu
            //if players click on the same spot, should not open the menu but reset/close it
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            GameObject spot = playerFloor.SpotTouched(mousePosition);
            if (spot && spot.GetComponent<FloorSpot>().GetCardInPlay() != null && lastSpotIndex != Array.IndexOf(playerFloor.spots, spot))
            {
                spot.GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().OpenMoveMenu();
                lastSpotIndex = Array.IndexOf(playerFloor.spots, spot);
            }
            else
            {
                lastSpotIndex = -1;
            }
        }
    }

    //close move menu of all lobsters
    private void ResetLobsters()
    {
        foreach (GameObject spot in playerFloor.spots)
        {
            if (spot.GetComponent<FloorSpot>().GetCardInPlay() != null)
                spot.GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().CloseMoveMenu();
        }
    }

    //=======================================================================================
    //Battle
    //=======================================================================================
    private Lobster attackerLobster;
    //when a lobster wants to attack, offer the choice of lobsters and wait
    public void PrepareToAttack(Lobster attacker)
    {
        attackerLobster = attacker;
        List<Lobster> possibleChoice = enemyFloor.GetAttackableLobsters();
        Debug.Log(possibleChoice.Count);
    }

}
