using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Poof : MonoBehaviour {

	float animIndex;

	Sprite[] sprites;

	// Start is called before the first frame update
	void Start() {
		animIndex = 0;
		sprites = Resources.LoadAll<Sprite>("Poof");
	}

	// Update is called once per frame
	void Update() {
		animIndex += Time.deltaTime * 20;
		if(animIndex < 11) {
			gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int) animIndex];
		} else {
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			if(animIndex / 20 > 4.288) {
				SceneManager.LoadScene("Level1");
			}
		}
	}
}
