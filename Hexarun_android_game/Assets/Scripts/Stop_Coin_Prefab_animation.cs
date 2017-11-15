using UnityEngine;
using System.Collections;

public class Stop_Coin_Prefab_animation : MonoBehaviour {

	void StopAnimation() {
		this.GetComponent<Animator> ().SetBool ("Appearing", false);
	}

	void StopMoveAnimation() {
	}

}
