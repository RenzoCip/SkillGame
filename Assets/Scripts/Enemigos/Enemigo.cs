using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Enemigo 
{
    public GameObject prefab; //prefab del enemigo
    public string nombre; // nombre del enemigo
    public float vida; // puntos de vida del enemigo
    public float vidaMaxima;
    public float da�o; // puntos de da�o del enemigo
    public float recompensa; // recompensa del enemigo  POR DEFINIR TODAVIA
}
