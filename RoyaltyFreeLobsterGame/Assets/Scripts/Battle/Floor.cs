using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Floor : MonoBehaviour {
	public GameObject[] spots;

	//return which spot pos is in
	public GameObject SpotTouched(Vector2 pos)
	{
		for(int x = 0; x < spots.Length; ++x)
		{
			if(spots[x].GetComponent<FloorSpot>().InBound(pos))
				return spots[x];
		}
		return null;
	}

    /*if there are lobsters defending, return a list of defend lobsters,
     if there are only attacking lobsters, return a list of lobsters,
    otherwise, return null to indicate there is no lobster to attack*/
   public List<Lobster> GetAttackableLobsters()
   {
        List<Lobster> result = new List<Lobster>();
        //get all defense lobsters
        foreach(GameObject spot in spots)
        {
            FloorSpot floorSpot = spot.GetComponent<FloorSpot>();
            //when there is defending lobster in play
            if(floorSpot.GetCardInPlay()!= null && floorSpot.GetCardType() == CardType.Lobster 
                && floorSpot.GetCardInPlay().GetComponent<Lobster>().GetState() == LobsterState.Defence)
            {
                result.Add(floorSpot.GetCardInPlay().GetComponent<Lobster>());
            }
        }
        //if the list is not empty, we have defending lobsters, return them
        if (result.Count > 0) return result;
        //do not have defending lobsters, get all lobsters
        foreach (GameObject spot in spots)
        {
            FloorSpot floorSpot = spot.GetComponent<FloorSpot>();
            //when there is defending lobster in play
            if (floorSpot.GetCardInPlay() != null && floorSpot.GetCardType() == CardType.Lobster)
            {
                result.Add(floorSpot.GetCardInPlay().GetComponent<Lobster>());
            }
        }
        //return the result of all lobsters on the floor
        return result;

    }

    public void ResetSpotsForNewTurn()
    {
        foreach(GameObject spot in spots)
        {
            spot.GetComponent<FloorSpot>().ResetCardForNewTurn();
        }
    }
}
