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
        // Configuraci�n inicial del rayo
        float rayLength = 1.2f; // Longitud m�xima del rayo
        Vector3 rayOrigin = jugador.transform.position + new Vector3(0, 0.5f, 0); // Origen del rayo (con elevaci�n)
        Vector3 rayDirection = -jugador.transform.forward; // Direcci�n del rayo (hacia atr�s)

        // Dibujar el rayo en la vista del editor para depuraci�n
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.green);

        // Lanza el rayo y detecta colisiones
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayLength))
        {
            ProcesarImpacto(hit); // Procesa el impacto del rayo
        }
        else
        {
            // No hay colisi�n
            camaraAtravesandoPared = false;
           // Debug.Log("El rayo de la camara no golpe� nada.");
        }
    }

    /// <summary>
    /// Procesa el impacto del rayo con un objeto en la escena.
    /// </summary>
    /// <param name="hit">Informaci�n del impacto del rayo.</param>
    void ProcesarImpacto(RaycastHit hit)
    {
        GameObject objeto = hit.collider.gameObject; // Objeto impactado por el rayo

        if (objeto.CompareTag("Obstaculo"))
        {
            camaraAtravesandoPared = true;
           // Debug.Log($"El rayo golpe� un obst�culo: {objeto.name}");
        }
        else
        {
            camaraAtravesandoPared = false;
            //Debug.Log($"El rayo golpe� un objeto diferente: {objeto.name}");
        }

        // Informaci�n adicional del impacto
        //Debug.Log($"El rayo golpe�: {objeto.name} en la posici�n {hit.point} a una distancia de {hit.distance}");
    }


}


