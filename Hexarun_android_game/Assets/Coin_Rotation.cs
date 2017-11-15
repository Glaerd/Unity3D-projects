using UnityEngine;
using System.Collections;

public class Coin_Rotation : MonoBehaviour {

	private Quaternion temp_rotate;

	void Update () {
		temp_rotate.x = this.transform.rotation.x;
		temp_rotate.y = this.transform.rotation.y;
		temp_rotate.z += Time.deltaTime;
		this.transform.rotation = temp_rotate;
	}
}
