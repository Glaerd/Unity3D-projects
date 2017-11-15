using UnityEngine;
using System.Collections;

public class Animation_handler : MonoBehaviour {

	void MoveAnimationOff() {
		this.GetComponentInParent<Player_move>().MoveAnimationOff = true;
	}

	void MemoryInputWhileAnimation() {
		this.GetComponentInParent<Player_move>().MemoryInputWhileAnimation = true;
	}

	void IdleAnimationOn() {
		this.GetComponentInParent<Player_move>().IdleAnimationOn = true;
		if(this.GetComponent<Animator>().GetBool("OnHit") == false) this.GetComponentInParent<Player_move> ().WaitForEnnemyHit = false;
	}

	void OnHit() {
		this.GetComponent<Animator>().SetBool("OnHit",false);
	}

	void OnDeath() {
		this.GetComponent<Animator>().SetBool("OnDeath",false);
		this.GetComponentInParent<Player_health> ().enddeathanimation = true;
	}
}
