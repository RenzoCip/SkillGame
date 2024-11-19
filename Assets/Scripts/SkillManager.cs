using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<Habilidad> HabilidadesDisponibles; //lista con las habilidades
    // Start is called before the first frame update
    void Start()
    {
        HabilidadesDisponibles = new List<Habilidad>();
        HabilidadesDisponibles.Add(new Habilidad { Nombre = "Golpear", EsActiva = true, Cooldown = 0f, Costo = 0f, Duracion = 1f, });
        HabilidadesDisponibles.Add(new Habilidad { Nombre = "Lanzar Proyectil", EsActiva = true, Cooldown = 3f, Duracion = 0f, Costo = 0f });
        HabilidadesDisponibles.Add(new Habilidad { Nombre = "Aumento de velocidad", EsActiva = false, Cooldown = 0, Duracion = 0f, Costo = 0f });

        // Mostrar las habilidades en la consola
        foreach (var habilidad in HabilidadesDisponibles)
        {
            Debug.Log($"Habilidad: {habilidad.Nombre}, Activa: {habilidad.EsActiva}");
        }
    }
    public void ActivarHabilidad(int index)
    {
        if (index < 0 || index >= HabilidadesDisponibles.Count)
        {
            Debug.LogError("Índice de habilidad fuera de rango");
            return;
        }
        Habilidad habilidad = HabilidadesDisponibles[index];
        Debug.Log($"Activando habilidad: {habilidad.Nombre}");
        // Aquí puedes agregar lógica específica para cada habilidad
    }
}