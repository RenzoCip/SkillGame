using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorEstadoJugador : MonoBehaviour
{
    public float vidaJugadorTot = 300;
    public float vidaJugadorRest = 300;
    public Image barraDeVida; // Referencia a la barra de vida 
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        vidaJugadorTot= vidaJugadorRest;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("la vida restante es :"+ vidaJugadorRest);
        CambiarColorVida();
    }
    void RecibirDañoJugador()
    {
            }
    public void CambiarColorVida()
    {
        barraDeVida.fillAmount = vidaJugadorRest/ vidaJugadorTot;
        Color VidaJugadorColor = Color.Lerp(Color.red, Color.green, vidaJugadorRest / vidaJugadorTot);
        barraDeVida.color = VidaJugadorColor;
    }
    public void AnimarDaño()
    {
        animator.SetTrigger("HitPlayer");
    }
}
