using System.Collections;
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
    [Header("UI on floor")]
    public GameObject moveMenu;
    public GameObject attackButton;
    public GameObject defendButton;
    
    private LobsterState state;

    private void Awake()
    {
        state = LobsterState.Attack;
    }

    //=========================================================================
    //Battle, most functions only used for players
    //=========================================================================
    public void RestMoveButton()
    {
        attackButton.SetActive(true);
        defendButton.SetActive(true);
    }

    public void OpenMoveMenu()
    {
        moveMenu.SetActive(true);
        moveMenu.transform.localScale = new Vector3(1, 1, 1);
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

        HideMoveButtons();
        state = LobsterState.Defence;
        GetComponent<Animator>().Play("Defend", -1, 0);
    }
    public void HideMoveButtons()
    {
        attackButton.SetActive(false);
        defendButton.SetActive(false);
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
                GameObject newRock = Instantiate(FindObjectOfType<BattleManager>().rock, Vector3.zero, Quaternion.identity);
                newRock.GetComponent<Lobster>().owner = owner;
                floorAssigned.GetComponent<FloorSpot>().SetCard(newRock);
            }
            //destroy itself, hurt owner
            owner.GetComponent<Player>().GetHurt(overflow);
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
}
