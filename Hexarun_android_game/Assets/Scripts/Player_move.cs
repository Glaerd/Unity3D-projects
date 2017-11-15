using UnityEngine;
using System.Collections;

public class Player_move : MonoBehaviour {

	public GameObject Floor;
	public GameObject[] Floors;
	public GameObject[] Hexagones;
	public GameObject[,] HexagonesArray = new GameObject[22,45];
	public bool MoveAnimationOff = false;
	public bool IdleAnimationOn = false;
	public bool MemoryInputWhileAnimation = false;
	public GameObject Spawn;
	public GameObject Ennemies;
	public bool EndOfPlayerAnimation = false;
	private GameObject Halo;
	private GameObject Bob;
	public GameObject MainCamera;
	public GameObject PointLight;
	public GameObject Snow;
	public GameObject Dust;
	public GameObject Player_Collider;
	public bool WaitForEnnemyHit = false;
	public float player_x = 0;
	public float player_y = 0;
	public float player_z = 4;
	private Attributes hexagonAttributes;
	private Attributes previoushexagonAttributes;
	private Attributes hexagonAttributes2;
	private Vector3 playerTempPosition;
	private Vector3 hexagonTempPosition;
	private bool canGo = true;
	private int floorNumber = 0;
	private int hexagonCounter = 0;
	private int left = 0;
	private int right = 0;
	private float[] hexagonTempXAttributes_row0 = new float[23];
	private float[] hexagonTempXAttributes_row1 = new float[22];
	private float x;
	private float attr_x;
	public float score = 0;
	public Color DarkColor;
	public Color DarkColor1;
	public Color DarkColor2;
	public Color other_Front_Color;
	public Color other_Left_Color;
	public Color other_Right_Color;
	private bool other_front;
	private bool other_left;
	private bool other_right;
	private bool doItOnceAMove = true;
	public int decal = 0;
	public int[] TerrainDecal = new int[11];
	public int playerflnumber;
	public int nextflnumber;
	public int player_pos;
	public int memory = 0;
	//private int[,] hill_matrix;

	void Start() {
		Hexagones = GameObject.FindGameObjectsWithTag ("Hexagon");
		Bob = GameObject.Find ("Bob");
		Halo = GameObject.Find ("Halo");
		for(int flnumber = 0; flnumber < 11; flnumber++) {
			HexagonesArray[2*flnumber,44] = Floors[flnumber].transform.GetChild (0).GetChild(22).gameObject;
			for(int row = 0; row < 2; row++){
				for(int i = 0; i < 22; i++) {
					HexagonesArray[2*flnumber+row,2*i + row] = Floors[flnumber].transform.GetChild (row).GetChild (i).gameObject;
					//Floors[flnumber].transform.GetChild (row).GetChild (i).renderer.material.color = Floor.GetComponent<SetOccupation>().BlueColor1;
				}
			}
		}
		playerTempPosition = Bob.transform.position;
		hexagonTempPosition = new Vector3(0,0,0);
		previoushexagonAttributes = Spawn.GetComponent<Attributes> ();
		hexagonTempPosition.z = Floors [floorNumber].transform.position.z;
		hexagonTempPosition.y = Floors [floorNumber].transform.position.y;
		hexagonTempPosition.x = Floors [floorNumber].transform.position.x;
		for(int t=0;t<23;t++) {
			hexagonTempXAttributes_row0[t] = -22 + 2*t;
		}
		for(int t=0;t<22;t++) {
			hexagonTempXAttributes_row1[t] = -21 + 2*t;
		}
		this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",0);
	}

	void FixedUpdate() {
		if (!canGo) {
			Halo.SetActive (true);
			Halo.transform.position = playerTempPosition;
		}
		else Halo.SetActive (false);
	}

	void LateUpdate() {
		this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",0);
	}

