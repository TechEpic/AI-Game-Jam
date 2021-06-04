using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleBehavior : MonoBehaviour {

	static int highScore;

	static int score;

	public GameObject txtObj;

	Text text;

	// Start is called before the first frame update
	void Start() {
		text = txtObj.GetComponent<Text>();
		score = 0;
		text.text = score + "/" + highScore;
	}

	// Update is called once per frame
	void Update() {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.name == "Player") {
			score++;
			if(score > highScore) {
				highScore = score;
			}
			text.text = score + "/" + highScore;
			transform.position = DroneSpawner.RandomLocation();
			gameObject.GetComponents<AudioSource>()[0].Play();
		}
	}
}
