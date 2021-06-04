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
	public GameObject Ability;
	

	static Sprite[] sprites;

	int health;
	float healTime;

	float animIndex;
	float abilIndex;

	GameObject[] hearts;
	GameObject ability;

	public GameObject Poof;

	public int GetHealth() {
		return health;
	}

	public bool Hurt(int amt) {
		health -= amt;
		if(health == 0) {
			gameObject.GetComponent<PlayerController>().enabled = false;
			UpdateHearts();
			base.enabled = false;
			gameObject.GetComponent<DirectedMovement>().Movement = Vector2.zero;
			gameObject.GetComponents<AudioSource>()[4].Play();
			gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			gameObject.GetComponent<CircleCollider2D>().enabled = false;
			Camera.main.GetComponent<AudioSource>().Stop();
			Instantiate(Poof, transform.position, Quaternion.identity);
			HideAll();
			return true;
		}
		if(health > 0)
			gameObject.GetComponents<AudioSource>()[1].Play();
		healTime = HealDelay;
		return false;
	}

	public void HideAll() {
		foreach(GameObject heart in hearts) {
			heart.GetComponent<SpriteRenderer>().enabled = false;
		}
		ability.GetComponent<SpriteRenderer>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
			float angle = i * Mathf.PI * 2 / 4 - Mathf.PI / 2;
			hearts[i] = Instantiate(Heart, new Vector3(Mathf.Sin(angle) * 0.4f, Mathf.Cos(angle) * 0.4f, 0), Quaternion.identity);
		}
		ability = Instantiate(Ability, Vector3.down * 0.4f, Quaternion.identity);
		UpdateHearts();
		ability.GetComponent<SpriteRenderer>().sprite = sprites[96];
	}

	void Update() {
		animIndex += Time.deltaTime * AnimSpeed;
		animIndex %= 48;
		if(PlayerController.isJumping) {
			abilIndex += Time.deltaTime * AnimSpeed;
			abilIndex %= 60;
		} else {
			abilIndex = 0;
		}
		if(health < MaxHealth) {
			healTime -= Time.deltaTime;
			if(healTime <= 0) {
				Heal(1);
				healTime += HealPeriod;
			}
		}
		UpdateHearts();
		ability.GetComponent<SpriteRenderer>().sprite = sprites[(int) (abilIndex + 10) % 60 + 96];
		if(PlayerController.jumpCD <= 0 && !ability.GetComponent<SpriteRenderer>().enabled) {
			gameObject.GetComponents<AudioSource>()[3].Play();
		}
		ability.GetComponent<SpriteRenderer>().enabled = PlayerController.jumpCD <= 0;
	}
}
