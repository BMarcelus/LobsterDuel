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
    [Header("using on floor")]
    public GameObject moveMenu;
    public GameObject sprite;
    private LobsterState state;

    private void Start()
    {
        state = LobsterState.Attack;
    }

    //=========================================================================
    //Battle
    //=========================================================================
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

    private IEnumerator Rotate(Vector3 angle, int times)
    {
        for(int x = 0; x<times; ++x)
        {
            sprite.transform.Rotate(angle);
            yield return new WaitForSeconds(0.01f);
        }
    }
    //=========================================================================
    //Interaction
    //=========================================================================
    public LobsterState GetState()
    {
        return state;
    }
}