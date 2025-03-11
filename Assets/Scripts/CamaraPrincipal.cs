using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrincipal : MonoBehaviour
{
    public GameObject jugador;
    GameManager gameManager;
    public bool camaraAtravesandoPared;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void Update()
    {
        DetectarObstruccion();
    }
    void DetectarObstruccion()
    {
        // Configuración inicial del rayo
        float rayLength = 1.2f; // Longitud máxima del rayo
        Vector3 rayOrigin = jugador.transform.position + new Vector3(0, 0.5f, 0); // Origen del rayo (con elevación)
        Vector3 rayDirection = -jugador.transform.forward; // Dirección del rayo (hacia atrás)

        // Dibujar el rayo en la vista del editor para depuración
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);

        // Lanza el rayo y detecta colisiones
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayLength))
        {
            ProcesarImpacto(hit); // Procesa el impacto del rayo
        }
        else
        {
            // No hay colisión
            camaraAtravesandoPared = false;
           // Debug.Log("El rayo de la camara no golpeó nada.");
        }
    }

    /// <summary>
    /// Procesa el impacto del rayo con un objeto en la escena.
    /// </summary>
    /// <param name="hit">Información del impacto del rayo.</param>
    void ProcesarImpacto(RaycastHit hit)
    {
        GameObject objeto = hit.collider.gameObject; // Objeto impactado por el rayo

        if (objeto.CompareTag("Obstaculo"))
        {
            camaraAtravesandoPared = true;
           // Debug.Log($"El rayo golpeó un obstáculo: {objeto.name}");
        }
        else
        {
            camaraAtravesandoPared = false;
            //Debug.Log($"El rayo golpeó un objeto diferente: {objeto.name}");
        }

        // Información adicional del impacto
        //Debug.Log($"El rayo golpeó: {objeto.name} en la posición {hit.point} a una distancia de {hit.distance}");
    }


}


