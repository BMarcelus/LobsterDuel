using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	private int health = 5;
	public Text hpText;

	public int GetHealth()
	{
		return health;
	}

    public void UpdateHealthUI()
    {
        hpText.text = health.ToString();
    }

	public void GetHurt(int damage)
	{
		health -= damage;
		if(health < 0) health = 0;
		UpdateHealthUI();
		//if(health == 0) check game over
	}

}
