using UnityEngine;
using System.Collections;

public class SetPathDeny : MonoBehaviour {

	public GameObject Bob;
	public GameObject[] Hexagones;
	public GameObject[] Floors;

	private Attributes hexagonAttributes;
	private bool front_block;
	private bool left_block;
	private bool right_block;


	void Start() {
		Hexagones = GameObject.FindGameObjectsWithTag ("Hexagon");
	}

	public void testIfBlocked() {

		front_block = false;
		right_block = false;
		left_block = false;
		foreach (GameObject hexagon in Hexagones) {
			//Debug.Log ("SetPathDeny");
			hexagonAttributes = hexagon.GetComponent<Attributes>();
			if ((Mathf.Abs (Bob.GetComponent<Player_move> ().player_y - hexagonAttributes.y) >= 1 || hexagonAttributes.occupied == true) && Bob.GetComponent<Player_move> ().player_x == hexagonAttributes.x && Bob.GetComponent<Player_move> ().player_z == hexagonAttributes.z - 2) {
				front_block = true;
			}
			if ((Mathf.Abs (Bob.GetComponent<Player_move> ().player_y - hexagonAttributes.y) >= 1 || hexagonAttributes.occupied == true) && Bob.GetComponent<Player_move> ().player_x == hexagonAttributes.x + 1 && Bob.GetComponent<Player_move> ().player_z == hexagonAttributes.z - 1) {
				left_block = true;
			}
			if ((Mathf.Abs (Bob.GetComponent<Player_move> ().player_y - hexagonAttributes.y) >= 1 || hexagonAttributes.occupied == true) && Bob.GetComponent<Player_move> ().player_x == hexagonAttributes.x - 1 && Bob.GetComponent<Player_move> ().player_z == hexagonAttributes.z - 1) {
				right_block = true;
			}
			if (front_block && left_block && right_block) {
				Bob.GetComponent<Player_health>().Player_health1 = 0;
				Bob.GetComponent<Player_health>().enddeathanimation = true;
			}
		}
	}

	public void SetPathDenial (int floorNumber) {

	}

}