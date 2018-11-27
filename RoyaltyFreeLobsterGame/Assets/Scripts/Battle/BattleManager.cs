using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour {
    private Floor enemyFloor;
    private Floor playerFloor;
    public GameObject materialSelectionPanel;
    public GameObject addingRockPanel;
    public GameObject lobsterCard;
    public CardData rockData;
    private GameObject player;
    private GameObject opponent;
    private Camera mainCamera;
    // Use this for initialization
    void Start()
    {
        playerFloor = GameObject.FindGameObjectWithTag("PlayerFloor").GetComponent<Floor>();
        enemyFloor = GameObject.FindGameObjectWithTag("EnemyFloor").GetComponent<Floor>();
        mainCamera = GameObject.FindObjectOfType<Camera>();
        opponent = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
        addingRockPanel.SetActive(false);
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

    //if player choose a target to attack, proceed the battle
    public void CheckChoosingTarget()
    {
        if(choosingTarget && Input.GetMouseButtonDown(0))
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
                    Battle(attackerLobster, attacked);
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
                //attacking enemy
                if (result && result.collider.gameObject == opponent)
                {
                    attackerLobster.canAttack = false;
                    result.collider.GetComponent<Player>().GetHurt(attackerLobster.GetClaw());
                    //enemy put a rock
                    //GetComponent<EnemyManager>().PlaceRock();
                }

            }
            
        }
    }

    public void Battle(Lobster attacker, Lobster attacked)
    {
        attacked.GetHurt(attacker.GetClaw());
        attacker.canAttack = false;
        if (attacker.transform.position.y<attacked.transform.position.y) {
          attacker.GetComponent<Animator>().Play("Attack", -1, 0);
          if(attacked.GetState() == LobsterState.Defence) {
            attacked.GetComponent<Animator>().Play("EnemyGetAttackedDefending", -1, 0);
          } else {
            attacked.GetComponent<Animator>().Play("EnemyGetAttacked", -1, 0);
          }
        } else {
          attacker.GetComponent<Animator>().Play("EnemyAttack", -1, 0);
          attacked.GetComponent<Animator>().Play("GetAttacked", -1, 0);
        }
    }    

    //player choose to add a rock in one spot, called after player got damage
    private bool cancelPlaceingRock = false;
    public IEnumerator PlayerAddRock()
    {
        cancelPlaceingRock = false;
        //show UI to tell users they need to add a rock
        addingRockPanel.SetActive(true);
        Vector3 mousePos;
        GameObject spot = null;
        //keep getting input until hit some spot
        while(spot == null && !cancelPlaceingRock)
        {
            yield return new WaitUntil(()=>Input.GetMouseButtonUp(0));
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spot = playerFloor.GetComponent<Floor>().SpotTouched(mousePos);
        }
        //adding rock at that spot
        if(!cancelPlaceingRock)
            spot.GetComponent<FloorSpot>().SetCardWithData(rockData);
        addingRockPanel.SetActive(false);
    }

    public void CancelPlacingRock()
    {
        cancelPlaceingRock = true;
    }

}
