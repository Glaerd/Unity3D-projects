using UnityEngine;
using System.Collections;

public class GameGui : MonoBehaviour {

	public GameObject Bob;
	public Texture heart;
	public Texture unmuted;
	public Texture muted;
	public Texture unpaused;
	public Texture paused;
	public Texture HomePageButton;
	public bool mute = false;
	public bool pause = false;
	public int heartnumber;
	public GUIStyle score_style;
	public GUIStyle score_style2;

	void OnGUI() {

		for(int i = 0; i < heartnumber; i++) {
			GUI.DrawTexture(ResizeGUI(new Rect((10+i*50),10,1920,1080)), heart);
		}
		if(heartnumber > 0){
			Rect score_label_rect = ResizeGUI (new Rect (1600, 10, 7700, 1000));
			GUI.Label (score_label_rect, "Score :",score_style);
			if (score_label_rect.width < 100) score_style.normal.textColor = Color.clear;
			else score_style.normal.textColor = Color.white;
			if (score_label_rect.height < 19) {
				score_style.fontSize = 0;
				score_style2.fontSize = 10;
			} else {
				score_style.fontSize = 20;
				score_style2.fontSize = 20;
			}
			GUI.Label (ResizeGUI (new Rect (1855, 10, 500, 1000)), Bob.GetComponent<Player_move>().score.ToString(), score_style2);
			if(pause == false) GUI.DrawTexture(ResizeGUI(new Rect(1824,108,96*25,108*15)), unpaused);
			else {
				GUI.DrawTexture(ResizeGUI(new Rect(1824,108,96*25,108*15)), paused);
				GUI.DrawTexture(ResizeGUI(new Rect(640,432,640*25,200*15)), HomePageButton);
			}
		}
		if(mute == false) GUI.DrawTexture(ResizeGUI(new Rect(0,108,96*25,108*15)), unmuted);
		else GUI.DrawTexture(ResizeGUI(new Rect(0,108,96*25,108*15)), muted);
	}

	void Update() {

		if(mute) AudioListener.pause = true;
		else AudioListener.pause = false;

		if(pause) {
			Time.timeScale = 0;
			if(Input.touches.Length > 0) {
				for(int i = 0; i < Input.touchCount; i++) {
					if(Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch(i).position.x > Screen.width/3 && Input.GetTouch(i).position.x < 2*Screen.width/3 && Input.GetTouch(i).position.y > 4*Screen.height/10 && Input.GetTouch(i).position.y < 6*Screen.height/10) {
						Application.LoadLevel("HomePage");
					}
				}
			}
		}
		else Time.timeScale = 1;

		if(Input.touches.Length > 0) {
			for(int i = 0; i < Input.touchCount; i++) {
				if(Input.GetTouch(i).phase == TouchPhase.Began && Input.GetTouch(i).position.x < Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) {
					mute = !mute;
				}
				if(Input.GetTouch(i).phase == TouchPhase.Began && Input.GetTouch(i).position.x > 19*Screen.width/20 && Input.GetTouch(i).position.y < 9*Screen.height/10 && Input.GetTouch(i).position.y > 8*Screen.height/10) {
					pause = !pause;
				}
			}
		}

	}

	Rect ResizeGUI(Rect _rect) {
		float FilScreenWidth = _rect.width / 1920;
		float rectWidth = FilScreenWidth * Screen.width / 25;
		float FilScreenHeight = _rect.height / 1080;
		float rectHeight = FilScreenHeight * Screen.height / 15;
		float rectX = (_rect.x / 1920) * Screen.width;
		float rectY = (_rect.y / 1080) * Screen.height;
		
		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}
}
