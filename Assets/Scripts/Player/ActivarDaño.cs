using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarDaño : MonoBehaviour
{
    public GameObject golpeDerecha;
    public GameObject golpeIzquierda;
    public GameObject esquive;
    public GameObject cuerpo;

    private SkillManager skillManager;
    private GameManager gameManager;

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>(); // Encuentra el SkillManager en la escena
        gameManager = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame

    void ActivarDerecha() // ACTIVAR DERECHA PARA QUE EL JUGADOR GOLPEE
    {
        if (skillManager.tipoDeGolpe == 0 || skillManager.tipoDeGolpe == 1)
        {
            golpeDerecha.SetActive(true);
        }
        else
        {
            golpeDerecha.SetActive(false);
        }
    }
    void ActivarIzquierda() // ACTIVAR iZQUIERDA PARA QUE EL JUGADOR GOLPEE
    {
        if (skillManager.tipoDeGolpe == 0.5)
        {
            golpeIzquierda.SetActive(true);
        }
        else
        {
            golpeIzquierda.SetActive(false);
        }
    }
    public void DesactivarManos() // DESACTIVAR AMBAS MANOS AL FINALIZAR ANIMACION DE GOLPEO
    {
        golpeDerecha.SetActive(false);
        golpeIzquierda.SetActive(false);

    }
    void ActivarBloqueo() // ACTIVAR ESQUIVE 
    {
        esquive.SetActive(true) ;
    }
    void DesactivarBloqueo() //DESACTIVAR ESQUIVE
    {
        esquive.SetActive(false);
    }

    void DesactivarCuerpo() // DESACTIVAR CUERPO PARA QUE JUGADOR NO PUEDA RECIBIR DAÑO
    {
        cuerpo.SetActive(false) ;
    }
    void ActivarCuerpo() // ACTIVAR CUERPO PARA QUE JUGADOR PUEDA RECIBIR DAÑO 
    {
        cuerpo.SetActive(true) ;
    }
}