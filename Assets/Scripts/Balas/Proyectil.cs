using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public int daño; // no ajustar desde aqui sino desde la lista de balas en el skill Manager

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Golpeable") || collision.gameObject.CompareTag("Enemigo"))
        {

            EnemigoComportamiento enemigo = collision.gameObject.GetComponent<EnemigoComportamiento>();
            if (enemigo != null )
            {
                enemigo.RecibirGolpe(daño);
               
            }
            Destroy(gameObject);
        }

    }
}
