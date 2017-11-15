using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject Bob;
	public GameObject Hips;
	private float diff1;
	private float diff2;
	private Vector3 EndPoint;
	private Vector3 StartPoint;
	private float lerpMoving;

	// Use this for initialization
	void Start () {
		diff1 = this.transform.position.z - Hips.transform.position.z;
		diff2 = this.transform.position.y - Hips.transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lerpMoving += Time.deltaTime;
		Vector3 temp = Vector3.MoveTowards(StartPoint, EndPoint, lerpMoving * 3f);
		temp.y = temp.y + diff2;
		temp.z = temp.z + diff1;
		this.transform.position = temp;

	}

	public void transformCamera(Vector3 Start, Vector3 End) {

		lerpMoving = 0;
		StartPoint = Start;
		EndPoint = End;

	}
}
