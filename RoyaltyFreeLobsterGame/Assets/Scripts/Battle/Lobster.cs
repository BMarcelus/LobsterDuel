﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LobsterState
{
    Attack,
    Defence
}


public class Lobster : MonoBehaviour {
    [Header("info stored")]
    public CardData data;
    public GameObject owner;
    public GameObject floorAssigned;
    public bool canAttack = true;
    [Header("UI on floor")]
    public GameObject moveMenu;
    public GameObject attackButton;
    public GameObject defendButton;
    public AudioSource deathSound;
    
    private LobsterState state;

    private void Awake()
    {
        state = LobsterState.Attack;
    }

    private void Start()
    {
        deathSound = FindObjectOfType<TurnManager>().transform.Find("DeathSound").GetComponent<AudioSource>();
    }

    public void SetData(CardData newData)
    {
        data = newData;
        GetComponent<CardStats>().cardData = newData;
        GetComponent<CardStats>().UpdateDisplay();
    }

    //=========================================================================
    //Battle, most functions only used for players
    //=========================================================================
    public void ResetForNewTurn()
    {
        if(state == LobsterState.Defence)
        {
            GetComponent<Animator>().Play("idle", -1, 0);
            state = LobsterState.Attack;
        }
        canAttack = true;
    }

    public void OpenMoveMenu()
    {
        if(canAttack)
        {
            moveMenu.SetActive(true);
            moveMenu.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void CloseMoveMenu()
    {
        if(moveMenu.activeSelf == true)
            StartCoroutine(CloseMoveMenuAnimation());
    }

    private IEnumerator CloseMoveMenuAnimation()
    {
        for(float x = 1; x>= 0;x-= 0.05f)
        {
            moveMenu.transform.localScale = new Vector3(x, x, 1);
            yield return new WaitForSeconds(0.01f);
        }
        moveMenu.SetActive(false);
    }

    public void CloseMoveMenuImmediately()
    {
        moveMenu.SetActive(false);
    }
    private void SwitchState()
    {
        //change the state and rotate card
        if(state == LobsterState.Attack)
        {
            state = LobsterState.Defence;
        }
        else
        {
            state = LobsterState.Attack;
        }
    }

    public void AttackButton()
    {
        FindObjectOfType<BattleManager>().PrepareToAttackEnemy(this);
    }
    public void DefendButton()
    {
        state = LobsterState.Defence;
        GetComponent<Animator>().Play("Defend", -1, 0);
        canAttack = false;
    }

    private IEnumerator Rotate(Vector3 angle, int times)
    {
        for(int x = 0; x<times; ++x)
        {
            //sprite.transform.Rotate(angle);
            transform.Rotate(angle);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public bool MouseIsOn(Vector2 mousePos)
    {
        RaycastHit result;
        return (Physics.Raycast(new Vector3(mousePos.x, mousePos.y, -10), new Vector3(0, 0, 1), out result) && result.collider == GetComponent<BoxCollider>());
    }
    
    public void GetHurt(int damage)
    {
        //when damage > shell, die and owner get hurt
        int overflow = damage - data.defense;
        if(overflow >= 0)
        {
            //spare place for new card
            floorAssigned.GetComponent<FloorSpot>().SetCard(null);
            //spawn a rock here if it is a lobster
            if(data.cardName != "Rock")
            {
                //create a card and assign rock data to it
                BattleManager battleManager = FindObjectOfType<BattleManager>();
                GameObject newRock = Instantiate(battleManager.lobsterCard, Vector3.zero, Quaternion.identity);
                newRock.GetComponent<Lobster>().SetData(battleManager.rockData);
                //use the rock in the floor
                floorAssigned.GetComponent<FloorSpot>().SetCard(newRock, owner);
                newRock.GetComponent<MoveFromPlayer>().Deactivate();
            }
            //destroy itself, hurt owner
            owner.GetComponent<Player>().GetHurt(overflow);
            deathSound.Play();
            Destroy(gameObject);
        }
    }
    //=========================================================================
    //Interaction
    //=========================================================================
    public LobsterState GetState()
    {
        return state;
        
    }

    public int GetClaw()
    {
        return data.attack;
    }
    public int GetShell()
    {
        return data.defense;
    }

    public int GetLevel()
    {
        return data.level;
    }

}
