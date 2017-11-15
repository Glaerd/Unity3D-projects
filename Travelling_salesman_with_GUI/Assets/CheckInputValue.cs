using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckInputValue : MonoBehaviour
{

    public void TestValue(int i)
    {
        if(i == 0)
        {
            int j = GameObject.Find("Button").GetComponent<CreateMap>().nbVilles;
            if (transform.GetComponent<InputField>().text != "")
            {
                if (transform.GetComponent<InputField>().text == "-") transform.GetComponent<InputField>().text = "";
                else
                {
                    if (int.Parse(transform.GetComponent<InputField>().text) >= j) transform.GetComponent<InputField>().text = "";
                }
            }
        }
        else
        {
            if (transform.GetComponent<InputField>().text != "")
            {
                if (transform.GetComponent<InputField>().text == "-") transform.GetComponent<InputField>().text = "";
                else
                {
                    if (int.Parse(transform.GetComponent<InputField>().text) >= i) transform.GetComponent<InputField>().text = "";
                }
            }
        }
    }
}