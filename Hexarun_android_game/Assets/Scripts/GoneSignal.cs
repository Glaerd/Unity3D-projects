using UnityEngine;
using System.Collections;

public class GoneSignal : MonoBehaviour {

	public Texture GuiTexture;
	private bool printGUI;

	void Start() {
		printGUI = true;
		StartCoroutine(Wait());
	}

	IEnumerator Wait() {
		for (int i = 0; i < 5; i++) {
			yield return new WaitForSeconds (0.75f);
			printGUI = !printGUI;
		}
	}

	void OnGUI () {
		if(printGUI) GUI.DrawTexture (ResizeGUI (new Rect (0, 0, 960, 540)), GuiTexture);
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
