using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimator : MonoBehaviour {
	
	static Sprite[] stationary;
	SpriteRenderer sr;

	void Start() {
		sr = gameObject.GetComponent<SpriteRenderer>();
		
		if(stationary == null) {
			stationary = Resources.LoadAll<Sprite>("Drone");
		}
	}

	int fc = 0;
	void Update() {
		sr.sprite = stationary[fc++ % stationary.Length];
	}
}
