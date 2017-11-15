using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {


	public Texture BackGroundTuto;
	public Texture Black;
	public Texture EndTuto;
	public Texture Ennemies1;
	public Texture Ennemies2;
	public Texture Goal;
	public Texture How;
	public Texture Jump1;
	public Texture Jump2;
	public Texture Left;
	public Texture Right;
	public Texture Stop;
	public Texture Traps1;
	public Texture Traps2;
	public Texture Next;
	public Texture Previous;
	public Texture Loading;
	public Texture Play;
	public int nextPage;

	void Start() {
		nextPage = 0;
	}

	void OnGUI() {

		GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Black);

		if (nextPage == 0) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),BackGroundTuto);
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Goal);
		}
		else if (nextPage == 1) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),How);
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Left);
		}
		else if (nextPage == 2) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),How);
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Right);
		}
		else if (nextPage == 3) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),How);
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Stop);
		}
		else if (nextPage == 4) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Jump1);
		}
		else if (nextPage == 5) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Jump2);
		}
		else if (nextPage == 6) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Ennemies1);
		}
		else if (nextPage == 7) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Ennemies2);
		}
		else if (nextPage == 8) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Traps1);
		}
		else if (nextPage == 9) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),Traps2);
		}
		else if (nextPage == 10) {
			GUI.DrawTexture(ResizeGUI (new Rect (0, 0, 1920, 1080)),EndTuto);
		}

		if(nextPage != 11 && nextPage >= 0) {
			if(nextPage != 10) GUI.DrawTexture(ResizeGUI (new Rect (1664, 952, 256, 128)),Next);
			if(nextPage == 10) GUI.DrawTexture(ResizeGUI (new Rect (1664, 952, 256, 128)),Play);
			GUI.DrawTexture(ResizeGUI (new Rect (0, 952, 256, 128)),Previous);
		}

		if(nextPage == 11 || nextPage < 0)
			GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), Loading);

	}

	void Update () {

		if (nextPage < 0) Application.LoadLevel ("HomePage");

		if(Input.touches.Length > 0 && nextPage < 11) {
			for(int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch (i).position.x > 3*Screen.width/4 && Input.GetTouch (i).position.y < Screen.height/6) {
					nextPage += 1;
					if(nextPage == 11) {
						Application.LoadLevel("Hexarun");
					}
				}
				if (Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch (i).position.x < Screen.width/4 && Input.GetTouch (i).position.y < Screen.height/6) {
					nextPage -= 1;
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

}
