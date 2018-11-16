using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour {
    private Floor enemyFloor;
    private Floor playerFloor;
    public GameObject rock;
    private GameObject opponent;

    private Camera mainCamera;
    // Use this for initialization
    void Start()
    {
        playerFloor = GameObject.FindGameObjectWithTag("PlayerFloor").GetComponent<Floor>();
        enemyFloor = GameObject.FindGameObjectWithTag("EnemyFloor").GetComponent<Floor>();
        mainCamera = GameObject.FindObjectOfType<Camera>();
        opponent = GameObject.Find("opponent");
    }

    // Update is called once per frame
    void Update()
    {
        CheckLobsterClick();
        if (choosingTarget)
            CheckChoosingTarget();
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
    private List<Lobster> possibleChoice = new List<Lobster>();
    private bool choosingTarget = false;
    //when a lobster wants to attack, offer the choice of lobsters and wait
    public void PrepareToAttackEnemy(Lobster attacker)
    {
        attackerLobster = attacker;
        possibleChoice = enemyFloor.GetAttackableLobsters();
        choosingTarget = true;
        //when there are lobsters exits, choose one lobster to attack
        if(possibleChoice.Count > 0)
        {
            foreach(Lobster lob in possibleChoice)
            {
                lob.transform.Find("sprite").gameObject.SetActive(true);
            }
        }
        else //no lobster exist...
        {
            opponent.GetComponent<SpriteRenderer>().color = new Color(GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.r, 
                                                                                         GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.g,
                                                                                         GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.b, 
                                                                                         0.5f);
        }
    }

    private void CheckChoosingTarget()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Lobster attacked = null;
            choosingTarget = false;
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //when attacking lobsters
            if (possibleChoice.Count > 0)
            {
                //reset Color and get attacked lobster
                foreach (Lobster lob in possibleChoice)
                {
                    lob.transform.Find("sprite").gameObject.SetActive(false);
                    //check if touched lobster      
                    if (lob.MouseIsOn(mousePos))
                        attacked = lob;
                }
                if (attacked)
                {
                    attacked.GetHurt(attackerLobster.GetClaw());
                    attackerLobster.HideMoveButtons();
                }
            }
            else//when attacking opponent directly
            {
                //reset color
                opponent.GetComponent<SpriteRenderer>().color = new Color(GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.r,
                                                                                         GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.g,
                                                                                         GameObject.Find("opponent").GetComponent<SpriteRenderer>().color.b,
                                                                                         1f);
                //check if touched opponent
                RaycastHit2D result = Physics2D.Raycast(mousePos, Vector2.zero);
                if (result && result.collider.gameObject == opponent)
                {
                    attackerLobster.HideMoveButtons();
                    result.collider.GetComponent<Player>().GetHurt(attackerLobster.GetClaw());
                }

            }
            
        }
    }

    

}
