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

}
