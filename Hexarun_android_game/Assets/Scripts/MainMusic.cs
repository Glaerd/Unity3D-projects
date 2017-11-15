using UnityEngine;
using System.Collections;

public class MainMusic : MonoBehaviour {

	public AudioSource Music;
	public AudioClip Opening;
	public AudioClip Middle;
	private bool startmusic;

	void Start(){
		Music.clip = Opening;
		Music.Play();
	}

	void Update(){
		if (Music.time >= Opening.length - 0.05f && startmusic == false) {
			Music.clip = Middle;
			Music.Play();
			startmusic = true;
			Music.loop = true;
		}
	}

}
