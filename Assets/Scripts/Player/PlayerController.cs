using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	
	public float Acceleration;
	public float Deceleration;
	public float MaxSpeed;

	Rigidbody2D rb;

	void Start() {
		// Get the rigid body object to add forces to
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	
	void Update() {
		// Determine the direction that the player wants to move in
		Vector2 movement = new Vector2(0, 0);
		if(Input.GetKey(KeyCode.W)) {
			movement += new Vector2(0, 1);
		}
		if(Input.GetKey(KeyCode.S)) {
			movement += new Vector2(0, -1);
		}
		if(Input.GetKey(KeyCode.A)) {
			movement += new Vector2(-1, 0);
		}
		if(Input.GetKey(KeyCode.D)) {
			movement += new Vector2(1, 0);
		}
		// No need to calculate this multiple times
		float speed = rb.velocity.magnitude;
		bool isSlowing = true;
		// Determine if the player is accelerating in any direction
		if(movement.sqrMagnitude > 0) {
			isSlowing = false;
			// Apply the accelerating force
			movement = movement.normalized * Acceleration;
			rb.AddForce(movement);
		}
		// Check if the player has exceeded the max speed
		if(speed > MaxSpeed) {
			// Apply the acceleration force to cancel out acceleration that would put the player past the max speed
			rb.AddForce(-rb.velocity / speed * Acceleration);
			// Check if the player is decelerating
		} else if(isSlowing && speed > 0) {
			// Apply the decelerating force, capping it at the current velocity to prevent vibration
			rb.AddForce(-rb.velocity / speed * Mathf.Min(Deceleration, speed / Time.deltaTime));
		}
	}
}
