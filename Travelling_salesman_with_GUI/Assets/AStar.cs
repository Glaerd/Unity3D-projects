using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{

    public GameObject NodePrefab;
    public GameObject LignePrefab;
    private GameObject Content;
    private GameObject GRP;
    private GameObject Villes;
    private GameObject Chemins;
    private GameObject temp_chemins;
    private GameObject greenChemins;
    private GameObject yellowChemins;
    private GameObject cyanChemins;
    private GameObject tableau;
    private List<Node> OUVERT = new List<Node>();
    private List<Node> FERME = new List<Node>();
    private List<Node> M = new List<Node>();
    private Node n;
    private float anchor;
    private float vitesse = 0;
    private int SommetDepart;
    private int nbVilles;
    private int heur;
    private float startTime;

    void Update()
    {
        vitesse = GameObject.Find("Slider").GetComponent<Slider>().value;
        nbVilles = GameObject.Find("Button").GetComponent<CreateMap>().nbVilles;
    }

    public void StopAStar()
    {
        StopAllCoroutines();
    }

    public void DepartAStar()
    {
        StopAllCoroutines();
        OUVERT.Clear();
        FERME.Clear();
        M.Clear();
        n = null;
        startTime = Time.time;
        anchor = 0;
        Content = GameObject.Find("Content");
        GRP = GameObject.Find("GRP");
        foreach (Transform child in GRP.transform)
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
	Content.GetComponent<RectTransform>().sizeDelta = new Vector2(-10, Screen.height);
        Villes = GameObject.Find("Villes");
        Chemins = GameObject.Find("Chemins");
        temp_chemins = GameObject.Find("temp");
        greenChemins = GameObject.Find("greenChemins");
        yellowChemins = GameObject.Find("yellowChemins");
        cyanChemins = GameObject.Find("cyanChemins");
        tableau = GameObject.Find("tableau");
        SommetDepart = int.Parse(GameObject.Find("SommetDepart").GetComponent<InputField>().text);
        nbVilles = GameObject.Find("Button").GetComponent<CreateMap>().nbVilles;
        vitesse = GameObject.Find("Slider").GetComponent<Slider>().value;
        heur = int.Parse(GameObject.Find("TypeHeur").GetComponent<InputField>().text);
        GameObject s = Instantiate(NodePrefab);
        s.GetComponent<Node>().listsom.Add(SommetDepart);
        s.transform.parent = GRP.transform;
        Node p = s.GetComponent<Node>();
        OUVERT.Add(p);
        M.Add(p);
        DerouleAStar();
    }

    void DerouleAStar() // Cherche le minimum de la liste OUVERT et met à jour des paramètres
    {
        if (GRP.GetComponent<Node>() == null) GRP.AddComponent<Node>();
        Node temp = GRP.GetComponent<Node>();
        temp.estim_f = Mathf.Infinity;
        foreach (Node sommet in M) // o(M.Count) avec M.Count < nbVilles - Calculer la fonction f des nouveaux éléments de la liste OUVERT
        {
            sommet.estim_f = fonctionf(sommet);
        }

        foreach (Node sommet in OUVERT) // o(OUVERT.Count), ce qui peut être énorme -> Chercher à effectuer une recherche de minimum en o(log(OUVERT.Count)) : probablement beaucoup plus intéressant
        {
            if (sommet.estim_f < temp.estim_f) temp = sommet;
        }
        n = temp; // n = Node de OUVERT ayant la valeur minimum d'estimf

        if (!GameObject.Find("pause").GetComponent<Pause>().pause)
        {
            OUVERT.Remove(n); // On enlève d'OUVERT le minimum de la liste
            FERME.Add(n); // Ajoute le minimum à la liste FERME
        }
        if (n.listsom.Count == nbVilles + 1) // Sortie de l'algorithme - On est sûr que n.listsom est le chemin hamiltonien le plus court parcourant toutes les villes et retournant à son point de départ
        {
            foreach (Transform child in GameObject.Find("CheminsMST").transform)
            {
                Destroy(child.gameObject);
            }
            EvaluationHeur();
            float endTime = Time.time;
            AfficheTemps(endTime - startTime);
            AfficheResultat(n, 0);
            return;
        }
        if (yellowChemins.GetComponent<Toggle>().isOn) AfficheResultat(n, 1);
        if (cyanChemins.GetComponent<Toggle>().isOn) AfficheMST(n);
        if (tableau.GetComponent<Toggle>().isOn && !GameObject.Find("pause").GetComponent<Pause>().pause) UpdateTableau(n);
        if (!GameObject.Find("pause").GetComponent<Pause>().pause) StartCoroutine(UpdateOUVERT(n)); // Cherche les successeurs de n dans le GRP et les met dans OUVERT
        else StartCoroutine(DuringPause());
    }

    IEnumerator UpdateOUVERT(Node sommet) // Cherche les successeurs de n
    {
        yield return new WaitForSeconds(1 - vitesse);
        int i;
        M.Clear(); // Détruit les éléments de la liste M
        for (i = 0; i < nbVilles; i++)
        {
            if (!sommet.listsom.Contains(i)) // Pour tous les sommets n'appartenant pas à n.listsom, on crée des Node ayant n.listsom + le sommet en question
            {
                GameObject s = Instantiate(NodePrefab);
                s.GetComponent<Node>().listsom.AddRange(sommet.listsom);
                s.GetComponent<Node>().listsom.Add(i);
                s.transform.parent = sommet.gameObject.transform;
                OUVERT.Add(s.GetComponent<Node>()); // On ajoute ce Node à OUVERT
                M.Add(s.GetComponent<Node>()); // On ajoute ce Node à M
            }
        }
        if (sommet.listsom.Count == nbVilles) // Si n.listsom contient tous les sommets, on crée un Node ayant n.listsom + le sommet de départ
        {
            GameObject s = Instantiate(NodePrefab);
            s.GetComponent<Node>().listsom.AddRange(sommet.listsom);
            s.GetComponent<Node>().listsom.Add(SommetDepart);
            s.transform.parent = sommet.gameObject.transform;
            OUVERT.Add(s.GetComponent<Node>()); // On ajoute ce Node à OUVERT
            M.Add(s.GetComponent<Node>()); // On ajoute ce Node à M
        }
        DerouleAStar(); // On recalcule le minimum d'OUVERT
    }

    double fonctionf(Node sommet) // switch le résultat selon les heuristiques
    {
        double res;
        switch (heur)
        {
            case 0:
                res = heur0(sommet);
                break;
            case 1:
                res = heur1(sommet);
                break;
            case 2:
                res = heur2(sommet);
                break;
            case 3:
                res = heur3(sommet);
                break;
            default:
                res = 0.0;
                break;
        }
        return res;
    }

    double heur0(Node sommet) // retourne la fonction g(n)
    {
        return CalculeG(sommet);
    }

    double heur1(Node sommet) // retourne la fonction g(n) + la valeur du chemin minimum entre tous les chemins allant du dernier sommet de n.listsom aux autres sommets restants n'appartenant pas à n.listsom
    {
        double res = CalculeG(sommet);
        int k;
        float heur1 = Mathf.Infinity;
        List<int> SRest = new List<int>();
        List<int> Min1 = new List<int>();
        for (k = 0; k < nbVilles; k++)
        {
            if (!sommet.listsom.Contains(k))
            {
                SRest.Add(k);
            }
        }
        if (sommet.listsom.Count < nbVilles)
        {
            foreach (int i in SRest)
            {
                if (Chemins.transform.GetChild(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], i)).GetComponent<CheminValues>().length < heur1)
                {
                    heur1 = Chemins.transform.GetChild(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], i)).GetComponent<CheminValues>().length;
                    Min1.Clear();
                    Min1.Add(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], i));
                }
            }
        }
        else if (sommet.listsom.Count == nbVilles)
        {
            heur1 = Chemins.transform.GetChild(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], SommetDepart)).GetComponent<CheminValues>().length;
            Min1.Add(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], SommetDepart));
        }
        else if (sommet.listsom.Count == nbVilles + 1)
        {
            heur1 = 0;
        }
        sommet.MST = Min1;
        return res + heur1;
    }

    double heur2(Node sommet) // retourne la fonction g(n) + la somme des valeurs des chemins minimums entre tous les chemins allant des sommets n'appartenant pas à n.listsom ou du dernier sommet de n.listsom aux autres sommets restants n'appartenant pas à n.listsom ou au sommet de départ
    {
        double res = CalculeG(sommet);
        int k;
        float heur2 = 0;
        List<int> SRest = new List<int>();
        List<int> Min2 = new List<int>();
        List<int> SMin = new List<int>();
        for (k = 0; k < nbVilles; k++)
        {
            if (!sommet.listsom.Contains(k) || k == SommetDepart || k == sommet.listsom[sommet.listsom.Count - 1])
            {
                SRest.Add(k);
            }
        }
        if (sommet.listsom.Count < nbVilles)
        {
            foreach (int h in SRest)
            {
                float temp_heur2 = Mathf.Infinity;
		        if(h != SommetDepart)
                {
                    foreach (int i in SRest) // o(nbVilles²)
                    {
                        if (i != sommet.listsom[sommet.listsom.Count - 1] && h != i && Chemins.transform.GetChild(SommetsToChemins(h, i)).GetComponent<CheminValues>().length < temp_heur2)
                        {
                            temp_heur2 = Chemins.transform.GetChild(SommetsToChemins(h, i)).GetComponent<CheminValues>().length;
                            Min2.Clear();
                            Min2.Add(SommetsToChemins(h, i));
                        }
                    }
                    SMin.AddRange(Min2);
                    heur2 += temp_heur2;
                }
            }
        }
        else if (sommet.listsom.Count == nbVilles)
        {
            heur2 = Chemins.transform.GetChild(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], SommetDepart)).GetComponent<CheminValues>().length;
            SMin.Add(SommetsToChemins(sommet.listsom[sommet.listsom.Count - 1], SommetDepart));
        }
        else if (sommet.listsom.Count == nbVilles + 1)
        {
            heur2 = 0;
        }
        sommet.MST = SMin;
        return res + heur2;
    }

    double heur3(Node sommet) // retourne la fonction g(n) + la valeur du poids du graphe de poids minimum entre tous les sommets n'appartenant pas à listsom ou étant le sommet de départ ou étant le dernier sommet de n.listsom
    {
        double res = CalculeG(sommet);
        float heur3 = 0;
        int j;
        List<int> temp_som = new List<int>();
        List<GameObject> temp_chem = new List<GameObject>();
        List<int> MST3 = new List<int>();
        for (j = 0; j < nbVilles; j++)
        {
            if (!sommet.listsom.Contains(j) || j == sommet.listsom[sommet.listsom.Count - 1] || j == SommetDepart)
            {
                temp_som.Add(j);
            }
        }
        foreach (Transform obj in temp_chemins.transform)
        {
            Destroy(obj.gameObject);
        }
        int k, l;
        for (k = 0; k < temp_som.Count; k++)
        {
            for (l = 0; l < temp_som.Count; l++) // o(nbVilles²)
            {
                if (temp_som[l] > temp_som[k])
                {
                    GameObject temp_chemin = new GameObject();
                    temp_chemin.transform.parent = temp_chemins.transform;
                    temp_chemin.AddComponent<CheminValues>();
                    temp_chemin.GetComponent<CheminValues>().sommetA = temp_som[k];
                    temp_chemin.GetComponent<CheminValues>().sommetB = temp_som[l];
                    temp_chemin.GetComponent<CheminValues>().length = Chemins.transform.GetChild(SommetsToChemins(temp_som[k], temp_som[l])).GetComponent<CheminValues>().length;
                    temp_chem.Add(temp_chemin);
                }
            }
        }
        if (temp_chem.Count > 0)
        {
            temp_chem.Sort(delegate (GameObject a, GameObject b)
            {
                return (a.GetComponent<CheminValues>().length).CompareTo(b.GetComponent<CheminValues>().length);
            });
        }
        foreach (Transform ville in Villes.transform)
        {
            ville.GetComponent<VilleSet>().parent = ville.gameObject;
        }

        for (j = 0; j < temp_chem.Count; j++)
        {
            if (findset(temp_chem[j].GetComponent<CheminValues>().sommetA) != findset(temp_chem[j].GetComponent<CheminValues>().sommetB)) // utilise une structure de données disjointe
            {
                union(temp_chem[j].GetComponent<CheminValues>().sommetA, temp_chem[j].GetComponent<CheminValues>().sommetB);
                MST3.Add(SommetsToChemins(temp_chem[j].GetComponent<CheminValues>().sommetA, temp_chem[j].GetComponent<CheminValues>().sommetB));
                heur3 += temp_chem[j].GetComponent<CheminValues>().length;
            }
        }
        sommet.MST = MST3;
        return res + heur3;
    }

    double CalculeG(Node sommet) // retourne la fonction g(n)
    {
        int k = -1;
        foreach (int som in sommet.listsom)
        {
            if (k != -1)
            {
                sommet.estim_g += Chemins.transform.GetChild(SommetsToChemins(som, k)).GetComponent<CheminValues>().length;
            }
            k = som;
        }
        return sommet.estim_g;
    }

    int SommetsToChemins(int i, int j) // Retourne le chemin correspondant à deux sommets
    {
        int x = Mathf.Min(i, j);
        int y = Mathf.Max(i, j);
        int res = 0;
        res = (int)((1f / 2f) * (1f + 2f * ((float)nbVilles - 1f) - (float)x) * (float)x + y - x - 1);
        return res;
    }

    GameObject findset(int sommet) // Retourne l'identifiant de la composante connexe à laquelle sommet est attaché
    {
        if (Villes.transform.GetChild(sommet).GetComponent<VilleSet>().parent == Villes.transform.GetChild(sommet).gameObject) return Villes.transform.GetChild(sommet).gameObject;
        else
        {
            return findset(int.Parse(Villes.transform.GetChild(sommet).GetComponent<VilleSet>().parent.name));
        }
    }

    void union(int sommetA, int sommetB) // Regroupe deux composantes connexes
    {
        GameObject x = findset(int.Parse(Villes.transform.GetChild(sommetA).GetComponent<VilleSet>().parent.name));
        GameObject y = findset(int.Parse(Villes.transform.GetChild(sommetB).GetComponent<VilleSet>().parent.name));
        x.GetComponent<VilleSet>().parent = y;
    }

    void AfficheResultat(Node resultat, int type) // Dessine le chemin correspondant au résultat en vert ou le chemin reliant tous les sommets de n.listsom en jaune
    {
        int k = -1;
        foreach (Transform go in GameObject.Find("CheminsAffiche").transform)
        {
            Destroy(go.gameObject);
        }
        foreach (int som in resultat.listsom)
        {
            if (k != -1)
            {
                GameObject cheminChoisi = new GameObject();
                cheminChoisi.transform.name = k.ToString() + " -> " + som.ToString();
                cheminChoisi.transform.parent = GameObject.Find("CheminsAffiche").transform;
                LineRenderer lineRenderer = cheminChoisi.AddComponent<LineRenderer>();
                if (type == 0)
                {
                    lineRenderer.SetColors(Color.green, Color.green);
                    Material greenUnlitMat = new Material(Shader.Find("Unlit/Color"));
                    greenUnlitMat.color = Color.green;
                    lineRenderer.material = greenUnlitMat;
                }
                if (type == 1)
                {
                    lineRenderer.SetColors(Color.yellow, Color.yellow);
                    Material yellowUnlitMat = new Material(Shader.Find("Unlit/Color"));
                    yellowUnlitMat.color = Color.yellow;
                    lineRenderer.material = yellowUnlitMat;
                }
                lineRenderer.SetWidth(0.05F, 0.05F);
                lineRenderer.SetPosition(0, new Vector3(Villes.transform.GetChild(k).transform.position.x, Villes.transform.GetChild(k).transform.position.y, Villes.transform.GetChild(k).transform.position.z + 9));
                lineRenderer.SetPosition(1, new Vector3(Villes.transform.GetChild(som).transform.position.x, Villes.transform.GetChild(som).transform.position.y, Villes.transform.GetChild(som).transform.position.z + 9));
                if (type == 0) lineRenderer.enabled = greenChemins.GetComponent<Toggle>().isOn;
            }
            k = som;
        }
    }

    void AfficheMST(Node sommet) // Dessine les chemins utilisés pour calculer h(n) en bleu
    {
        foreach (Transform go in GameObject.Find("CheminsMST").transform)
        {
            Destroy(go.gameObject);
        }
        foreach (int chem in sommet.MST)
        {
            GameObject cheminChoisi = new GameObject();
            cheminChoisi.transform.name = Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetA.ToString() + " -> " + Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetB.ToString();
            cheminChoisi.transform.parent = GameObject.Find("CheminsMST").transform;
            LineRenderer lineRenderer = cheminChoisi.AddComponent<LineRenderer>();
            lineRenderer.SetColors(Color.cyan, Color.cyan);
            Material cyanUnlitMat = new Material(Shader.Find("Unlit/Color"));
            cyanUnlitMat.color = Color.cyan;
            lineRenderer.material = cyanUnlitMat;
            lineRenderer.SetWidth(0.05F, 0.05F);
            lineRenderer.SetPosition(0, new Vector3(Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetA).transform.position.x, Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetA).transform.position.y, Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetA).transform.position.z + 9));
            lineRenderer.SetPosition(1, new Vector3(Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetB).transform.position.x, Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetB).transform.position.y, Villes.transform.GetChild(Chemins.transform.GetChild(chem).GetComponent<CheminValues>().sommetB).transform.position.z + 9));
        }
    }

    void UpdateTableau(Node sommet) // Ajoute une ligne au tableau de l'historique du déroulement de l'algorithme
    {
        string nvalue = "";
        string mvalue = "";
        int k;
        int j = 0;
        int height1 = 1;
        int height2 = 1;

        for (k = 0; k < sommet.listsom.Count; k++)
        {
            if (k % 4 == 0 && k != 0)
            {
                nvalue += "\n";
                if (k < sommet.listsom.Count - 1) nvalue += "->" + sommet.listsom[k].ToString() + "->";
                else nvalue += "->" + sommet.listsom[k].ToString();
                height1++;
            }
            else if (k % 4 == 3)
            {
                if (k < sommet.listsom.Count - 1) nvalue += sommet.listsom[k].ToString();
                else nvalue += sommet.listsom[k].ToString();
            }
            else
            {
                if (k < sommet.listsom.Count - 1) nvalue += sommet.listsom[k].ToString() + "->";
                else nvalue += sommet.listsom[k].ToString();
            }
        }

        if (M.Count != 0)
        {
            if(M.Count == 1 && M[0].listsom.Count == 1)
            {
                mvalue = "{" + SommetDepart.ToString() + "}";
            }
            else
            {
                mvalue += "{";
                foreach (Node s in M)
                {
                    if (j % 3 == 0 && j != 0)
                    {
                        mvalue += "\n";
                        height2++;
                        mvalue += "n->" + s.listsom[s.listsom.Count - 1].ToString() + ",";
                    }
                    else mvalue += "n->" + s.listsom[s.listsom.Count - 1].ToString() + ",";
                    j++;
                }
                if (mvalue == "{") mvalue += "n->" + SommetDepart.ToString() + ",";
                mvalue = mvalue.Substring(0, mvalue.Length - 1);
                if (mvalue != "") mvalue += "}";
            }
        }

        GameObject ligne = Instantiate(LignePrefab);
        ligne.transform.SetParent(Content.transform.GetChild(5).transform);
        ligne.transform.GetChild(0).GetComponent<Text>().text = mvalue;
        ligne.transform.GetChild(1).GetComponent<Text>().text = OUVERT.Count.ToString();
        ligne.transform.GetChild(2).GetComponent<Text>().text = FERME.Count.ToString();
        ligne.transform.GetChild(3).GetComponent<Text>().text = nvalue;
        ligne.GetComponent<RectTransform>().localPosition = new Vector3(0, ligne.GetComponent<RectTransform>().localPosition.y - anchor, 0);
        ligne.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        ligne.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30 * Mathf.Max(height1, height2));
        ligne.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -anchor);
        Content.GetComponent<RectTransform>().sizeDelta += new Vector2(0, ligne.GetComponent<RectTransform>().sizeDelta.y);
        anchor += ligne.GetComponent<RectTransform>().sizeDelta.y;
    }

    void EvaluationHeur() // Affiche (le nombre de Node parcourus / la taille totale du GRP)
    {
        GameObject[] nodesparcourus = GameObject.FindGameObjectsWithTag("Node");
        int nbnodesparcourus = nodesparcourus.Length;
        int nbnodestotal = 0;
        int etage;
        int k, l;
        for (k = 1; k < nbVilles; k++)
        {
            etage = 1;
            for (l = k; l < nbVilles; l++)
            {
                etage = l * etage;
            }
            if (k == 1) etage = etage * 2;
            nbnodestotal += etage;
        }
        nbnodestotal += 1;
        double eval;
        eval = System.Math.Round((float)nbnodesparcourus / (float)nbnodestotal, 4);
        GameObject.Find("eval").GetComponent<Text>().text = GameObject.Find("eval").GetComponent<Text>().text.Substring(0, 40);
        string res = eval.ToString();
        if (res.Length > 3) GameObject.Find("eval").GetComponent<Text>().text += res.Substring(0, 4);
        else GameObject.Find("eval").GetComponent<Text>().text += res;
    }

    void AfficheTemps(float delta) // Affiche le temps effectué pour dérouler l'algorithme
    {
        GameObject.Find("temps").GetComponent<Text>().text = GameObject.Find("temps").GetComponent<Text>().text.Substring(0, 15);
        string res = delta.ToString();
        if (res.Length > 4) GameObject.Find("temps").GetComponent<Text>().text += res.Substring(0, 5) + " secondes";
        else GameObject.Find("temps").GetComponent<Text>().text += res + " secondes";
    }

    IEnumerator DuringPause() // Attend 1 seconde
    {
        yield return new WaitForSeconds(1);
        DerouleAStar();
    }
}
