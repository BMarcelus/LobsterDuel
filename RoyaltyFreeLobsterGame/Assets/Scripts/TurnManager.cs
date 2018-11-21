using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Turn{
    Player,
    Enemy
}
public class TurnManager : MonoBehaviour {
    private Turn currentTurn;
    public PlayerHand playerHand;
    public GameObject playerFloor;
    public Text turnText;
    void Start()
    {
        UpdateTurnUI();
    }

    public void SwitchToEnemy()
    {
        currentTurn = Turn.Enemy;
        UpdateTurnUI();
    }

    public void SwitchToPlayer()
    {
        currentTurn = Turn.Player;
        PlayerTurnReset();
        UpdateTurnUI();
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

    public void UpdateTurnUI()
    {
        turnText.text = currentTurn.ToString() + "'s Turn";
    }
}
