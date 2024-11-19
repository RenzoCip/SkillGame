

using UnityEngine;
[System.Serializable]
public class Habilidad 
{
    public string Nombre;         // Nombre de la habilidad
    public bool EsActiva;         // True para activas, false para pasivas
    public float Cooldown;        // Tiempo de enfriamiento en segundos
    public float Duracion;        // Duración del efecto en segundos (si aplica)
    public float Costo;           // Costo en energía/maná (si aplica)
    public Sprite Icono;          // Icono de la habilidad para la UI
}
