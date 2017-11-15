using UnityEngine;
using System.Collections;

public class resizeimagetoscreen : MonoBehaviour {

	public Texture GuiTexture;
	public Texture Loading;
	public Texture TapToStart;
	public Texture Credits;
	public Texture HowTo;
	public Texture CreditsPage;
	public bool start = false;
	private bool isLoading = false;

	/*void Start() {

		PlayerPrefs.SetInt ("Tutorial", 0);
		PlayerPrefs.SetInt ("Highscore", 0);

	}*/

	void OnGUI()
	{
		GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), GuiTexture);
		GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), TapToStart);
		GUI.DrawTexture(ResizeGUI(new Rect(5*1920/6,0,1920/6,108)), Credits);
		GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920/6,108)), HowTo);
		if(isLoading) GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), Loading);
	}

	void Update() {

		if(Input.touches.Length > 0) {
			if (Input.GetTouch(0).phase == TouchPhase.Began && start == false) {
				start = true;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended && start == true) {
				if((Input.GetTouch(0).position.x > Screen.width/6 && Input.GetTouch(0).position.x < 5*Screen.width/6 && Input.GetTouch(0).position.y < Screen.height) || (Input.GetTouch(0).position.x < Screen.width && Input.GetTouch(0).position.y < 9*Screen.height/10)) {
					isLoading = true;
					if(PlayerPrefs.GetInt("Tutorial") == 0) {
						PlayerPrefs.SetInt("Tutorial",1);
						Application.LoadLevel("Tutorial");
					}
					else if(PlayerPrefs.GetInt("Tutorial") == 1) Application.LoadLevel("Hexarun");
				}
				else if(Input.GetTouch(0).position.x < Screen.width/6 && Input.GetTouch(0).position.y > 9*Screen.height/10) {
					isLoading = true;
					if(PlayerPrefs.GetInt("Tutorial") == 0) PlayerPrefs.SetInt("Tutorial",1);
					Application.LoadLevel("Tutorial");
				}
				else if(Input.GetTouch(0).position.x > 5*Screen.width/6 && Input.GetTouch(0).position.y > 9*Screen.height/10) {
					isLoading = true;
					Application.LoadLevel("Credits");
				}
			}
		}

	}

	Rect ResizeGUI(Rect _rect) {
		float FilScreenWidth = _rect.width / 1920;
		float rectWidth = FilScreenWidth * Screen.width;
		float FilScreenHeight = _rect.height / 1080;
		float rectHeight = FilScreenHeight * Screen.height;
		float rectX = (_rect.x / 1920) * Screen.width;
		float rectY = (_rect.y / 1080) * Screen.height;
		
		return new Rect(rectX,rectY,rectWidth,rectHeight);
	}
}