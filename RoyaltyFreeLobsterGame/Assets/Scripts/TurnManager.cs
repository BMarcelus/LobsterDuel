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

    public void PlayerTurnReset()
    {
        //allow players to place card again
        playerHand.ResetForNewTurn();
        //allow players' lobsters to act again
        playerFloor.GetComponent<Floor>().ResetSpotsForNewTurn();
    }

}
