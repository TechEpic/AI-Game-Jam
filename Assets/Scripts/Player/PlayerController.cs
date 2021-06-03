using UnityEngine;

// General controller for the player that handles user input
public class PlayerController : MonoBehaviour {
	
	DirectedMovement mover;
	Rigidbody2D rb;

	public Animator animator;

	bool diagonal;

	void Start() {
		// Grab the mover
		mover = gameObject.GetComponent<DirectedMovement>();
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
		mover.Movement = movement;
		float speed = rb.velocity.magnitude;
		if (speed > 0.01) {
			float moveAngle = Mathf.Atan2(rb.velocity.x, -rb.velocity.y);
			moveAngle += (moveAngle < 0) ? 2 * Mathf.PI : 0;
			int moveDir = 0;
			while (moveAngle > 3 * Mathf.PI / 8) {
				moveAngle -= Mathf.PI / 2;
				moveDir++;
			}
			diagonal = moveAngle > Mathf.PI / 8;
			rb.transform.eulerAngles = Vector3.forward * moveDir * 90;
		}
		animator.SetFloat("speed", speed);
		animator.SetBool("diagonal", diagonal);
	}
}
