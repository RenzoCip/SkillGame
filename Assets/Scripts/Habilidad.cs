

using UnityEngine;
[System.Serializable]
public class Habilidad 
{
    public string Nombre;         // Nombre de la habilidad
    public bool EsActiva;         // True para activas, false para pasivas
    public float Cooldown;        // Tiempo de enfriamiento en segundos
    public float Duracion;        // Duraci�n del efecto en segundos (si aplica)
    public float Costo;           // Costo en energ�a/man� (si aplica)
    public Sprite Icono;          // Icono de la habilidad para la UI
}
