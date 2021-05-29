using UnityEngine;

// General controller for the player that handles user input
public class PlayerController : MonoBehaviour {
	
	OctoMovement mover;

	void Start() {
		mover = gameObject.GetComponent<OctoMovement>();
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
		mover.Movement = movement;
	}
}
