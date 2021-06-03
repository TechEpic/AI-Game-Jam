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
		sr.sprite = stationary[0];
	}

	
	void Update() {
		
	}
}