	void Update () {


		if(Input.touches.Length > 0) {
			for(int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch(i).phase == TouchPhase.Began && Input.GetTouch(i).position.x < Screen.width/3 && MemoryInputWhileAnimation) memory = 1;
				if (Input.GetTouch(i).position.x < Screen.width/3 && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false && (Input.GetTouch(i).position.x > 19*Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false && (Input.GetTouch(i).position.x < Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false) {
					Move(-1,1);//#gauche
				}
				if (Input.GetTouch(i).position.x >= Screen.width/3 && Input.GetTouch(i).position.x < 2*Screen.width/3 && canGo && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false && (Input.GetTouch(i).position.x > 19*Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false && (Input.GetTouch(i).position.x < Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false) {
					//#Stop
				}
				if (Input.GetTouch(i).phase == TouchPhase.Began && Input.GetTouch(i).position.x >= 2*Screen.width/3 && MemoryInputWhileAnimation) memory = 2;
				if (Input.GetTouch(i).position.x >= 2*Screen.width/3 && canGo && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false && (Input.GetTouch(i).position.x > 19*Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false && (Input.GetTouch(i).position.x < Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) == false) {
					Move(1,1);
				}
			}
		}
		else {
			if (Input.GetKey("q") && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				Move(-1,1);			
			}
			else if (Input.GetKeyDown("q") && MemoryInputWhileAnimation) memory = 1;
			else if (Input.GetKey("s") && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				//#Stop
			}
			else if (Input.GetKey("d") && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				Move(1,1);			
			}
			else if (Input.GetKeyDown("d") && MemoryInputWhileAnimation) memory = 2;
			else if (memory == 1 && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				Move (-1,1);
			}
			else if (memory == 2 && canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				Move (1,1);
			}
			else if (canGo && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnDeath") == false && this.transform.GetChild (0).GetComponent<Animator>().GetBool("OnHit") == false && WaitForEnnemyHit == false) {
				Move(0,2);			
			}
		}
		
		if (MoveAnimationOff) {
			Bob.transform.position = playerTempPosition;
			this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("MoveAnimationOff",true);
			MoveAnimationOff = false;
			canGo = !canGo;
			MemoryInputWhileAnimation = false;
			EndOfPlayerAnimation = true;
			doItOnceAMove = true;
		}
	}

	void Move(int dir_x, int dir_z) {
		memory = 0;
		if (doItOnceAMove && this.gameObject.transform.GetChild (0).GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).nameHash == 1432961145) {
			EndOfPlayerAnimation = false;
			if ((int) player_z - 22 * decal + dir_z >= 22) {
				decal++;
			}

			if ((int) player_z - 22 * decal == -1) {
				player_pos = 21;
				MovePlayer (dir_x, dir_z);
			}
			else if ((int) player_z - 22 * decal == -2) {
				player_pos = 20;
				MovePlayer (dir_x, dir_z);
			}
			else {
				player_pos = (int)player_z - 22 * decal;
				MovePlayer (dir_x, dir_z);
			}
		}
	}

	bool CanPlayerMoveNextY(float y, int dir_x, int dir_z){
		if (Mathf.Abs (y) < 1)
			return HexagonesArray [(int)player_z - 22 * decal + dir_z, (int)player_x + 22 + dir_x - TerrainDecal [nextflnumber]].GetComponent<Attributes> ().y == HexagonesArray [player_pos, (int)player_x + 22 - TerrainDecal[playerflnumber]].GetComponent<Attributes> ().y + y;
		else
			return false;
	}

	int HexagonesArrayZ(string state,int dir_z) {
		if (state == "Next")
			return (int)player_z - 22 * decal + dir_z;
		else if (state == "Current")
			return player_pos;
		else
			return 0;
	}

	int HexagonesArrayX(string state,int dir_x) {
		if (state == "Next")
			return (int)player_x + 22 + dir_x - TerrainDecal[nextflnumber];
		else if (state == "Current")
			return (int)player_x + 22 - TerrainDecal[playerflnumber];
		else
			return 0;
	}

	float GetObjectYAttribute(GameObject thisObject) {
		return thisObject.GetComponent<Attributes> ().y;
	}

	bool NextHexagonOccupation(int dir_x, int dir_z) {
		return HexagonesArray [(int)player_z - 22 * decal + dir_z, (int)player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes> ().occupied;
	}

	void MovePlayer(int dir_x, int dir_z) {

		playerflnumber = (int) Mathf.Round((player_pos - 0.25f)/2);
		if (dir_z == 2) {
			if ((int)Mathf.Round ((player_pos + 2 - 0.25f) / 2) > 10)
				nextflnumber = 0;
			else
				nextflnumber = (int)Mathf.Round ((player_pos + 2 - 0.25f) / 2);
		}
		else if (dir_z == 1) {
			if((int)Mathf.Round ((player_pos + 1 - 0.25f) / 2) > 10) nextflnumber = 0;
			else nextflnumber = (int)Mathf.Round ((player_pos + 1 - 0.25f) / 2);
		}
		float diffY = GetObjectYAttribute(HexagonesArray[HexagonesArrayZ("Next",dir_z),HexagonesArrayX("Next",dir_x)]) - GetObjectYAttribute(HexagonesArray[HexagonesArrayZ("Current",dir_z),HexagonesArrayX("Current",dir_x)]);
		if(CanPlayerMoveNextY(diffY,dir_x,dir_z) &&  NextHexagonOccupation(dir_x,dir_z) == false) {
			if(HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>().x == HexagonesArray[player_pos,(int) player_x + 22 - TerrainDecal[playerflnumber]].GetComponent<Attributes>().x + dir_x && HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>().z == HexagonesArray[player_pos,(int) player_x + 22 - TerrainDecal[playerflnumber]].GetComponent<Attributes>().z + dir_z) {
				doItOnceAMove = false;
				instantiateTerrain(dir_x,HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>());
				this.GetComponent<AudioSource>().Play();
				canGo = !canGo;
				GameObject.Find("Ennemies").GetComponent<Ennemy_Behaviour>().playermoved = true;
				if(dir_x == 0) {
					if(diffY == 0) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",1);
					if(diffY == 0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",4);
					if(diffY == -0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",7);
					score += 2;
				}
				if(dir_x == 1 ) {
					if(diffY == 0) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",2);
					if(diffY == 0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",5);
					if(diffY == -0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",8);
					score += 1;
				}
				if(dir_x == -1 ) {
					if(diffY == 0) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",3);
					if(diffY == 0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",6);
					if(diffY == -0.5f) this.gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Move",9);
					score += 1;
				}

				this.transform.GetChild(0).GetComponent<Animator>().SetBool("MoveAnimationOff",false);
				playerTempPosition.z = HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].transform.position.z + 0.41f;
				playerTempPosition.y = Bob.transform.position.y + diffY;
				playerTempPosition.x = HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].transform.position.x;
				float player_x2 = HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>().x;
				float player_y2 = HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>().y;
				float player_z2 = HexagonesArray[(int) player_z - 22*decal + dir_z,(int) player_x + 22 + dir_x - TerrainDecal[nextflnumber]].GetComponent<Attributes>().z;
				player_x = player_x2;
				player_y = player_y2;
				player_z = player_z2;
				MainCamera.GetComponent<FollowPlayer>().transformCamera(this.transform.GetChild(0).transform.position, playerTempPosition);
				PointLight.GetComponent<FollowPlayer> ().transformCamera (this.transform.GetChild(0).transform.position, playerTempPosition);
				Snow.GetComponent<FollowPlayer> ().transformCamera (this.transform.GetChild(0).transform.position, playerTempPosition);
				Dust.GetComponent<FollowPlayer> ().transformCamera (this.transform.GetChild(0).transform.position, playerTempPosition);
				Player_Collider.GetComponent<FollowPlayer> ().transformCamera (this.transform.GetChild(0).transform.position, playerTempPosition);
			}
		}
		else {
			if(dir_z == 2 && (int)player_z - 22 * decal + dir_z == 0) decal--;
		}
	}

	void instantiateTerrain(int dir, Attributes hexagonAttributes) {

		if(floorNumber == 11) {
			floorNumber = 0;
		}

		if(dir == -1) {
			if(left == 1) left = 0;
			left = 1;
		}

		if(dir == 1) {
			if(right == 1) right = 0;
			right = 1;
		}

		if(left == 0 && right == 0 && dir == 0 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 0;
			attr_x = hexagonAttributes.GetComponent<Attributes> ().x;
			left = 0;
			right = 0;

		}

		if (left == 1 && right == 0 && dir == 0 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 0;
			attr_x = hexagonAttributes.GetComponent<Attributes> ().x + 1;
			left = 1;
			right = 0;

		}

		if (left == 0 && right == 1 && dir == 0 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 0;
			attr_x = hexagonAttributes.GetComponent<Attributes> ().x - 1;
			left = 0;
			right = 1;

		}

		if (left == 0 && right == 1 && dir == 1 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = -3.0086f;
			attr_x = previoushexagonAttributes.GetComponent<Attributes> ().x + 1;
			left = 0;
			right = 0;

		}

		if(right == 1 && dir == -1 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 0;
			attr_x = hexagonAttributes.GetComponent<Attributes> ().x;
			left = 0;
			right = 0;

		}

		if (left == 1 && right == 0 && dir == -1 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 3.0086f;
			attr_x = previoushexagonAttributes.GetComponent<Attributes> ().x - 1;
			left = 0;
			right = 0;

		}

		if(left == 1 && dir == 1 && previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {

			x = 0;
			attr_x = hexagonAttributes.GetComponent<Attributes> ().x;
			left = 0;
			right = 0;

		}

		if(previoushexagonAttributes.gameObject.transform.parent.transform.parent != hexagonAttributes.gameObject.transform.parent.transform.parent) {
			float r1;
			Vector3 Vr1;
			for(int row = 0; row <= 1; row++) {
				for(hexagonCounter = 0; hexagonCounter < 22; hexagonCounter++) {
					SetBiome(row,hexagonCounter);
					if(row == 0) Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).GetComponent<Attributes> ().x = hexagonTempXAttributes_row0[hexagonCounter] + attr_x;
					if(row == 1) Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).GetComponent<Attributes> ().x = hexagonTempXAttributes_row1[hexagonCounter] + attr_x;
					r1 = (float) Random.Range(-1, 2)/2;
					Vr1 = new Vector3 (Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).transform.position.x, r1 - 1, Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).transform.position.z);
					Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).GetComponent<Attributes> ().y = r1;
					Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).transform.position = Vr1;
					Floors [floorNumber].transform.GetChild (row).GetChild (hexagonCounter).GetComponent<Attributes> ().z += 22;
				}
			}
			Floors [floorNumber].transform.GetChild (0).GetChild (22).GetComponent<Attributes> ().x = hexagonTempXAttributes_row0[22] + attr_x;
			r1 = (float) Random.Range(-1, 2)/4;
			Vr1 = new Vector3 (Floors [floorNumber].transform.GetChild (0).GetChild (22).transform.position.x,r1 - 1,Floors [floorNumber].transform.GetChild (0).GetChild (22).transform.position.z);
			SetBiome(0,22);
			Floors [floorNumber].transform.GetChild (0).GetChild (22).GetComponent<Attributes> ().y = r1;
			Floors [floorNumber].transform.GetChild (0).GetChild (22).transform.position = Vr1;
			Floors [floorNumber].transform.GetChild (0).GetChild (22).GetComponent<Attributes> ().z += 22;
			hexagonTempPosition.x = hexagonTempPosition.x + x;
			hexagonTempPosition.z = Floors [floorNumber].transform.position.z - 19.109469f;
			Floors [floorNumber].transform.position = hexagonTempPosition;
			Floors [floorNumber].transform.GetChild (0).GetComponent<Animator> ().SetBool ("Active", true);
			Floors [floorNumber].transform.GetChild (1).GetComponent<Animator> ().SetBool ("Active", true);
			TerrainDecal[floorNumber] = (int) attr_x;
			Ennemies.GetComponent<Ennemy_Behaviour>().rowMoved (floorNumber);
			Floor.GetComponent<SetPathDeny>().SetPathDenial(floorNumber);
			floorNumber++;
		}

		previoushexagonAttributes = hexagonAttributes;
	}

	void SetBiome(int row, int hexaCounter) {

		//if(Random.Range (0,100) > 95 && !Floors[floorNumber].transform.GetChild (row).GetChild (hexagonCounter).GetComponent<Attributes> ().isOnBiome) {
			//hill_matrix = new int[Random.Range (10, 100),Random.Range(10, 100)];
		//}		
	}

}