using UnityEngine;

// 8-directional movement script
// Entities with this script must also have a Rigidbody2D
// To control this, set the Movement variable in another script ABOVE this one
// Movement doesn't need to be normalized, it's just a direction
public class OctoMovement : MonoBehaviour {

	// Self explanatory
	public float Acceleration;
	public float Deceleration;
	public float MaxSpeed;

	// The direction that the entity wants to move in
	[HideInInspector]
	public Vector2 Movement;
	
	Rigidbody2D rb;

	void Start() {
		// Get the rigid body object to add forces to
		rb = gameObject.GetComponent<Rigidbody2D>();
	}

	void Update() {
		// No need to calculate this multiple times
		float speed = rb.velocity.magnitude;
		bool isSlowing = true;
		// Determine if the player is accelerating in any direction
		if(Movement.sqrMagnitude > 0) {
			isSlowing = false;
			// Apply the accelerating force
			Movement = Movement.normalized * Acceleration;
			rb.AddForce(Movement);
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
