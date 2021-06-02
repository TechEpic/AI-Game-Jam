using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour {

	public float BonkStrength = 2000;
	public float VisionDelay = 0.5f;

	float visionDelay;

	GameObject player;
	DirectedMovement mover;

	Vector2 targPos;
	Dictionary<Vector2Int, Vector2> path;
	bool changedPos;
	bool isChasing;

	void Start() {
		// Grab the mover script and the player object
		mover = gameObject.GetComponent<DirectedMovement>();
		player = GameObject.Find("Player");
		changedPos = false;
		isChasing = false;
	}

	void Update() {
		visionDelay -= Time.deltaTime;
		// Check if the enemy can see the player
		if(Vision.VisionCheck(player.transform.position, transform.position, 0.17f)) {
			if(visionDelay <= 0) {
				// Update the last known position of the player
				changedPos = true;
				targPos = player.transform.position;
				mover.Movement = targPos - (Vector2) transform.position;
				isChasing = true;
			}
		} else {
			Vector2Int tilePos = Pathfinding.TileSpace(transform.position);
			if(isChasing) {
				if(changedPos) {
					path = Pathfinding.GetPath(tilePos, Pathfinding.TileSpace(targPos));
					changedPos = false;
				}
			} else {
				visionDelay = VisionDelay;
			}
			if(path != null) {
				if(path.ContainsKey(tilePos)) {
					mover.Movement = path[tilePos];
					if(mover.Movement == Vector2.zero) {
						isChasing = false;
					}
				} else {
					mover.Movement = Vector2.zero;
					changedPos = true;
				}
			} else {
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
