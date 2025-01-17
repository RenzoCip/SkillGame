using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemigoComportamiento : MonoBehaviour
{

    public enum EstadoEnemigo
    {
        Idle,
        Vagar,
        Alerta,
        Perseguir,
        Atacar,
        Aturdido,
        Muriendo
    }
    public EstadoEnemigo estadoActual = EstadoEnemigo.Idle;

    public Enemigo datosEnemigo;
    private EnemyManager enemyManager;
    private ControladorBarraDeVidaEnemigo controladorBarra;
    private NavMeshAgent agente; // Declarar el NavMeshAgent


    public GameObject target;
    public GameObject estadoEnemigo;

    public Vector3 spawnPoint; // Punto de spawn original del enemigo 

    private Renderer Renderer;

    public float rangoDeDeteccion;
    public float distanciaParaDetenerse;
    public float velocidadEnemigo;
    private float velocidadInicialEnemigo;
    public float recordarPosPorTiempo; // Tiempo que el enemigo recuerda la �ltima posici�n

    private bool JugadorDentroRango;

    public bool persistente; // Si el enemigo nunca pierde al jugador
    public bool puedeResucitar; // Si el enemigo puede resucitar despu�s de morir
    public bool targetEsVisible;
    //private bool haResucitado = false;

    public  float temporizadorDetectarJugador; //temporizador para dejar de buscar al jugador 
    private Vector3 ultimaPosicionRecordadaTarget; // posicion para buscar al jugador en el ultimo lugar donde lo vio 
    private bool targetDetectado;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>(); // Inicializar el agente
        agente.speed = velocidadEnemigo; // Configurar la velocidad del NavMeshAgent
        agente.stoppingDistance = distanciaParaDetenerse; // Distancia m�nima para detenerse

        velocidadInicialEnemigo = velocidadEnemigo;
        target = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        controladorBarra = FindObjectOfType<ControladorBarraDeVidaEnemigo>();
        enemyManager = FindObjectOfType<EnemyManager>();
        estadoEnemigo = GameObject.FindWithTag("PruebaEstado");
        Renderer = estadoEnemigo.GetComponent<Renderer>();

        //animator.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadAnimator = animator.GetFloat("XSpeed");
        Debug.Log($"El valor actual de 'XSpeed' en el Animator es: {velocidadAnimator}");

        switch (estadoActual)
        {
            case EstadoEnemigo.Idle: // <<<!!! Estado Incial- estar� quieto y haciendo ruidos pero es un estado previo a todo lo que implica comportamientos, debe ser cambiado siempre de manera mecanica  !!!<<<
                EstadoIdle();
                
                break;

            case EstadoEnemigo.Vagar:// Pasa siempre del Estado Idel a Vagar de manera mecanica, hay que llamarlo , se mantiende haciendo algo sumamente basico esperando que el jugador entre en el area trigger para cambiar a estado de Alerta
                EstadoVagar();
                
                break;

            case EstadoEnemigo.Alerta: // jugador entra en el area trigger , se activa Estado Alerta y enemigo espera un estimulo para pasar a Estado Perseguir ( linea de vision, interaccion con el entorno del jugador, recibir luz de la linterna, recibir alerta de un enemigo que alerta a los demas)
                EstadoAlerta();

                break;

            case EstadoEnemigo.Perseguir: // el enemigo detect� al jugador mediante un estimulo, se activa Estado Perseguir y cuando est� suficientemnete cerca pasa a Estado Atacar o si recibe da�o a Estado Golpeado
                EstadoPerseguir();
                break;

            case EstadoEnemigo.Atacar: //en Estado Atacar el enemigo realiza un ataque, puede pasar a Estado Perseguir si el jugador se aleja o Estado Golpeado si recibe da�o
                EstadoAtacar();
                break;

            //case EstadoEnemigo.Golpeado: //en Estado Golpeado el enemigo se estunea y recibe da�o, puede pasar a Estado Atacar si el jugador sigue cerca o a Estado Perseguir si jugador se alej� o a Estado Morir si vida llega a Cero
                //EstadoGolpeado);
                //break;

           // case EstadoEnemigo.Morir: // en estado morir vida es cero y animacion y sonido de morir 
             //   EstadoMorir();
               // break;
        }
    }
    private void EstadoIdle()
    {
        Debug.Log($"{datosEnemigo.nombre} est� en reposo.");
        Renderer.material.color = Color.white;
        estadoActual = EstadoEnemigo.Vagar;
        // estadoEnemigo.
        //animacion basica 

    }
    private void EstadoVagar()
    {

        Debug.Log($"{datosEnemigo.nombre} est� Vagueando espoerando que el jugador entre en el trigger");
        Renderer.material.color = Color.green;

        //agregar animacion de lo que sea que est� haciendo y cubito de colores para ver su estado 
        //si acaba de volver del estado perseguir debe volver a su posicion original 
        // Mover hacia el punto de spawn si no est� en �l
        if (Vector3.Distance(transform.position, spawnPoint) > 0.1f)
        {
           
            SeguirTarget(spawnPoint);

        }

        if (JugadorDentroRango)
        {
            Debug.Log("el jugador esta en el trigger");
            estadoActual = EstadoEnemigo.Alerta;
        }
    }
    private void EstadoAlerta()
    {
        
        Debug.Log($"{datosEnemigo.nombre} est� Alerta por el jugador esperando estimulo directo");
        Renderer.material.color = Color.yellow;
        
        if (JugadorDentroRango && JugadorDentroDeCampoDeVision())
        {
            estadoActual = EstadoEnemigo.Perseguir;  
        }
        if (!JugadorDentroRango && Temporizador()==0)
        {
            estadoActual = EstadoEnemigo.Vagar; //Si el jugador vuelve a salir del rango del trigger vuelve al estado de Vagar
        }
        // el enemigo debe recibir un estimulo para pasar a Estado Perseguir ( linea de vision (principal por ahora), interaccion con el entorno del jugador, recibir luz de la linterna, recibir alerta de un enemigo que alerta a los demas)

    }

    private void EstadoPerseguir()
    {
        targetEsVisible = true;
        Renderer.material.color = Color.red;
        velocidadEnemigo = velocidadInicialEnemigo; // velocidad para que cuando est� en estado perseguir se mueva hacia el jugador 
        Debug.Log($"{datosEnemigo.nombre} est� Persiguiendo al juagdor esperando a recibir da�o o atacar o perder de vista");
        Vector3 destino;
        destino = target.transform.position; // Perseguir al jugador

        if (!JugadorDentroDeCampoDeVision())
        {
            if (targetEsVisible)
            {
                ultimaPosicionRecordadaTarget = target.transform.position; // Guarda la �ltima posici�n conocida
                targetEsVisible = false;

            }
            // Si el jugador no est� en el campo de visi�n, usar �ltima posici�n conocida
            destino = ultimaPosicionRecordadaTarget;
            SeguirTarget(destino);
            animator.SetFloat("XSpeed", 1, 0.2f, Time.deltaTime);

            if (Temporizador() == temporizadorDetectarJugador/3)
            {
                estadoActual = EstadoEnemigo.Alerta;
                return;
            }
            return;
        }
        // Llamar a SeguirTarget con el destino decidido.
        SeguirTarget(destino);
        animator.SetFloat("XSpeed", 1, 0.2f, Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < distanciaParaDetenerse)
        {
            estadoActual = EstadoEnemigo.Atacar;
        }
    }
    private void EstadoAtacar()
    {
        GirarHacia(target.transform.position);
        Renderer.material.color = Color.blue;
        velocidadEnemigo = 0; // se ajusta velocidad para que el enemigo no se mueva mientras golpea al jugador 
        animator.SetFloat("XSpeed", 2, 0.2f, Time.deltaTime);
        if ( Vector3.Distance(transform.position,target.transform.position) > distanciaParaDetenerse)
        {
            estadoActual = EstadoEnemigo.Perseguir;
        }
    }
    private void EstadoAturdido()
    {
        Debug.Log($"{datosEnemigo.nombre} est� aturdido.");
        //DetectarRangoTarget();
        // Aqu� puedes a�adir un temporizador para el aturdimiento
        // Despu�s de que pase el tiempo, vuelve al estado anterior
    }

    private void EstadoMuriendo()
    {
        Debug.Log($"{datosEnemigo.nombre} est� muriendo.");
        //SerDestruido();
    }


    public void RecibirGolpe(float da�o)
    {
        if (datosEnemigo == null)
        {
            Debug.LogError("El enemigo no tiene datos asignados.");
            return;
        }

        datosEnemigo.vida -= da�o;


        Debug.Log($"{datosEnemigo.nombre} recibi� {da�o} de da�o. Vida restante: {datosEnemigo.vida}");

        // Actualiza la barra de vida en la UI
        if (controladorBarra != null)
        {
            controladorBarra.MostrarBarraDeVida(this); // Configura la barra con el enemigo actual
            controladorBarra.ActualizarBarra(); // Actualiza la vida en la barra
        }

        if (datosEnemigo.vida <= 0)
        {
            SerDestruido(); // Destruir al enemigo
        }

    }
    private void SerDestruido()
    {


        Debug.Log($"{datosEnemigo.nombre} fue derrotado.");

        // Ocultar la barra de vida si este enemigo era el actual
        if (controladorBarra != null)
        {
            controladorBarra.OcultarBarra();
        }
        enemyManager.enemigosActivos.Remove(gameObject) ;
        Destroy(gameObject); // Destruye el objeto cuando la vida llega a 0
        // Aqu� puedes agregar l�gica adicional, como otorgar recompensas al jugador
    }

    private void SeguirTarget(Vector3 destino) //mueve el enemigo hacia el jugador 
    {
        
        if (agente != null)
        {
            agente.SetDestination(destino); // Establecer el destino del agente
            if (Vector3.Distance(agente.transform.position, destino)<0.2f)
            {
                animator.SetFloat("XSpeed", 0, 0.2f, Time.deltaTime); // Animaci�n de idle
            }
        }

    }
    private void GirarHacia(Vector3 destino)
    {
        Vector3 direction = (destino - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Velocidad de giro
    }

    private bool JugadorDentroDeCampoDeVision()
    {
        Vector3 direccionAlJugador  = (target.transform.position - transform.position).normalized;

        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

        // Dibujar el rayo en la escena (de la posici�n del enemigo hacia el jugador)
        Debug.DrawRay(transform.position + new Vector3(0,0.5f,0), direccionAlJugador * rangoDeDeteccion, Color.red);

        // Comprobar si el jugador est� dentro del �ngulo de visi�n
        if (angulo < 60f) // Por ejemplo, un campo de visi�n de 120 grados (60 hacia cada lado)
        {
            // Verificar si hay l�nea de visi�n clara (sin obst�culos)
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direccionAlJugador, out hit, rangoDeDeteccion))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position + Vector3.up, direccionAlJugador * hit.distance, Color.green);
                    Debug.Log("El jugador es visible");
                    return true; // Jugador visible
                    
                }
            }
            
        }
        Debug.Log("El jugador NO es visible");
        return false;
    }


    private float Temporizador()
    {
        if (temporizadorDetectarJugador == 0)
        {
            temporizadorDetectarJugador = recordarPosPorTiempo;
        }
        // Reduce el temporizador real de la clase
        temporizadorDetectarJugador -= Time.deltaTime;

        // Asegurarse de que no sea menor a 0
        if (temporizadorDetectarJugador < 0)
        {
            temporizadorDetectarJugador = 0;

        }

        Debug.Log("El tiempo restante es: " + temporizadorDetectarJugador);

        // Retornar el valor actualizado del temporizador
        return temporizadorDetectarJugador;
    
   }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorDentroRango = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorDentroRango = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorDentroRango = false;
        }
    }

}
    
