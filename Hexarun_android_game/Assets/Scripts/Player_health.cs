using UnityEngine;
using System.Collections;

public class Player_health : MonoBehaviour {

	public Texture DeathGUI;
	public int Player_health1;
	private GameObject InGameGUI;
	public bool activeGui = false;
	public GameObject DeathGUIGO;
	public bool enddeathanimation = false;


	void Start () {
		Player_health1 = 3;
		InGameGUI = GameObject.Find ("InGameGUI");
	}

	void Update () {

		InGameGUI.GetComponent<GameGui> ().heartnumber = Player_health1;
		if (Player_health1 <= 0 && activeGui == false && enddeathanimation) {
			Death ();	
		}

	}

	public void Death() {
		activeGui = true;
		DeathGUIGO.GetComponent<DeathGUI> ().enabled = true;
	}
}
