using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Habilidad> HabilidadesDisponibles; //lista con las habilidades


    // Start is called before the first frame update
    public Transform puntoDeGolpe;

    void Start()
    {
       

        HabilidadesDisponibles = new List<Habilidad>();
        HabilidadesDisponibles.Add(new Habilidad { nombre = "Golpear", daño = 10, esActiva = true, cooldown = 0f, costo = 0f, duracion = 10f, rango = 3f, objetivoMask = LayerMask.GetMask("Golpeable") });
        HabilidadesDisponibles.Add(new Habilidad { nombre = "Lanzar Proyectil", esActiva = false, cooldown = 3f, duracion = 0f, costo = 0f });
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
        }
    }
    private void RealizarGolpe(Habilidad habilidad)
    {
        Vector3 origenGolpe = puntoDeGolpe.position;
        Vector3 direccionGolpe = puntoDeGolpe.forward * habilidad.rango;
        Debug.DrawRay(origenGolpe, direccionGolpe, Color.red, 1f);

        RaycastHit hit;
        if (Physics.Raycast(origenGolpe, puntoDeGolpe.forward, out hit, habilidad.rango, habilidad.objetivoMask))
        {
            EnemigoComportamiento enemigo = hit.collider.GetComponent<EnemigoComportamiento>();
            if (enemigo !=null)
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


}
