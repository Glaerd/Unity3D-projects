using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	public Texture Go;
	private bool wait = false;

	void OnGUI() {

		GUI.DrawTexture(ResizeGUI(new Rect(0,0,1920,1080)), Go);

	}

	IEnumerator WaitSeconds(float i) {

		yield return new WaitForSeconds (i);
		wait = true;

	}

	void Update() {

		StartCoroutine (WaitSeconds (0.5f));

		if(Input.touches.Length > 0 && wait) {
			for(int i = 0; i < Input.touchCount; i++) {
				if (Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch (i).position.x < Screen.width && Input.GetTouch(i).position.y < Screen.height) {
					this.GetComponent<Player_move>().enabled = true;
					this.GetComponent<StartGame>().enabled = false;
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
