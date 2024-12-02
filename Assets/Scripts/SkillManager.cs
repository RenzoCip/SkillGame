using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Habilidad> HabilidadesDisponibles; //lista con las habilidades
    public List<Bala> BalasDisponibles; //lista con las balas 

    // Start is called before the first frame update
    public Transform puntoDeGolpe;
    public Transform direccionCamara;

    // public GameObject bala; // luego se quitara y se hara con una clase bala, cuando se decida si 

    void Start()
    {
        BalasDisponibles = new List<Bala>();
        BalasDisponibles.Add(new Bala { nombre = "Bala Basica", uso = " Daño", prefabBala = Resources.Load<GameObject>("Prefab/DisparoBasico"), velocidad = 30f, daño = 10 }); // tipo de bala que solo hace daño
        BalasDisponibles.Add(new Bala { nombre = "Bala Fria", uso = "Congelar" }); // tipo de bala que congela FALTA GREGAR PREFAB
        BalasDisponibles.Add(new Bala { nombre = "Bala Caliente", uso = "Calentar" }); // tipo de bala que quema FALTA GREGAR PREFAB
        BalasDisponibles.Add(new Bala { nombre = "Bala Luz", uso = "Iluminar" }); // tipo de bala que da luz FALTA GREGAR PREFAB

        HabilidadesDisponibles = new List<Habilidad>();
        HabilidadesDisponibles.Add(new Habilidad { nombre = "Golpear", daño = 10, esActiva = true, cooldown = 0f, costo = 0f, duracion = 10f, rango = 3f, objetivoMask = LayerMask.GetMask("Golpeable") });
        HabilidadesDisponibles.Add(new Habilidad { nombre = "Lanzar Proyectil", daño = 5, esActiva = false, cooldown = 3f, duracion = 0f, costo = 0f, rango = 8f, objetivoMask = LayerMask.GetMask("Golpeable") });
        HabilidadesDisponibles.Add(new Habilidad { nombre = "Aumento de velocidad", esActiva = false, cooldown = 0, duracion = 0f, costo = 0f });

        // Mostrar las habilidades en la consola
        foreach (var habilidad in HabilidadesDisponibles)
        {
            Debug.Log($"Habilidad: {habilidad.nombre}, Activa: {habilidad.esActiva}");
        }
    }
    public void ActivarHabilidad(int index) // recibe el parametro cuando desde el script de PlayerSKill le envia que presionó x boton
    {
        if (index < 0 || index >= HabilidadesDisponibles.Count)
        {
            Debug.LogError("Índice de habilidad fuera de rango");
            return;
        }
        Habilidad habilidad = HabilidadesDisponibles[index];
        Debug.Log($"Activando habilidad: {habilidad.nombre}");

        switch (habilidad.nombre) // dentro de el switch debes poner cada caso de ataque diferente que haga el jugador para que el metodo de la habilidad se ejecute 
        {
            case "Golpear":
                RealizarGolpe(habilidad);
                break;
            case "Lanzar Proyectil":
                RealizarDisparo(habilidad);
                break;
        }
    }
    private void RealizarGolpe(Habilidad habilidad)
    {
        Vector3 origenGolpe = puntoDeGolpe.position;
        Vector3 direccionGolpe = direccionCamara.forward;
        Debug.DrawRay(origenGolpe, direccionGolpe * habilidad.rango, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(origenGolpe, direccionGolpe, out hit, habilidad.rango, habilidad.objetivoMask))
        {
            EnemigoComportamiento enemigo = hit.collider.GetComponent<EnemigoComportamiento>();
            if (enemigo != null)
            {
                Debug.Log($"Golpeaste al enemigo {enemigo.datosEnemigo.nombre}");
                enemigo.RecibirGolpe(habilidad.daño); // Aplica el daño según la habilidad

            }
        }
        else
        {
            Debug.Log("no golpeaste ningun objeto.");
        }
    }

    private void RealizarDisparo(Habilidad habilidad)
    {
        Vector3 origenDisparo = puntoDeGolpe.position;
        Vector3 direccionDisparo = direccionCamara.forward;
        Debug.DrawRay(origenDisparo, direccionDisparo * habilidad.rango, Color.green, 1f);

        // Dispara el proyectil en la dirección deseada
        DispararProyectil(0, origenDisparo, direccionDisparo);

    }


    private void DispararProyectil(int balaIndex, Vector3 origenDisparo, Vector3 direccionDisparo) //pasa el parametro para que se intancie la bala
    {
        if (balaIndex < 0 || balaIndex >= BalasDisponibles.Count) //esto en caso de que el indice pasado sea erroneo, verifica el indice 
        {
            Debug.LogError("Índice de bala fuera de rango");
            return;
        }


        Bala bala = BalasDisponibles[balaIndex]; // obten el parametro de la bala que se va a usar 
        Transform puntoDeAparicion = puntoDeGolpe; // obten el parametro de punto de aparicion de la bala

        if (bala.prefabBala == null)// Asegúrate de que el prefab exista
        {
            Debug.LogError($"Prefab no asignado para la bala: {bala.nombre}");
            return;
        }

        // Instancia la bala en el punto de disparo y la lanza en la dirección deseada
        GameObject nuevaBala = Instantiate(bala.prefabBala, origenDisparo, Quaternion.identity);
        Rigidbody rb = nuevaBala.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = direccionDisparo.normalized * bala.velocidad;
        }

        // Configura las propiedades únicas del proyectil
        Proyectil scriptProyectil = nuevaBala.GetComponent<Proyectil>();
        if (scriptProyectil != null)
        {
            scriptProyectil.daño = bala.daño; // Asigna el daño desde la clase Bala
        }

    }
}
