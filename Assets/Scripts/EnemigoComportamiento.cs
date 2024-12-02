using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoComportamiento : MonoBehaviour
{
    public Enemigo datosEnemigo;
    private EnemyManager enemyManager;
    private ControladorBarraDeVidaEnemigo controladorBarra;

    private GameObject enemigoActual;

    // Start is called before the first frame update
    void Start()
    {
       
        controladorBarra = FindObjectOfType<ControladorBarraDeVidaEnemigo>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //controladorBarra.ActualizarBarra();
    }

    public void RecibirGolpe(float da�o)
    {
         enemigoActual = datosEnemigo.prefab;
        datosEnemigo.vida -= da�o;
       
        Debug.Log($"{datosEnemigo.nombre} recibi� {da�o} de da�o. Vida restante: {datosEnemigo.vida}");

        // Actualiza la barra de vida en la UI
        if (controladorBarra != null)
        {
            controladorBarra.MostrarBarraDeVida(this); // Configura la barra con el enemigo actual
            controladorBarra.ActualizarBarra(); // Actualiza la vida en la barra
        }

        if (datosEnemigo.vida <= 0)
        {
            // Remover al enemigo actual de enemigosActivos
            enemyManager.enemigosActivos.Remove(gameObject); // gameObject se refiere al objeto que tiene este script
            Debug.Log("Los Enemigos Activos son:" + enemyManager.ObtenerListaDeEnemigosActivos());

            SerDestruido(); // Destruir al enemigo
        }

    }
    private void SerDestruido()
    {


        Debug.Log($"{datosEnemigo.nombre} fue derrotado.");

        // Ocultar la barra de vida si este enemigo era el actual
        if (controladorBarra != null)
        {
            controladorBarra.OcultarBarra();
        }

        Destroy(gameObject); // Destruye el objeto cuando la vida llega a 0
        // Aqu� puedes agregar l�gica adicional, como otorgar recompensas al jugador
    }
}
