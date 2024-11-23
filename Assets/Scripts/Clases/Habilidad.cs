

using UnityEngine;
[System.Serializable]
public class Habilidad
{
    public string nombre;         // Nombre de la habilidad
    public float daño;
    public bool esActiva;         // True para activas, false para pasivas
    public float cooldown;        // Tiempo de enfriamiento en segundos
    public float duracion;        // Duración del efecto en segundos (si aplica)
    public float costo;           // Costo en energía/maná (si aplica)
    public Sprite icono;          // Icono de la habilidad para la UI

    // Propiedades específicas para habilidades activas
    public float rango;           // Rango de acción en unidades
    public LayerMask objetivoMask; // Qué tipos de objetos puede afectar (golpeables, enemigos, etc.)
}
