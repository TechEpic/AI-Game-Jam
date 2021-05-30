using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {

	GameObject player;
	DirectedMovement mover;

	Vector2 knownPos;
	bool hasSeenPlayer;

	void Start() {
		// Grab the mover script and the player object
		mover = gameObject.GetComponent<DirectedMovement>();
		player = GameObject.Find("Player");
		hasSeenPlayer = false;
	}

	void Update() {
		// Check if the enemy can see the player
		if(Vision.VisionCheck(player.transform.position, transform.position, 0.2f)) {
			// Update the last known position of the player
			knownPos = player.transform.position;
			hasSeenPlayer = true;
		}
		// Don't try to go somewhere when there's nowhere to go
		if(hasSeenPlayer) {
			// Relative position of the last known position of the player
			Vector2 rel = knownPos - (Vector2) transform.position;
			// Check if the drone isn't "close enough" to where the player is/was
			if(rel.sqrMagnitude > 0.3 * 0.3) {
				// Move towards the last known position of the player
				mover.Movement = rel;
			} else {
				// Stop moving
				mover.Movement = Vector2.zero;
			}
		}
	}
}
