using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimator : MonoBehaviour {
	
	public float Speed = 20;

	static readonly int IDLE = 1;
	static readonly int SIGNAL = 2;
	static readonly int WALK = 3;
	static int dirOffset = 100;
	static int[] stateOffsets = {0, 0, 30, 60, dirOffset};
	static Sprite[] sprites;

	int curState;
	float animIndex;
	SpriteRenderer sr;
	Rigidbody2D rb;

	void Start() {
		sr = gameObject.GetComponent<SpriteRenderer>();
		rb = gameObject.GetComponent<Rigidbody2D>();
		if(sprites == null) {
			sprites = Resources.LoadAll<Sprite>("Drone");
		}
		animIndex = 0;
		curState = IDLE;
	}


	void Update() {
		animIndex += Time.deltaTime * Speed;
		animIndex %= stateOffsets[curState + 1] - stateOffsets[curState];

		int index = (int) (animIndex + (int) ((Mathf.Atan2(rb.velocity.y, rb.velocity.x) + Mathf.PI) / Mathf.PI * 4) * dirOffset);

	}
}
