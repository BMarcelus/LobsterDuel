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
    [Header("UI on floor")]
    public GameObject moveMenu;
    public GameObject attackButton;
    public GameObject switchButton;
    public GameObject sprite;
    [SerializeField]
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
        switchButton.SetActive(true);
    }

    public void OpenMoveMenu()
    {
        moveMenu.SetActive(true);
    }

    public void CloseMoveMenu()
    {
        moveMenu.SetActive(false);
    }

    public void SwitchState()
    {
        CloseMoveMenu();
        switchButton.SetActive(false);
        //change the state and rotate card
        if(state == LobsterState.Attack)
        {
            state = LobsterState.Defence;
            StartCoroutine(Rotate(new Vector3(0, 0, -9), 10));
        }
        else
        {
            state = LobsterState.Attack;
            StartCoroutine(Rotate(new Vector3(0, 0, 9), 10));
        }
    }

    public void AttackButton()
    {
        FindObjectOfType<BattleManager>().PrepareToAttackEnemy(this);
    }

    public void HideAttackButton()
    {
        attackButton.SetActive(false);
    }

    private IEnumerator Rotate(Vector3 angle, int times)
    {
        for(int x = 0; x<times; ++x)
        {
            sprite.transform.Rotate(angle);
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
        Debug.Log("Cry");
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
        //return data.attack;
        return 1;
    }
}
