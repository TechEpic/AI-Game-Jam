using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
	public AudioClip hard;
	public AudioClip easy;

	// Start is called before the first frame update
	void Start() {
		gameObject.GetComponent<AudioSource>().clip = DroneController.Difficulty == 1 ? hard : easy;
		gameObject.GetComponent<AudioSource>().Play();
	}

	// Update is called once per frame
	void Update() {
		
	}
}
