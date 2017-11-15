using UnityEngine;
using System.Collections;

public class Ennemy_move : MonoBehaviour {
	
	public float ennemy_x;
	public float ennemy_y;
	public float ennemy_z;
	public float ennemy_occupy_attack_x;
	public float ennemy_occupy_attack_z;
	public GameObject[] Hexagones;
	public bool MoveAnimationOff = false;
	public bool IsInstantiated = false;
	private GameObject Bob;
	private bool notNextTo = false;
	private Attributes hexagonAttributes;
	private Vector3 ennemyTempPosition;
	private int notFront;
	private int dir_x;
	private int dir_z;
	private int rand;
	public float attack = -1;
	private bool wait_ennemy_move = true;
	public int ennemyflnumber;
	public int nextflnumber;
	public int next_ennemy_pos;

	void Start() {
		Hexagones = GameObject.FindGameObjectsWithTag ("Hexagon");
		Bob = GameObject.Find("Player");
	}

	void LateUpdate() {
		this.GetComponent<Animator>().SetFloat("Move",0);
		this.GetComponent<Animator>().SetFloat("Attack",-1);
	}

	void Update() {
		if (MoveAnimationOff == true) {
			this.GetComponent<Animator>().SetBool("MoveAnimationOff",true);
			this.transform.position = ennemyTempPosition;
			MoveAnimationOff = false;
		}
	}

	public void ennemyMove() {
		notFront = 0;
		rand = Random.Range(0,2);
		if (ennemy_x == Bob.GetComponent<Player_move> ().player_x - 1 && ennemy_z == Bob.GetComponent<Player_move> ().player_z + 1 && Mathf.Abs(ennemy_y - Bob.GetComponent<Player_move> ().player_y) < 1) {
			wait_ennemy_move = true;
			Bob.GetComponent<Player_move>().WaitForEnnemyHit = true;
			StartCoroutine(WaitPlayerMove(1, -1));
		} else if (ennemy_x == Bob.GetComponent<Player_move> ().player_x + 1 && ennemy_z == Bob.GetComponent<Player_move> ().player_z + 1 && Mathf.Abs(ennemy_y - Bob.GetComponent<Player_move> ().player_y) < 1) {
			wait_ennemy_move = true;
			Bob.GetComponent<Player_move>().WaitForEnnemyHit = true;
			StartCoroutine(WaitPlayerMove(-1, -1));
		} else if (ennemy_x == Bob.GetComponent<Player_move> ().player_x && ennemy_z == Bob.GetComponent<Player_move> ().player_z + 2 && Mathf.Abs(ennemy_y - Bob.GetComponent<Player_move> ().player_y) < 1) {
			wait_ennemy_move = true;
			Bob.GetComponent<Player_move>().WaitForEnnemyHit = true;
			StartCoroutine(WaitPlayerMove(0, -2));
		} else {
			ennemy_occupy_attack_x = 0;
			ennemy_occupy_attack_z = -1;
			notNextTo = true;
			Move (0, -2);
		}
	}

	void Move(int dir_x, int dir_z) {
		
		if (((int)ennemy_z + dir_z)%22 == -1) {
			next_ennemy_pos = 21;
			MoveEnnemy(dir_x, dir_z);
		} 
		else if (((int)ennemy_z + dir_z)%22 == -2) {
			next_ennemy_pos = 20;
			MoveEnnemy(dir_x, dir_z);
		} else {
			next_ennemy_pos = ((int)ennemy_z + dir_z)%22;
			MoveEnnemy(dir_x, dir_z);
		}
	}

