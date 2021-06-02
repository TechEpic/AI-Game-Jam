using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {

	public float BonkStrength = 2000;

	GameObject player;
	DirectedMovement mover;

	Vector2 targPos;
	Dictionary<Vector2Int, Vector2> path;
	bool changedPos;

	void Start() {
		// Grab the mover script and the player object
		mover = gameObject.GetComponent<DirectedMovement>();
		player = GameObject.Find("Player");
		changedPos = true;
	}

	void Update() {
		// Check if the enemy can see the player
		if(Vision.VisionCheck(player.transform.position, transform.position, 0.2f)) {
			// Update the last known position of the player
			changedPos = true;
			targPos = player.transform.position;
			mover.Movement = targPos - (Vector2) transform.position;
		} else {
			Vector2Int intPos = Vector2Int.RoundToInt(transform.position);
			if(changedPos) {
				path = Pathfinding.GetPath(intPos, Vector2Int.RoundToInt(targPos));
				changedPos = false;
			}

			if(path != null && path.ContainsKey(intPos)) {
				mover.Movement = path[intPos];
			} else {
				changedPos = true;
				mover.Movement = Vector2.zero;
			}
		}
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if(col.collider.name == "Player") {
			player.GetComponent<PlayerHealth>().Hurt(1);
			Vector2 rel = player.transform.position - transform.position;
			player.GetComponent<Rigidbody2D>().AddForce(rel.normalized * BonkStrength);
			gameObject.GetComponent<Rigidbody2D>().AddForce(rel.normalized * (BonkStrength / -2));
		}
	}
}
