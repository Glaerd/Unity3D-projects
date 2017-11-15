using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	public Texture CreditsPage;
	public Texture Next;
	
	void OnGUI() {

		GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), CreditsPage);
		GUI.DrawTexture(ResizeGUI (new Rect (1664, 952, 256, 128)),Next);
	}

	void Update() {

		if(Input.touches.Length > 0) {
			for(int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch (i).phase == TouchPhase.Began && Input.GetTouch (i).position.x > Screen.width/3 && Input.GetTouch (i).position.x < 2*Screen.width/3 && Input.GetTouch (i).position.y > Screen.height/6 && Input.GetTouch (i).position.y < 3*Screen.height/7) {
					Application.OpenURL("https://www.youtube.com/user/OlivierDJPyo");
				}
				if (Input.GetTouch (i).phase == TouchPhase.Began && Input.GetTouch (i).position.x > 3*Screen.width/4 && Input.GetTouch (i).position.y < Screen.height/6) {
					Application.LoadLevel("HomePage");
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