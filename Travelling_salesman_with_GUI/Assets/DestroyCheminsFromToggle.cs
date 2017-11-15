using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestroyCheminsFromToggle : MonoBehaviour {

    public void Effacer(string name)
    {
        GameObject temp = GameObject.Find(name);
        if(transform.name == "greenChemins")
        {
            foreach (Transform go in temp.transform)
            {
                go.GetComponent<LineRenderer>().enabled = GetComponent<Toggle>().isOn;
            }
        }
        else
        {
            foreach (Transform go in temp.transform)
            {
                Destroy(go.gameObject);
            }
        }

    }
}
