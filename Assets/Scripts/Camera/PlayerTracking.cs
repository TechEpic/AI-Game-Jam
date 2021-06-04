using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking : MonoBehaviour {

	public float closingRatio = 0.9F;
	float logClosingRatio;

	GameObject player;

	// Start is called before the first frame update
	void Start() {
		player = GameObject.Find("Player");
		logClosingRatio = -1 * Mathf.Log(1 - closingRatio);
	}

	// Update is called once per frame
	void LateUpdate() {
		Vector2 targ = player.transform.position 
			+ (Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position) / 3;
		Vector2 deltaPosition = (Vector3) targ - gameObject.transform.position;
		transform.position += (Vector3) (logClosingRatio * deltaPosition * Time.deltaTime);
	}
}
