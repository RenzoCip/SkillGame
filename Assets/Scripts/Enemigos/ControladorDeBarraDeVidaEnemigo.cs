using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

 public class ControladorBarraDeVidaEnemigo : MonoBehaviour
{
    public Slider barraDeVida; // Referencia al slider
    private EnemigoComportamiento enemigoActual; // Referencia al enemigo actual

    public void MostrarBarraDeVida(EnemigoComportamiento enemigo)
    {
        
        if (enemigoActual != enemigo)
        {
            enemigoActual = enemigo; // Guarda el enemigo actual
            barraDeVida.maxValue = enemigo.datosEnemigo.vidaMaxima; // Configura la vida máxima

        }
        barraDeVida.value = enemigo.datosEnemigo.vida; // Actualiza la vida restante del enemigo
        barraDeVida.gameObject.SetActive(true); // Activa la barra
    }

    public void ActualizarBarra()
    {
        if (enemigoActual != null && barraDeVida != null )
        {
            barraDeVida.value = enemigoActual.datosEnemigo.vida; // Actualiza la vida restante
        }
    }
    public void OcultarBarra()
    {
        barraDeVida.gameObject.SetActive (false);
        enemigoActual = null; // Elimina la referencia al enemigo
    }
}
