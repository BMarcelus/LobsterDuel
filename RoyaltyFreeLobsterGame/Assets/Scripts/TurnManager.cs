using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn{
    Player,
    Enemy
}
public class TurnManager : MonoBehaviour {
    private Turn currentTurn;
    public PlayerHand playerHand;
    public GameObject playerFloor;
    void Start()
    {

    }

    public void SwitchToEnemy()
    {
        currentTurn = Turn.Enemy;
    }

    public void SwitchToPlayer()
    {

    }

    public void PlayerTurnReset()
    {
        //allow players to place card again
        playerHand.ResetForNewTurn();
        //allow players' lobsters to act again
        playerFloor.GetComponent<Floor>().ResetSpotsForNewTurn();
    }

    public bool IsPlayerTurn()
    {
        return currentTurn == Turn.Player;
    }
}
