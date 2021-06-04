using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int MaxHealth = 3;
	// Period of time that passes in-between heal cycles
	public float HealPeriod = 1;
	// Time that must pass after the player has taken damage in order to start healing
	public float HealDelay = 10;

	public float AnimSpeed = 20;

	public GameObject Heart;
	

	static Sprite[] sprites;

	int health;
	float healTime;

	float animIndex;

	GameObject[] hearts;

	public int GetHealth() {
		return health;
	}

	public bool Hurt(int amt) {
		health -= amt;
		if(health <= 0) {
			//Unga bunga you lose
			Debug.Log("player is super ded");
			return true;
		}
		healTime = HealDelay;
		return false;
	}

	public bool Heal(int amt) {
		if(health == MaxHealth) {
			return false;
		}
		health = Mathf.Min(health + amt, MaxHealth);
		return true;
	}

	void UpdateHearts() {
		for(int i = 0; i < hearts.Length; i++) {
			if(i < health) {
				hearts[i].GetComponent<SpriteRenderer>().sprite = sprites[(int) animIndex + 48];
			} else {
				hearts[i].GetComponent<SpriteRenderer>().sprite = sprites[(int) animIndex];
			}
		}
	}

	void Start() {
		animIndex = 0;
		if(sprites == null) {
			sprites = Resources.LoadAll<Sprite>("Health");
		}
		health = MaxHealth;
		hearts = new GameObject[MaxHealth];
		for(int i = 0; i < hearts.Length; i++) {
			float angle = i * Mathf.PI * 2 / 4;
			hearts[i] = Instantiate(Heart, new Vector3(Mathf.Sin(angle) * 0.4f, Mathf.Cos(angle) * 0.4f, 0), Quaternion.identity);
		}
		UpdateHearts();
	}

	void Update() {
		animIndex += Time.deltaTime * AnimSpeed;
		animIndex %= 48;
		if(health < MaxHealth) {
			healTime -= Time.deltaTime;
			if(healTime <= 0) {
				Heal(1);
				healTime += HealPeriod;
			}
		}
		UpdateHearts();
	}
}
