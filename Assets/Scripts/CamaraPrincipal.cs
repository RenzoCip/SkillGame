using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrincipal : MonoBehaviour
{
    public GameObject jugador;
    private Vector3 posicionJugador;
    private Quaternion rotacionJugador;
    public Vector3 posicionCamara;
    public Vector3 rotacionCamara;

    // Start is called before the first frame update
    void Start()
    {
        rotacionCamara = transform.position;
        posicionJugador = jugador.transform.position;
        rotacionJugador = jugador.transform.rotation;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotacionCamara.x = rotacionJugador.x;
        transform.position = posicionCamara + jugador.transform.position;
    }
}
