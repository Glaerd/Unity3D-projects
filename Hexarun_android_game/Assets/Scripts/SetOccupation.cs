using UnityEngine;
using System.Collections;

public class SetOccupation : MonoBehaviour {

	public GameObject Bob;
	public GameObject[] Hexagones;
	public GameObject Ennemies;
	private Attributes hexagonAttributes;
	private bool other_front;
	private bool other_right;
	private bool other_left;
	public Color DarkColor;
	public Color DarkColor1;
	public Color DarkColor2;
	public Color BlueColor;
	public Color BlueColor1;
	public Color BlueColor2;
	public GameObject BlackHexagonGoneGUI;
	public GameObject RedHexagonGoneGUI;
	private Color other_Front_Color;
	private Color other_Right_Color;
	private Color other_Left_Color;
	private int nextflnumber;
	private int hexagonflnumber;


	void Start () {
		Hexagones = GameObject.FindGameObjectsWithTag ("Hexagon");
		Bob = GameObject.Find ("Player");
		Ennemies = GameObject.Find ("Ennemies");
		DarkColor = new Color (0.8f, 0.8f, 0.8f, 1);
		DarkColor1 = new Color (0.4f, 0.4f, 0.4f, 1);
		DarkColor2 = new Color (0.2f, 0.2f, 0.2f, 1);
		BlueColor = new Color (0.6f, 0.6f, 1, 1);
		BlueColor1 = new Color (0.75f, 0.75f, 1, 1);
		BlueColor2 = new Color (0.85f, 0.85f, 1, 1);
		foreach (GameObject hexagon in Hexagones) {
			hexagon.GetComponent<Renderer>().material.color = BlueColor1;
		}
	}

	GameObject NextHexagonAttributes(int dir_z, float hexagonAttr_z, int dir_x, float hexagonAttr_x) {
		int next_hexa_pos_z;
		int next_hexa_pos_x;

		hexagonflnumber = (int) Mathf.Round((((int)hexagonAttr_z%22 - 0.25f)/2));
		if (dir_z == -2) {
			if ((int)Mathf.Round (((int)hexagonAttr_z%22 - 2 - 0.25f)/2) < 0)
				nextflnumber = 10;
			else
				nextflnumber = (int)Mathf.Round (((int)hexagonAttr_z%22 - 2 - 0.25f)/2);
		}
		else if (dir_z == -1) {
			if ((int)Mathf.Round (((int)hexagonAttr_z%22 - 1 - 0.25f)/2) < 0) 
				nextflnumber = 10;
			else 
				nextflnumber = (int)Mathf.Round ((((int)hexagonAttr_z%22 - 1 - 0.25f)/2));
		}

		next_hexa_pos_z = (((int)hexagonAttributes.z + dir_z)%22);

		if ((int)hexagonAttr_x + 22 + dir_x - Bob.GetComponent<Player_move> ().TerrainDecal [nextflnumber] <= -1) {
			next_hexa_pos_x = (int)hexagonAttr_x + 22 - Bob.GetComponent<Player_move> ().TerrainDecal [hexagonflnumber];
			next_hexa_pos_z = (int)hexagonAttr_z%22;
		} else if ((int)hexagonAttr_x + 22 + dir_x - Bob.GetComponent<Player_move> ().TerrainDecal [nextflnumber] >= 45) {
			next_hexa_pos_x = (int)hexagonAttr_x + 22 - Bob.GetComponent<Player_move> ().TerrainDecal [hexagonflnumber];
			next_hexa_pos_z = (int)hexagonAttr_z%22;
		} else
			next_hexa_pos_x = (int)hexagonAttr_x + 22 + dir_x - Bob.GetComponent<Player_move> ().TerrainDecal [nextflnumber];

		return Bob.GetComponent<Player_move> ().HexagonesArray [next_hexa_pos_z,next_hexa_pos_x];
	}

