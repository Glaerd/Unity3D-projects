using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateMap : MonoBehaviour {

    public int nbVilles;
    public GameObject VillePrefab;
    private GameObject Villes;
    private GameObject Chemins;
    private GameObject Cache;
    private GameObject AffChem;
    private GameObject ArcType;
    private GameObject View;
    private GameObject Content;
    private bool block = false;

	public void MapGenerator(int type) {

        GameObject.Find("Button (1)").GetComponent<AStar>().StopAStar();

        Villes = GameObject.Find("Villes");
        Chemins = GameObject.Find("Chemins");
        Cache = GameObject.Find("Cache");
        AffChem = GameObject.Find("Toggle1");
        ArcType = GameObject.Find("ArcType");
        View = GameObject.Find("View");
        Content = GameObject.Find("Content");
        Content.GetComponent<RectTransform>().sizeDelta = new Vector2(-10, Screen.height);
        foreach (Transform child in Villes.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Chemins.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("GRP").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("CheminsAffiche").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("CheminsMST").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in GameObject.Find("AStarContent").transform)
        {
            Destroy(child.gameObject);
        }

        if (type == 1 || block == true) if(Cache.transform.GetChild(4).GetComponent<InputField>().text != "") nbVilles = int.Parse(GameObject.Find("Cache").transform.GetChild(4).GetComponent<InputField>().text);
        if(type == 0 & block == false)
        {
            if(GameObject.Find("Input").transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text != "")
            {
                View.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                View.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                View.GetComponent<RectTransform>().localPosition = new Vector3(0, View.GetComponent<RectTransform>().localPosition.y, View.GetComponent<RectTransform>().localPosition.z);
                nbVilles = int.Parse(GameObject.Find("Input").transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text);
                block = true;
            }
            else
            {
                GameObject.Find("Input").transform.GetChild(0).transform.GetChild(2).GetComponent<Image>().color = Color.red;
                return;
            }
        }
        
        int i;
        for(i = 0; i < nbVilles; i++)
        {
            GameObject temp_ville = Instantiate(VillePrefab);
            temp_ville.transform.SetParent(Villes.transform);
            temp_ville.name = i.ToString();
            float temp = (float)i/nbVilles;
            if(ArcType.GetComponent<InputField>().text == "2") temp_ville.GetComponent<RectTransform>().localPosition = new Vector3(((-Screen.width)/2 - Mathf.Cos(2*Mathf.PI*temp)*(Screen.width)/2)*0.8f/2, ((Screen.height)/2 + Mathf.Sin(2*Mathf.PI*temp)*(Screen.height)/2)*0.8f/2, temp_ville.GetComponent<RectTransform>().localPosition.z);
            else temp_ville.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range((float)-Screen.width*0.8f,0)/2, Random.Range(0,(float)Screen.height*0.8f)/2, temp_ville.GetComponent<RectTransform>().localPosition.z);
            //while (!TestVillePosition(temp_ville.transform, i)) temp_ville.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range((float)-Screen.width / 100, (float)Screen.width / 100), Random.Range((float)-Screen.height / 100, (float)Screen.height / 100), 0);
            GameObject num_ville = new GameObject();
            num_ville.transform.parent = temp_ville.transform;
            num_ville.layer = 5;
            num_ville.transform.position = temp_ville.transform.position;
            Text num = num_ville.AddComponent<Text>();
            num.text = i.ToString();
            num.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            num.alignment = TextAnchor.MiddleCenter;
            num.color = Color.blue;
            num.GetComponent<RectTransform>().sizeDelta = new Vector2(25,25);
            num.GetComponent<RectTransform>().localScale = new Vector3(0.04f, 0.04f, 1);
        }
        StartCoroutine(WaitSeconds(0.01f,0));
    }

    private bool TestVillePosition(Transform positionVille, int i)
    {
        int j;
        for (j = 0; j < i; j++)
        {
            if (positionVille.position == Villes.transform.GetChild(j).transform.position)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator WaitSeconds(float k, int code)
    {
        yield return new WaitForSeconds(k);
        if (code == 0) CreateChemin();
        if (code == 1) AfficheChemin();
    }

    void CreateChemin()
    {
        int i;
        for (i = 0; i < nbVilles; i++)
        {
            int j;
            for (j = i; j < nbVilles; j++)
            {
                if (i != j)
                {
                    GameObject temp_chemin = new GameObject();
                    temp_chemin.transform.parent = Chemins.transform;
                    temp_chemin.name = i.ToString() + " - " + j.ToString();
                    LineRenderer lineRenderer = temp_chemin.AddComponent<LineRenderer>();
                    CheminValues length_value = temp_chemin.AddComponent<CheminValues>();
                    lineRenderer.SetColors(Color.red, Color.red);
                    lineRenderer.SetWidth(0.05F, 0.05F);
                    lineRenderer.SetPosition(0, new Vector3(Villes.transform.GetChild(i).transform.position.x, Villes.transform.GetChild(i).transform.position.y, Villes.transform.GetChild(i).transform.position.z + 10));
                    lineRenderer.SetPosition(1, new Vector3(Villes.transform.GetChild(j).transform.position.x, Villes.transform.GetChild(j).transform.position.y, Villes.transform.GetChild(j).transform.position.z + 10));
                    if (ArcType.GetComponent<InputField>().text == "0") length_value.length = Mathf.Sqrt(Mathf.Pow(Villes.transform.GetChild(i).transform.position.x - Villes.transform.GetChild(j).transform.position.x, 2) + Mathf.Pow(Villes.transform.GetChild(i).transform.position.y - Villes.transform.GetChild(j).transform.position.y, 2));
                    else if (ArcType.GetComponent<InputField>().text == "1") length_value.length = Mathf.Sqrt(Mathf.Pow(Villes.transform.GetChild(i).transform.position.x - Villes.transform.GetChild(j).transform.position.x, 2) + Mathf.Pow(Villes.transform.GetChild(i).transform.position.y - Villes.transform.GetChild(j).transform.position.y, 2)) * Random.Range(1f, 1.25f);
                    else if (ArcType.GetComponent<InputField>().text == "2") length_value.length = Random.Range(1f, 10f);
                    else length_value.length = Mathf.Sqrt(Mathf.Pow(Villes.transform.GetChild(i).transform.position.x - Villes.transform.GetChild(j).transform.position.x, 2) + Mathf.Pow(Villes.transform.GetChild(i).transform.position.y - Villes.transform.GetChild(j).transform.position.y, 2));
                    length_value.sommetA = i;
                    length_value.sommetB = j;
                    Material redUnlitMat = new Material(Shader.Find("Unlit/Color"));
                    redUnlitMat.color = Color.red;
                    lineRenderer.material = redUnlitMat;
                    GameObject num_chemin = new GameObject();
                    num_chemin.transform.parent = temp_chemin.transform;
                    num_chemin.layer = 5;
                    num_chemin.transform.position = new Vector3((Villes.transform.GetChild(i).transform.position.x + Villes.transform.GetChild(j).transform.position.x) / 2, (Villes.transform.GetChild(i).transform.position.y + Villes.transform.GetChild(j).transform.position.y) / 2, (Villes.transform.GetChild(i).transform.position.z + Villes.transform.GetChild(j).transform.position.z) / 2 + 5);
                    Text num = num_chemin.AddComponent<Text>();
                    num.text = length_value.length.ToString().Substring(0, 3);
                    num.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    num.alignment = TextAnchor.MiddleCenter;
                    num.color = Color.white;
                    num.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
                    num.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 1);
                }
            }
        }
        StartCoroutine(WaitSeconds(0.01f, 1));
    }

    public void AfficheChemin()
    {
        foreach(Transform child in Chemins.transform)
        {
            child.transform.GetChild(0).GetComponent<Text>().enabled = AffChem.GetComponent<Toggle>().isOn;
        }
    }
}
