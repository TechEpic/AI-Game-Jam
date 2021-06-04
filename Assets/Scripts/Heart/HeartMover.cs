using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartMover : MonoBehaviour {

	Vector3 relPos;
	GameObject player;

	// Start is called before the first frame update
	void Start() {
		relPos = transform.position;
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void Update() {
		transform.position = player.transform.position + relPos;
	}
}
