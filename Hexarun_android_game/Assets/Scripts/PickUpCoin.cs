using UnityEngine;
using System.Collections;

public class PickUpCoin : MonoBehaviour {

	private GameObject Bob;

	void Start() {

		Bob = GameObject.Find ("Player");

	}

	void OnTriggerEnter(Collider other){

		//if(other.gameObject.tag == "Player") {

			Bob.GetComponent<Player_move>().score += 10;
			Destroy(this.gameObject);

		//}

	}

}
