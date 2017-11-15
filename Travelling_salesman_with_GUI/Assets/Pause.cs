using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pause : MonoBehaviour {

    public bool pause = false;

	public void OnClickPause()
    {
        if (!pause)
        {
            pause = true;
            GameObject.Find("pausetext").GetComponent<Text>().enabled = true;
            GameObject.Find("pauseimage").GetComponent<RawImage>().enabled = true;
        }
        else
        {
            pause = false;
            GameObject.Find("pausetext").GetComponent<Text>().enabled = false;
            GameObject.Find("pauseimage").GetComponent<RawImage>().enabled = false;
        }
    }
}
