using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public double estim_g;
    public double estim_f;
    public List<int> listsom = new List<int>();
    public int len;
    public List<int> MST = new List<int>(); // Ajouté pour afficher plus facilement les chemins utilisés pour calculer les différentes heuristiques (Pas juste le Minimum Spanning Tree - le nom est mal adapté)

}
