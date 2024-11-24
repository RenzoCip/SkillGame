using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemigo> enemigosDisponibles;
    public Transform[] puntosDeAparicion; // por definir cuales seran los puntos de aparicion en este array
    public List<GameObject> enemigosActivos;

    // Start is called before the first frame update
    void Start()
    {
        
        enemigosActivos = new List<GameObject>();

        enemigosDisponibles = new List<Enemigo>();

        enemigosDisponibles.Add(new Enemigo { nombre = "EnemigoBasico", vida = 100f, daño = 10f, recompensa= 1f, prefab = Resources.Load <GameObject>("Prefab/EnemigoBasico")});
        enemigosDisponibles.Add(new Enemigo { nombre = "EnemigoVolador", vida = 100f, daño = 5f, recompensa = 1f, prefab = Resources.Load <GameObject>("Prefab/EnemigoVolador") });

        Debug.Log("EnemyManager inicializado.");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AparecerEnemigo(int enemigoIndex, int puntoDeAparicionIndex) //pasa el parametro del enemigo que quieres instanciar
    {
        if (enemigoIndex < 0 || enemigoIndex >= enemigosDisponibles.Count) //esto en caso de que el indice pasado sea erroneo
        {
            Debug.LogError("Índice de enemigo fuera de rango");
            return;
        }

        if (puntoDeAparicionIndex < 0 || puntoDeAparicionIndex >= puntosDeAparicion.Length)
        {
            Debug.LogError("Índice de punto de spawn fuera de rango");
            return;
        }


        // Obtén el enemigo y el punto de spawn
        Enemigo enemigo = enemigosDisponibles[enemigoIndex];
        Transform puntoDeAparicion = puntosDeAparicion[puntoDeAparicionIndex];


        if (enemigo.prefab != null)
        {
            GameObject nuevoEnemigo = Instantiate(enemigo.prefab, puntoDeAparicion.position, Quaternion.identity);

            EnemigoComportamiento comportamiento = nuevoEnemigo.GetComponent<EnemigoComportamiento>();
            if (comportamiento != null)
            {
                comportamiento.datosEnemigo = enemigo;
            }
            enemigosActivos.Add(nuevoEnemigo); // Añadir el enemigo a la lista de activos
            Debug.Log("los enemigos activos son: " + ObtenerListaDeEnemigosActivos());
            Debug.Log($"Apareció un {enemigo.nombre} en el punto de spawn {puntoDeAparicionIndex}");
        }
        else
        {
            Debug.LogError($"Prefab no asignado para el enemigo: {enemigo.nombre}");
        }
    }
    public string ObtenerListaDeEnemigosActivos()
    {
        if (enemigosActivos.Count == 0)
            return "No hay enemigos activos.";

        string nombres = "";
        foreach (GameObject enemigo in enemigosActivos)
        {
            nombres += enemigo.name + ", ";
        }
        return nombres.TrimEnd(',', ' '); // Elimina la coma y el espacio extra al final
    }
}