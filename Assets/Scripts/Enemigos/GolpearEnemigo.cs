using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpearEnemigo : MonoBehaviour
{
    private EnemigoComportamiento comportamientoEnemigo;
    // Start is called before the first frame update
    void Start()
    {
        comportamientoEnemigo = FindObjectOfType<EnemigoComportamiento>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuerpo"))
        {
            Debug.Log("Enemigo Atacó");
            comportamientoEnemigo.AtacarJugador();
        }
    }
}
