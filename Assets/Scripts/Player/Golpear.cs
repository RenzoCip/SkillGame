using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golpear : MonoBehaviour
{
    public Habilidad habilidad;
    private SkillManager skillManager;
    private GameManager gameManager;


    public bool puedeHacerDa�o;
     void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        skillManager = FindObjectOfType<SkillManager>(); // Encuentra el SkillManager en la escena
    }

    // Start is called before the first frame update
     void OnTriggerEnter(Collider other)
    {
        if (gameManager.isInShootMode)
        {
            if (other.CompareTag("Enemigo"))
            {
                habilidad = skillManager.HabilidadesDisponibles[0]; // Asigna la habilidad deseada
                EnemigoComportamiento enemigo = other.GetComponent<EnemigoComportamiento>();

                if (enemigo != null)
                {
                    Debug.Log($"Golpeaste al enemigo {enemigo.datosEnemigo.nombre} con el pu�o.");
                    Debug.Log($"Habilidad en Golpear: {habilidad.nombre}, Da�o: {habilidad.da�o}");
                    puedeHacerDa�o = true;
                    if (gameManager.isInShootMode )
                    {
                        enemigo.RecibirGolpe(habilidad.da�o); // Aplica el da�o al enemigo
                    }
                    
                }
            }
        }
    }
}

