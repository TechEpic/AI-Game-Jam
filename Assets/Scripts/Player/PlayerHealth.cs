using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int MaxHealth = 3;
	// Period of time that passes in-between heal cycles
	public float HealPeriod = 1;
	// Time that must pass after the player has taken damage in order to start healing
	public float HealDelay = 10;

	int health;
	float healTime;

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
		health = Mathf.Max(health + amt, MaxHealth);
		return true;
	}

	void Start() {
		health = MaxHealth;
	}

	void Update() {
		healTime -= Time.deltaTime;
		if(healTime <= 0) {
			Heal(1);
			healTime += HealPeriod;
		}
	}
}
