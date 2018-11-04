using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInHand : MonoBehaviour {
	public float width;
	public float height;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool MouseOnCard()
	{
		return (Input.mousePosition.x < transform.position.x + width/2) && (Input.mousePosition.x > transform.position.x - width/2) 
		&& (Input.mousePosition.y < transform.position.y + height/2) && (Input.mousePosition.y > transform.position.y - height/2);
	}


}
