using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloor : MonoBehaviour
{
    public GameObject[] spots;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //return which spot pos is in
    public GameObject SpotTouched(Vector2 pos)
    {
        for (int x = 0; x < spots.Length; ++x)
        {
            if (spots[x].GetComponent<FloorSpot>().InBound(pos))
                return spots[x];
        }
        return null;
    }

}

