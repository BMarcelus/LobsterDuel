using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Turn{
    Player,
    Enemy,
    GameOver
}
public class TurnManager : MonoBehaviour {
    private Turn currentTurn;
    public PlayerHand playerHand;
    public GameObject playerFloor;
    public EnemyManager enemyManager;
    public Text turnText;
    public int turnNumber = 1;
    public EnemyTurnEvents enemyTurnEvent;
    public PlayerTurnEvents playerTurnEvent;

    void Start()
    {
        UpdateTurnUI();
    }

    public void SwitchToEnemy()
    {
        StartCoroutine(SwitchToEnemyTurn());
    }
    private IEnumerator SwitchToEnemyTurn()
    {
        if(currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
            UpdateTurnUI();
            //events in enemy's turn
            if(enemyTurnEvent)
                yield return enemyTurnEvent.CheckTurnEvent(GetComponent<TurnManager>().turnNumber);
        }
    }
    public void SwitchToPlayer()
    {
        StartCoroutine(SwitchToPlayerTurn());
    }
    public IEnumerator SwitchToPlayerTurn()
    {
        ++turnNumber;
        if(playerTurnEvent)
            yield return playerTurnEvent.CheckTurnEvent(turnNumber);
        currentTurn = Turn.Player;
        PlayerTurnReset();
        UpdateTurnUI();
    }
    public void GameOver()
    {
        currentTurn = Turn.GameOver;
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

    public bool IsGameOver()
    {
        return currentTurn == Turn.GameOver;
    }

    public void UpdateTurnUI()
    {
        turnText.text = currentTurn.ToString() + "'s Turn";
    }
}
