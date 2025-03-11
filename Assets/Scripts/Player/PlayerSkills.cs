using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private GameManager gameManager;
    private SkillManager skillManager;
    private EnemyManager enemyManager;
    private Animator animator;
    private EstadoPunch estadoPunch;

    public GameObject linterna;
    public GameObject luz;
    public GameObject menu;
    public bool menuAbierto = true;
    public bool luzEncendida = false;
    public bool linternaEncendida = false;
    public bool estaOcupado = false; // Nueva variable de estado

    Vector3[] puntosDeAparicionPrueba;
    // Start is called before the first frame update
    void Start()
    {

     animator = GetComponent<Animator>();
     gameManager = FindAnyObjectByType<GameManager>();
     enemyManager = FindAnyObjectByType<EnemyManager>();
     skillManager = FindAnyObjectByType<SkillManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        bool atacando = animator.GetBool("Atacando");
        bool esquivando = animator.GetBool("Esquivando");
        bool golpeado = animator.GetBool("Golpeado");
       // Debug.Log("el estado de ocupado es" + estaOcupado);

        if (estaOcupado)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && gameManager.isInShootMode && !esquivando && !golpeado ) // Activa la primera habilidad GOLPE
        {
            estaOcupado = true;
            ResetearTriggers();
            skillManager.ActivarHabilidad(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && gameManager.isInShootMode) // Activa la segunda habilidad DISPARO
        {
            
            skillManager.ActivarHabilidad(1);
        }
        if (Input.GetKeyDown(KeyCode.Space) && gameManager.isInShootMode && !atacando && !golpeado) //Activa la tercera habilidad ESQUIVE
        {
            estaOcupado = true;
            ResetearTriggers();

            animator.SetTrigger("Esquive");
        }
        if (Input.GetKeyDown(KeyCode.P)) //Activa la tercera habilidad 
        {
            enemyManager.AparecerEnemigo(0, 0);
            enemyManager.AparecerEnemigo(0, 1);
            enemyManager.AparecerEnemigo(0, 2);
            enemyManager.AparecerEnemigo(0, 3);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            linternaEncendida = !linternaEncendida;
            linterna.gameObject.SetActive(linternaEncendida); // Activa o desactiva la luz
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            luzEncendida = !luzEncendida;
            luz.gameObject.SetActive(luzEncendida); // Activa o desactiva la luz
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            menuAbierto = !menuAbierto;
            menu.gameObject.SetActive(menuAbierto); // Activa o desactiva la luz
        }
        void ResetearTriggers()
        {
            animator.ResetTrigger("Esquive");
            animator.ResetTrigger("Punch"); 
            animator.ResetTrigger("HitPlayer");
        }
    }
}
