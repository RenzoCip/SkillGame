

using UnityEngine;
[System.Serializable]
public class Habilidad
{
    public string nombre;         // Nombre de la habilidad
    public float da�o;
    public bool esActiva;         // True para activas, false para pasivas
    public float cooldown;        // Tiempo de enfriamiento en segundos
    public float duracion;        // Duraci�n del efecto en segundos (si aplica)
    public float costo;           // Costo en energ�a/man� (si aplica)
    public Sprite icono;          // Icono de la habilidad para la UI

    // Propiedades espec�ficas para habilidades activas
    public float rango;           // Rango de acci�n en unidades
    public LayerMask objetivoMask; // Qu� tipos de objetos puede afectar (golpeables, enemigos, etc.)
}
