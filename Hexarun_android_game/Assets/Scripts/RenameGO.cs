using UnityEngine;
using System.Collections;

public class RenameGO : MonoBehaviour {
	
	private int i = 0;
	private int j = 0;
	private int k = 0;
	
	void Start ()
	{
		while(j<=10){
			if(i==0) {
				while(k<=22){
					Transform Child = this.transform.GetChild (j).GetChild (i).GetChild(k).GetComponent<Transform>();
					Child.name = Child.name.Insert(10, "_".Insert(1,j.ToString().Insert(j.ToString().Length,"_".Insert(1, i.ToString().Insert(1,"_".Insert(1, k.ToString()))))));
					k++;
				}
				k = 0;
				i = 1;
			}
			if(i==1) {
				while(k<=21){
					Transform Child = this.transform.GetChild (j).GetChild (i).GetChild(k).GetComponent<Transform>();
					Child.name = Child.name.Insert(10, "_".Insert(1,j.ToString().Insert(j.ToString().Length,"_".Insert(1, i.ToString().Insert(1,"_".Insert(1, k.ToString()))))));
					k++;
				}
				k = 0;
				i = 0;
			}
			j++;
		}
	}
}