using UnityEngine;

// General controller for the player that handles user input
public class PlayerController : MonoBehaviour {
	
	DirectedMovement mover;
	Rigidbody2D rb;

	public Animator animator;

	bool diagonal;

	public static bool isJumping = false;
	public static float JumpVision = 2f;
	float jumpTimer = 0;

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
		if(Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
			jumpTimer = 1f;
		}
		if(jumpTimer <= 0 && isJumping) {
			isJumping = false;
			Vector2 dir = ((Vector2) Camera.main.ScreenToWorldPoint((Vector2) Input.mousePosition)
				- (Vector2) transform.position);
			if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
				dir.y = 0;
			} else {
				dir.x = 0;
			}
			dir = dir.normalized * 0.9f;
			RaycastHit2D hit = Vision.MoveCast(transform.position, dir, 0.17f);
			if(hit.distance < 1) {
				transform.position = (Vector2) transform.position + dir * hit.distance;
				hit = Vision.MoveCast((Vector2) transform.position + dir, -dir, 0.17f);
				if(hit.distance > 0) {
					transform.position = (Vector2) transform.position - dir * hit.distance + dir;
				}
			}
		} else {
			jumpTimer -= Time.deltaTime;
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
