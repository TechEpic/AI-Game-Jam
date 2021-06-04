using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnimator : MonoBehaviour {
	
	public float Speed = 20;

	static readonly int IDLE = 1;
	static readonly int SIGNAL = 2;
	static readonly int WALK = 3;
	static int dirOffset = 151;
	static int[] stateOffsets = {0, 0, 54, 80, dirOffset};
	static Sprite[] sprites;

	int curState;
	int prevDir;
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
		prevDir = 0;
	}


	void Update() {
		int direction = 0;
		bool isMoving = rb.velocity.sqrMagnitude > 0.01;
		if(curState == IDLE && (isMoving || gameObject.GetComponent<DroneController>().isChasing)) {
			curState = WALK;
			animIndex = 0;
		} else if(curState == WALK && (!isMoving && !gameObject.GetComponent<DroneController>().isChasing)) {
			curState = IDLE;
			animIndex = 0;
		}
		if(isMoving) {
			direction = (int) ((Mathf.Atan2(rb.velocity.y, rb.velocity.x) + Mathf.PI * 1.125) / Mathf.PI * 4) % 8;
		} else {
			direction = prevDir;
		}
		animIndex += Time.deltaTime * Speed;
		animIndex %= stateOffsets[curState + 1] - stateOffsets[curState];
		// maffy waffy
		int index = (int) animIndex + direction * dirOffset + stateOffsets[curState];
		sr.sprite = sprites[index];
		prevDir = direction;
	}
}
