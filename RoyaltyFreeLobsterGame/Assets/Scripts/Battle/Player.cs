using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour {
	private int health = 5;
	public Text hpText;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public GameObject manager;
  public PipsDisplay rockHealthDisplay;
  public GameObject perilIndicator;

  void Start() {
    UpdateHealthUI();
  }
	public int GetHealth()
	{
		return health;
	}

    public void UpdateHealthUI()
    {
        hpText.text = health.ToString();
        if(rockHealthDisplay) {
          rockHealthDisplay.setLevel(health);
        }
        if(health==1&&perilIndicator) {
          perilIndicator.SetActive(true);
        }
    }

	public void GetHurt(int damage)
	{
    if(damage==0)return;
    Shake shake = GetComponentInChildren<Shake>();
    if(shake)shake.setShakeValue(1);
		health -= damage;
		if(health < 0) health = 0;
		UpdateHealthUI();
		if(health == 0)
		{
			LoseGame();
		}
		//when getting hurt, drop rocks
		if(damage > 0)
		{
			if(tag == "Enemy")
			{
				//enemy Place Rock
				StartCoroutine(manager.GetComponent<EnemyManager>().PlaceRockWithDelay());
			}
		}
	}

	public void LoseGame()
	{
			if(tag == "Player")
			{
				manager.GetComponent<TurnManager>().GameOver(false);
				gameOverText.text = "DEFEAT";
			}
			else
			{
				manager.GetComponent<TurnManager>().GameOver(true);
				gameOverText.text = "VICTORY";
			}
			gameOverPanel.SetActive(true);
	}

}