	public void SetTerrainOccupation () {

		Ennemies.GetComponent<Ennemy_Behaviour> ().wait_ennemy_move = false;

		foreach(GameObject hexagon in Hexagones) {
			hexagonAttributes = hexagon.GetComponent<Attributes>();
			hexagonAttributes.occupied = false;
			other_front = false;
			other_left = false;
			other_right = false;
			other_Front_Color = Color.white;
			other_Left_Color = Color.white;
			other_Right_Color = Color.white;

			if(Bob.GetComponent<Player_move>().score <= 200) {
				if(hexagon.GetComponent<Renderer>().material.color != DarkColor && hexagon.GetComponent<Renderer>().material.color != DarkColor1 && hexagon.GetComponent<Renderer>().material.color != DarkColor2) {
					
					GameObject nextfrontAttributes = NextHexagonAttributes(2,hexagonAttributes.z,0,hexagonAttributes.x);
					GameObject nextrightAttributes = NextHexagonAttributes(1,hexagonAttributes.z,-1,hexagonAttributes.x);
					GameObject nextleftAttributes = NextHexagonAttributes(1,hexagonAttributes.z,1,hexagonAttributes.x);
					if(Mathf.Abs(hexagonAttributes.y - nextfrontAttributes.GetComponent<Attributes>().y) >= 1 && hexagonAttributes.x == nextfrontAttributes.GetComponent<Attributes>().x && hexagonAttributes.z == nextfrontAttributes.GetComponent<Attributes>().z - 2){
						other_front = true;
					}
					if(Mathf.Abs(hexagonAttributes.y - nextrightAttributes.GetComponent<Attributes>().y) >= 1 && hexagonAttributes.x == nextrightAttributes.GetComponent<Attributes>().x + 1 && hexagonAttributes.z == nextrightAttributes.GetComponent<Attributes>().z - 1){
						other_left = true;
					}
					if(Mathf.Abs(hexagonAttributes.y - nextleftAttributes.GetComponent<Attributes>().y) >= 1 && hexagonAttributes.x == nextleftAttributes.GetComponent<Attributes>().x - 1 && hexagonAttributes.z == nextleftAttributes.GetComponent<Attributes>().z - 1){
						other_right = true;
					}
					if(Mathf.Abs(hexagonAttributes.y - nextfrontAttributes.GetComponent<Attributes>().y) < 1 && hexagonAttributes.x == nextfrontAttributes.GetComponent<Attributes>().x && hexagonAttributes.z == nextfrontAttributes.GetComponent<Attributes>().z - 2){
						other_Front_Color = nextfrontAttributes.GetComponent<Renderer>().material.color;
					}
					if(Mathf.Abs(hexagonAttributes.y - nextrightAttributes.GetComponent<Attributes>().y) < 1 && hexagonAttributes.x == nextrightAttributes.GetComponent<Attributes>().x + 1 && hexagonAttributes.z == nextrightAttributes.GetComponent<Attributes>().z - 1){
						other_Left_Color = nextrightAttributes.GetComponent<Renderer>().material.color;
					}
					if(Mathf.Abs(hexagonAttributes.y - nextleftAttributes.GetComponent<Attributes>().y) < 1 && hexagonAttributes.x == nextleftAttributes.GetComponent<Attributes>().x - 1 && hexagonAttributes.z == nextleftAttributes.GetComponent<Attributes>().z - 1){
						other_Right_Color = nextleftAttributes.GetComponent<Renderer>().material.color;
					}
					
					if(other_front == true && other_left == true && other_right == true) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor2);
					else if(other_front == true && other_left == true && other_right == false && other_Right_Color == DarkColor2) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor1);
					else if(other_front == true && other_right == true && other_left == false && other_Left_Color == DarkColor2) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor1);
					else if(other_right == true && other_left == true && other_front == false && other_Front_Color == DarkColor2) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor1);
					else if(other_right == false && other_left == false && other_front == true && other_Left_Color == DarkColor2 && other_Right_Color == DarkColor2) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor1);
					else if(other_front == true && other_left == true && other_right == false && other_Right_Color == DarkColor1) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor);
					else if(other_front == true && other_right == true && other_left == false && other_Left_Color == DarkColor1) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor);
					else if(other_right == true && other_left == true && other_front == false && other_Front_Color == DarkColor1) hexagon.GetComponent<Renderer>().material.SetColor("_Color",DarkColor);
				}
			}
			else{
				BlackHexagonGoneGUI.GetComponent<GoneSignal>().enabled = true;
			}
			
			if(hexagon.transform.position.z >= Bob.transform.GetChild(0).transform.position.z - 12 && hexagon.GetComponent<Renderer>().material.color != DarkColor && hexagon.GetComponent<Renderer>().material.color != DarkColor1 && hexagon.GetComponent<Renderer>().material.color != DarkColor2) {

				if(hexagon.GetComponent<Attributes>().y == -0.5f) hexagon.GetComponent<Renderer>().material.color = BlueColor;
				if(hexagon.GetComponent<Attributes>().y == 0) hexagon.GetComponent<Renderer>().material.color = BlueColor1;
				if(hexagon.GetComponent<Attributes>().y == 0.5f) hexagon.GetComponent<Renderer>().material.color = BlueColor2;


				if(Bob.GetComponent<Player_move>().score <= 400) {
					for(int i = 0; i < Ennemies.transform.childCount; i++) {
						if(hexagonAttributes.occupied == false && Mathf.Abs(Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_y - hexagonAttributes.y) < 1  && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_x == hexagonAttributes.x - 1 && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_z == hexagonAttributes.z + 1){
							hexagon.GetComponent<Renderer>().material.SetColor("_Color",new Color(1, 0.56f, 0.56f, 1));
						}
						if(hexagonAttributes.occupied == false && Mathf.Abs(Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_y - hexagonAttributes.y) < 1 && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_x == hexagonAttributes.x + 1 && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_z == hexagonAttributes.z + 1){
							hexagon.GetComponent<Renderer>().material.SetColor("_Color",new Color(1, 0.56f, 0.56f, 1));
						}
						if(hexagonAttributes.occupied == false && Mathf.Abs(Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_y - hexagonAttributes.y) < 1 && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_x == hexagonAttributes.x && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_z == hexagonAttributes.z + 2){
							hexagon.GetComponent<Renderer>().material.SetColor("_Color",new Color(1, 0.56f, 0.56f, 1));
						}
						if(hexagon.GetComponent<Renderer>().material.color != new Color(0.2f, 0.2f, 0.2f, 1) && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_x == hexagonAttributes.x && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_z == hexagonAttributes.z) {
							hexagonAttributes.occupied = true;
							hexagon.GetComponent<Renderer>().material.SetColor("_Color",new Color(1, 0.36f, 0.36f, 1));
						}
						if(Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_occupy_attack_x == hexagonAttributes.x && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_occupy_attack_z == hexagonAttributes.z) {
							hexagonAttributes.occupied = false;
						}
					}
				}
				else {
					RedHexagonGoneGUI.GetComponent<GoneSignal>().enabled = true;
					for(int i = 0; i < Ennemies.transform.childCount; i++) {
						if(Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_x == hexagonAttributes.x && Ennemies.transform.GetChild (i).GetComponent<Ennemy_move>().ennemy_z == hexagonAttributes.z) {
							hexagonAttributes.occupied = true;
						}
					}
				}
			}
			else if(hexagon.transform.position.z < Bob.transform.GetChild(0).transform.position.z - 12 || (hexagon.GetComponent<Renderer>().material.color != DarkColor && hexagon.GetComponent<Renderer>().material.color != DarkColor1 && hexagon.GetComponent<Renderer>().material.color != DarkColor2)){
				if(hexagon.GetComponent<Attributes>().y == -0.5f) hexagon.GetComponent<Renderer>().material.color = BlueColor;
				if(hexagon.GetComponent<Attributes>().y == 0) hexagon.GetComponent<Renderer>().material.color = BlueColor1;
				if(hexagon.GetComponent<Attributes>().y == 0.5f) hexagon.GetComponent<Renderer>().material.color = BlueColor2;
			}
		}
	}
}