using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn{
    Player,
    Enemy
}
public class TurnManager : MonoBehaviour {
    private Turn currentTurn;
    public GameObject playerhand;
    public GameObject playerFloor;
    void Start()
    {

    }

    public void PlayerTurnReset()
    {
        //allow players to place card again

        //allow players' lobsters to act again

    }

}
