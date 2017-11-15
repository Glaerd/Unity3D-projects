using UnityEngine;
using System.Collections;

public class Ennemy_Behaviour : MonoBehaviour {
	
	public bool playermoved = false;
	public bool rowmoved = false;
	public GameObject Floor;
	public GameObject[] Floors;
	public GameObject Ennemy_1;
	public GameObject Coin_Prefab;
	private GameObject ennemy;
	private Vector3 ennemy_temp;
	public bool wait_ennemy_move;
	private int j;

	void Start () {
		ennemy_temp = new Vector3 (0, 0, 0);
	}

	IEnumerator WaitEnnemyMove(){
		if (this.transform.childCount > 0) {
			yield return new WaitForSeconds (0.5f);
			while (wait_ennemy_move == true) {
				if (this.transform.GetChild (0).GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
					Floor.GetComponent<SetOccupation> ().SetTerrainOccupation ();
					Floor.GetComponent<SetPathDeny> ().testIfBlocked ();
					break;
				}
				yield return 0;
			}
		} else {
			Floor.GetComponent<SetOccupation> ().SetTerrainOccupation ();
			Floor.GetComponent<SetPathDeny> ().testIfBlocked ();
			yield return 0;
		}
	}

	void Update () {
		if (playermoved == true) {
			for(int i = 0; i < this.transform.childCount; i++) {
				if(this.transform.GetChild(i).transform.position.z > Floors[j].transform.GetChild(0).GetChild(0).transform.position.z) Destroy(this.transform.GetChild(i).gameObject);
				else if(this.transform.GetChild(i).GetComponent<Ennemy_move>().IsInstantiated == true) this.transform.GetChild(i).GetComponent<Ennemy_move>().ennemyMove();
			}
			playermoved = false;
			wait_ennemy_move = true;
			StartCoroutine(WaitEnnemyMove());
		}
	}
	
	public void rowMoved(int floorNumber) {
		j = floorNumber + 1;
		for(int i = 0; i < 22; i++) {
			int random = Random.Range(0,1000);
			if(random > 900) {
				ennemy = (GameObject) Instantiate(Ennemy_1);
				ennemy.transform.parent = transform;
				ennemy_temp.x = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.x;
				ennemy_temp.y = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.y + 1;
				ennemy_temp.z = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.z - 0.41f;
				ennemy.transform.position = ennemy_temp;
				ennemy.GetComponent<Ennemy_move>().ennemy_x = Floors[floorNumber].transform.GetChild(0).GetChild(i).GetComponent<Attributes>().x;
				ennemy.GetComponent<Ennemy_move>().ennemy_y = Floors[floorNumber].transform.GetChild(0).GetChild(i).GetComponent<Attributes>().y;
				ennemy.GetComponent<Ennemy_move>().ennemy_z = Floors[floorNumber].transform.GetChild(0).GetChild(i).GetComponent<Attributes>().z;
				ennemy.GetComponent<Animator>().SetBool("Appearing", true);
			}
			if(random > 890 && random <= 900) {
				ennemy = (GameObject) Instantiate(Coin_Prefab);
				ennemy_temp.x = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.x;
				ennemy_temp.y = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.y + 2.4f;
				ennemy_temp.z = Floors[floorNumber].transform.GetChild(0).GetChild(i).transform.position.z - 0.41f;
				ennemy.transform.position = ennemy_temp;
				ennemy.GetComponent<Animator>().SetBool("Appearing", true);
			}
		}
		if (j == 11) j = 0;
	}
	
}