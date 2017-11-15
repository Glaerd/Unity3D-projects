using UnityEngine;
using System.Collections;

public class Stop_Ennemy_1_animation : MonoBehaviour {
	
	public GameObject Bob;
	
	void Start() {
		Bob = GameObject.Find ("Player");
	}
	
	void StopAnimation() {
		this.GetComponent<Animator> ().SetBool ("Appearing", false);
		this.GetComponent<Ennemy_move>().IsInstantiated = true;
	}
	
	void StopMoveAnimation() {
		this.GetComponent<Ennemy_move>().MoveAnimationOff = true;
	}
	
}
