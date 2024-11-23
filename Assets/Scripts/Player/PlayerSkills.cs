using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private SkillManager skillManager;
    private EnemyManager enemyManager;

    Vector3[] puntosDeAparicionPrueba;
    // Start is called before the first frame update
    void Start()
    {
     enemyManager = FindAnyObjectByType<EnemyManager>();
     skillManager = FindAnyObjectByType<SkillManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Activa la primera habilidad
        {
            skillManager.ActivarHabilidad(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // Activa la segunda habilidad
        {
            skillManager.ActivarHabilidad(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) //Activa la tercera habilidad 
        {
            skillManager.ActivarHabilidad(2); 
        }
        if (Input.GetKeyDown(KeyCode.P)) //Activa la tercera habilidad 
        {
            enemyManager.AparecerEnemigo(0, 0);
        }

    }
}
