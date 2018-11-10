using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFloor : MonoBehaviour {
	public GameObject[] spots;
    private Camera mainCamera;
	// Use this for initialization
	void Start () {
        mainCamera = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckLobsterClick();
	}

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

    public void CheckLobsterClick()
    {
        if(Input.GetMouseButtonUp(0))
        {
            ResetLobsters();
            //if mouse is down on any spot which has lobster, open the move menu
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            GameObject spot = SpotTouched(mousePosition);
            if(spot && spot.GetComponent<FloorSpot>().GetCardInPlay()!= null)
            {
                spot.GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().OpenMoveMenu();
            }
        }
    }

    //close move menu of all lobsters
    private void ResetLobsters()
    {
        foreach(GameObject spot in spots)
        {
            if(spot.GetComponent<FloorSpot>().GetCardInPlay() != null)
                spot.GetComponent<FloorSpot>().GetCardInPlay().GetComponent<Lobster>().CloseMoveMenu();
        }
    }

}
