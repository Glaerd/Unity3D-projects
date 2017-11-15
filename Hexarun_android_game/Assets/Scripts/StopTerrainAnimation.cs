using UnityEngine;
using System.Collections;

public class StopTerrainAnimation : MonoBehaviour {

	void StopAnimation() {
		this.GetComponent<Animator> ().SetBool ("Active", false);
	}
}