	bool CanEnnemyMoveNextY(float y, int dir_x, int dir_z){
		if (Mathf.Abs (y) < 1)
			return Bob.GetComponent<Player_move>().HexagonesArray [((int)ennemy_z + dir_z)%22, (int)ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal [nextflnumber]].GetComponent<Attributes> ().y == Bob.GetComponent<Player_move>().HexagonesArray [(int)ennemy_z%22, (int)ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber]].GetComponent<Attributes> ().y + y;
		else
			return false;
	}
	
	int HexagonesArrayZ(string state,int dir_z) {
		if (state == "Next")
			return next_ennemy_pos;
		else if (state == "Current")
			return (int)ennemy_z%22;
		else
			return 0;
	}
	
	int HexagonesArrayX(string state,int dir_x) {
		if (state == "Next")
			return (int)ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber];
		else if (state == "Current")
			return (int)ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber];
		else
			return 0;
	}
	
	float GetObjectYAttribute(GameObject thisObject) {
		return thisObject.GetComponent<Attributes> ().y;
	}
	
	bool NextHexagonOccupation(int dir_x, int dir_z) {
		return Bob.GetComponent<Player_move>().HexagonesArray [((int)ennemy_z + dir_z)%22, (int)ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes> ().occupied;
	}

	void MoveEnnemy(int dir_x, int dir_z) {

		ennemyflnumber = (int) Mathf.Round((((int)ennemy_z%22 - 0.25f)/2));
		if (dir_z == -2) {
			if ((int)Mathf.Round (((int)ennemy_z%22 - 2 - 0.25f)/2) < 0)
				nextflnumber = 10;
			else
				nextflnumber = (int)Mathf.Round (((int)ennemy_z%22 - 2 - 0.25f)/2);
		}
		else if (dir_z == -1) {
			if ((int)Mathf.Round (((int)ennemy_z%22 - 1 - 0.25f)/2) < 0) 
				nextflnumber = 10;
			else 
				nextflnumber = (int)Mathf.Round ((((int)ennemy_z%22 - 1 - 0.25f)/2));
		}
		if(HexagonesArrayX("Current",dir_x) >= 0 && HexagonesArrayX("Current",dir_x) <= 44 && HexagonesArrayX("Next",dir_x) >= 0 && HexagonesArrayX("Next",dir_x) <= 44) {

			float diffY = GetObjectYAttribute(Bob.GetComponent<Player_move>().HexagonesArray[HexagonesArrayZ("Next",dir_z),HexagonesArrayX("Next",dir_x)]) - GetObjectYAttribute(Bob.GetComponent<Player_move>().HexagonesArray[HexagonesArrayZ("Current",dir_z),HexagonesArrayX("Current",dir_x)]);
			if(CanEnnemyMoveNextY(diffY,dir_x,dir_z) &&  NextHexagonOccupation(dir_x,dir_z) == false) {
				if(Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().x == Bob.GetComponent<Player_move>().HexagonesArray[(int)ennemy_z%22,(int) ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber]].GetComponent<Attributes>().x + dir_x && Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().z == Bob.GetComponent<Player_move>().HexagonesArray[(int)ennemy_z%22,(int) ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber]].GetComponent<Attributes>().z + dir_z) {
					this.GetComponent<AudioSource>().Play ();
					if(dir_x == 0) {
						if(diffY == 0) this.GetComponent<Animator>().SetFloat("Move",1);
						if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Move",4);
						if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Move",7);
					}
					if(dir_x == 1 ) {
						if(diffY == 0) this.GetComponent<Animator>().SetFloat("Move",2);
						if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Move",5);
						if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Move",8);
					}
					if(dir_x == -1 ) {
						if(diffY == 0) this.GetComponent<Animator>().SetFloat("Move",3);
						if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Move",6);
						if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Move",9);
					}
					this.GetComponent<Animator>().SetBool("MoveAnimationOff",false);
					ennemyTempPosition.z = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].transform.position.z - 0.41f;
					ennemyTempPosition.y = this.transform.position.y + diffY;
					ennemyTempPosition.x = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].transform.position.x;
					float ennemy_x2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().x;
					float ennemy_y2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().y;
					float ennemy_z2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().z;
					ennemy_x = ennemy_x2;
					ennemy_y = ennemy_y2;
					ennemy_z = ennemy_z2;
					notFront = 1;
				}
			}
			NextMove (dir_x, dir_z);
		}
	}

	void NextMove(int dir_x, int dir_z) {

		if (dir_x == 1 && dir_z == -1 && notFront == 2 && (int)ennemy_x + 22 + Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber] > 0) {
			notFront = 3;
			Move (-1, -1);
		}
		else if(dir_x == -1 && dir_z == -1 && notFront == 2) {
			notFront = 3;
			Move (1, -1);
		}
		else if (notFront == 0) {
			if (rand == 0) {
				if((int)ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber] <= 44) {
					notFront = 2;
					Move (1, -1);
				}
				else {
					rand = 1;
					NextMove (dir_x,dir_z);
				}
			} else if (rand == 1) {
				if((int)ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber] >= 0) {
					notFront = 2;
					Move (-1, -1);
				}
				else {
					rand = 0;
					NextMove (dir_x,dir_z);
				}
			}
		}
		else if(notFront == 0 && notNextTo == true) {
			notNextTo = false;
			Move (0, -2);
		}
	}

	IEnumerator WaitPlayerMove(int dir_x, int dir_z){
		ennemy_occupy_attack_x = ennemy_x;
		ennemy_occupy_attack_z = ennemy_z;
		while(wait_ennemy_move == true) {
			if(Bob.GetComponent<Player_move>().EndOfPlayerAnimation == true) {
				AttackMove(dir_x, dir_z);
				break;
			}
			yield return 0;
		}
	}

	void AttackMove(int dir_x, int dir_z) {
		next_ennemy_pos = ((int)ennemy_z + dir_z)%22;
		AttackMoveEnnemy(dir_x, dir_z);
	}

	void AttackMoveEnnemy(int dir_x, int dir_z) {
		ennemyflnumber = (int) Mathf.Round((((int)ennemy_z%22 - 0.25f)/2));
		if (dir_z == -2) {
			if ((int)Mathf.Round (((int)ennemy_z%22 - 2 - 0.25f)/2) < 0)
				nextflnumber = 10;
			else
				nextflnumber = (int)Mathf.Round (((int)ennemy_z%22 - 2 - 0.25f)/2);
		}
		else if (dir_z == -1) {
			if ((int)Mathf.Round (((int)ennemy_z%22 - 1 - 0.25f)/2) < 0) 
				nextflnumber = 10;
			else 
				nextflnumber = (int)Mathf.Round ((((int)ennemy_z%22 - 1 - 0.25f)/2));
		}
		wait_ennemy_move = false;
		float diffY = GetObjectYAttribute(Bob.GetComponent<Player_move>().HexagonesArray[HexagonesArrayZ("Next",dir_z),HexagonesArrayX("Next",dir_x)]) - GetObjectYAttribute(Bob.GetComponent<Player_move>().HexagonesArray[HexagonesArrayZ("Current",dir_z),HexagonesArrayX("Current",dir_x)]);
		if(CanEnnemyMoveNextY(diffY,dir_x,dir_z) &&  NextHexagonOccupation(dir_x,dir_z) == false) {
			if(Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().x == Bob.GetComponent<Player_move>().HexagonesArray[(int)ennemy_z%22,(int) ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber]].GetComponent<Attributes>().x + dir_x && Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().z == Bob.GetComponent<Player_move>().HexagonesArray[(int)ennemy_z%22,(int) ennemy_x + 22 - Bob.GetComponent<Player_move>().TerrainDecal[ennemyflnumber]].GetComponent<Attributes>().z + dir_z) {
				attackPlayer();
				if(dir_x == 0) {
					if(diffY == 0) this.GetComponent<Animator>().SetFloat("Attack",0);
					if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Attack",3);
					if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Attack",6);
				}
				if(dir_x == 1 ) {
					if(diffY == 0) this.GetComponent<Animator>().SetFloat("Attack",2);
					if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Attack",5);
					if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Attack",8);
				}
				if(dir_x == -1 ) {
					if(diffY == 0) this.GetComponent<Animator>().SetFloat("Attack",1);
					if(diffY == 0.5f) this.GetComponent<Animator>().SetFloat("Attack",4);
					if(diffY == -0.5f) this.GetComponent<Animator>().SetFloat("Attack",7);
				}
				this.GetComponent<Animator>().SetBool("MoveAnimationOff",false);
				ennemyTempPosition.z = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].transform.position.z - 0.41f;
				ennemyTempPosition.y = this.transform.position.y + diffY;
				ennemyTempPosition.x = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].transform.position.x;
				float ennemy_x2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().x;
				float ennemy_y2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().y;
				float ennemy_z2 = Bob.GetComponent<Player_move>().HexagonesArray[next_ennemy_pos,(int) ennemy_x + 22 + dir_x - Bob.GetComponent<Player_move>().TerrainDecal[nextflnumber]].GetComponent<Attributes>().z;
				ennemy_x = ennemy_x2;
				ennemy_y = ennemy_y2;
				ennemy_z = ennemy_z2;
				notFront = 1;
				attack = -1;
			}
		}
	}

	void attackPlayer(){

		Bob.GetComponent<Player_health> ().Player_health1 -= 1;
		Handheld.Vibrate();
		if(Bob.GetComponent<Player_health>().Player_health1 <= 0) Bob.transform.GetChild (0).GetComponent<Animator>().SetBool ("OnDeath", true);
		else Bob.transform.GetChild (0).GetComponent<Animator>().SetBool ("OnHit", true);
	}
}
