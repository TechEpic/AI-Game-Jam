using UnityEngine;
using UnityEngine.SceneManagement;

// Put on an entity to make it invisible when the player doesn't have line of sight to it
public class OcclusionHiding : MonoBehaviour {

	// Radius of the entity
	public float Radius;

	SpriteRenderer sr;
	GameObject player;

	void Start() {
		// You know the deal, get the stuff
		sr = gameObject.GetComponent<SpriteRenderer>();
		player = GameObject.Find("Player");
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			DroneController.Difficulty = 0.6f;
			SceneManager.LoadScene("Level1");
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			DroneController.Difficulty = 1f;
			SceneManager.LoadScene("Level1");
		}

		// Only show if visible to the player
		sr.enabled = Vision.VisionCheck(transform.position, player.transform.position, Radius)
			|| (PlayerController.isJumping
			&& (transform.position - player.transform.position).magnitude < Radius + PlayerController.JumpVision);
	}
}
