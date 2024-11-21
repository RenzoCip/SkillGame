using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemigo 
{
    public GameObject prefab; //prefab del enemigo
    public string nombre; // nombre del enemigo
    public float vida; // puntos de vida del enemigo
    public float daño; // puntos de daño del enemigo
    public float recompensa; // recompensa del enemigo  POR DEFINIR TODAVIA
}
