using UnityEngine;
using System.Collections;

public class DeathGUI : MonoBehaviour {

	public GameObject Bob;
	public GUIStyle DeathGUIStyle;
	public GUIStyle HighscoreStyle;
	public Texture Border_DeathGUISkin;
	public Texture Background_DeathGUISkin;
	public Texture Spot_DeathGUISkin;
	public Texture BronzeMedal_DeathGUISkin;
	public Texture SilverMedal_DeathGUISkin;
	public Texture GoldMedal_DeathGUISkin;
	public Texture PlayAgain_DeathGUISkin;
	public GUITexture PlayAgain;
	public Texture GameOver_DeathGUISkin;
	public Texture Highscore;
	public GUISkin none_DeathGUISkin;
	public bool highscore = false;
	public int score_type = 0;
	public bool test = false;
	private float score_temp = 0;
	private int i;
	private bool norepeat = false;
	private float color_r;
	private float color_g;
	private float color_b;

	
	void Start () {
		DeathGUIStyle.fontSize = 50;
		HighscoreStyle.fontSize = 15;
		score_temp = 0;
		if (PlayerPrefs.GetInt ("Highscore") <= Bob.GetComponent<Player_move> ().score) highscore = true;
	}

	void OnGUI(){
		if (Bob.GetComponent<Player_health>().activeGui && Bob.transform.GetChild(0).GetComponent<Animator>().GetBool("OnDeath") == false) {
			GUI.DrawTexture (ResizeGUI (new Rect (363, this.transform.position.y, 1200, 700)), Background_DeathGUISkin);
			if(score_type == 1 || (Bob.GetComponent<Player_move>().score >= 150 && Bob.GetComponent<Player_move>().score < 300)) {
				GUI.DrawTexture (ResizeGUI (new Rect (380, 70 + this.transform.position.y, 600, 350)), Spot_DeathGUISkin);
				GUI.DrawTexture (ResizeGUI (new Rect (380, 30 + this.transform.position.y + flyLook(5,3), 600, 350)), BronzeMedal_DeathGUISkin);
			}
			if(score_type == 2 || (Bob.GetComponent<Player_move>().score >= 300 && Bob.GetComponent<Player_move>().score < 500)) {
				GUI.DrawTexture (ResizeGUI (new Rect (380, 70 + this.transform.position.y, 600, 350)), Spot_DeathGUISkin);
				GUI.DrawTexture (ResizeGUI (new Rect (388, 30 + this.transform.position.y + flyLook(5,3), 600, 350)), SilverMedal_DeathGUISkin);
			}
			if(score_type == 3 || Bob.GetComponent<Player_move>().score >= 500) {
				GUI.DrawTexture (ResizeGUI (new Rect (380, 70 + this.transform.position.y, 600, 350)), Spot_DeathGUISkin);
				GUI.DrawTexture (ResizeGUI (new Rect (385, 30 + this.transform.position.y + flyLook(5,3), 600, 350)), GoldMedal_DeathGUISkin);
			}
			GUIUtility.RotateAroundPivot(25, new Vector2(Screen.width/2,Screen.height/2));
			GUI.DrawTexture (ResizeGUI (new Rect (450, 420 + this.transform.position.y, 700, 100)), GameOver_DeathGUISkin);
			GUIUtility.RotateAroundPivot(-25, new Vector2(Screen.width/2,Screen.height/2));
			GUI.Label(ResizeGUI(new Rect (1150, 300 + this.transform.position.y, 500, 300)),score_temp.ToString(), DeathGUIStyle);
			GUI.Label(ResizeGUI(new Rect (1100, 40 + this.transform.position.y, 500, 300)),"Highscore : " + PlayerPrefs.GetInt("Highscore").ToString(), HighscoreStyle);
			GUI.DrawTexture (ResizeGUI (new Rect (363, this.transform.position.y, 1200, 700)), Border_DeathGUISkin);
			if(highscore){
				PlayerPrefs.SetInt("Highscore",(int) Bob.GetComponent<Player_move>().score);
				GUI.DrawTexture (ResizeGUI(new Rect (900, 100 + this.transform.position.y + flyLook(8,5), 600, 350)), Highscore);
			}
			GUI.DrawTexture (ResizeGUI (new Rect (1000, 570 + this.transform.position.y, 500, 100)), PlayAgain.texture);
		}
	}

	void Update () {

		if(Bob.GetComponent<Player_health>().activeGui && Bob.transform.GetChild(0).GetComponent<Animator>().GetBool("OnDeath") == false && norepeat == false){
			InvokeRepeating("changeColor", 0, 0.15f);
			InvokeRepeating("changeScore", 1, 1/Bob.GetComponent<Player_move>().score);
			this.GetComponent<Animator>().SetBool("DeathAnimation", true);
			norepeat = true;
		}

		if(Input.touchCount > 0) {

			for(int i = 0; i < Input.touchCount; i++) {
				if(Input.GetTouch(i).phase==TouchPhase.Began) {
					if(ResizeGUIForButton (new Rect (1000, 570 + this.transform.position.y, 500, 100)).Contains(Input.GetTouch(i).position)) {
					 Application.LoadLevel("HomePage");
					}
				}
			}
		}
	}

	Rect ResizeGUI(Rect _rect)
	{
		float FilScreenWidth = _rect.width / 1920;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / 1080;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / 1920) * Screen.width;
		float rectY = (_rect.y / 1080) * Screen.height;
		
		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}

	Rect ResizeGUIForButton(Rect _rect)
	{
		float FilScreenWidth = _rect.width / 1920;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / 1080;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / 1920) * Screen.width;
		float rectY = Screen.height - (_rect.y / 1080) * Screen.height - rectHeight;
		
		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}

	float flyLook(float multiplier, float timer){
		
		float x = Time.time * timer;
		return multiplier * Mathf.Sin (x);
		
	}
	
	void changeScore() {
		
		if(score_temp < Bob.GetComponent<Player_move>().score) {
			score_temp++;
		}
		
	}
	
	void changeColor() {
		color_b = Random.Range(0,255);
		color_g = Random.Range(0,255);
		color_r = Random.Range(0,255);
		DeathGUIStyle.normal.textColor = new Color(color_r/255,color_g/255,color_b/255,1);
	}
}
