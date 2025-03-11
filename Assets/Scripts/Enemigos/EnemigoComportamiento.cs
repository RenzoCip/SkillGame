using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.ShaderData;
public class EnemigoComportamiento : MonoBehaviour
{

    public enum EstadoEnemigo
    {
        Idle,
        Vagar,
        Alerta,
        Perseguir,
        Buscando,
        Atacar,
        Aturdido,
        Muriendo
    }
    public EstadoEnemigo estadoActual = EstadoEnemigo.Idle;


    //  DATOS DEL ENEMIGO Y COMPONENTES PRINCIPALES
    public Enemigo datosEnemigo; // Contiene estadísticas del enemigo (vida, daño, etc.)
    private EnemyManager enemyManager; // Referencia al sistema que maneja a los enemigos
    private ControladorBarraDeVidaEnemigo controladorBarra; // Controlador de la barra de vida del enemigo
    private ControladorEstadoJugador estadoJugador; // Estado del jugador
    private NavMeshAgent agente; // Componente de navegación del enemigo
    private Animator animator; // Controlador de animaciones
    private Renderer Renderer; // Material del enemigo para cambiar colores según el estado

    //  OBJETIVOS Y POSICIONES CLAVE
    public GameObject target; // Referencia al jugador
    public GameObject estadoEnemigo; // Objeto que visualiza el estado del enemigo
    public Vector3 spawnPoint; // Punto inicial donde aparece el enemigo
    private Vector3 ultimaDireccionJugador; // Última dirección en la que se movió el jugador
    private Vector3 ultimaPosicionRecordadaTarget; // Última posición confirmada del jugador
    private Vector3 supuestaPosicionTarget; // Posición estimada donde podría estar el jugador
    private Vector3 posicionAtasco; // Última posición registrada cuando el enemigo quedó atascado
    private Vector3 ultimaPosicion; // Última posición del enemigo antes de verificar movimiento

    //  CONFIGURACIÓN DE MOVIMIENTO Y DETECCIÓN
    public float velocidadEnemigo; // Velocidad base del enemigo
    private float velocidadInicialEnemigo; // Velocidad inicial antes de posibles modificaciones
    public float rangoDeDeteccion; // Rango en el que puede detectar al jugador
    public float distanciaParaDetenerse; // Distancia a la que el enemigo deja de moverse cuando alcanza su destino
    public float recordarPosPorTiempo; // Tiempo en el que el enemigo recordará la última posición del jugador

    //  VARIABLES DE DETECCIÓN Y ALERTA
    private float velocidadJugador; // Velocidad actual del jugador


    static bool jugadorDetectado;


    private bool JugadorDentroRango; // Si el jugador está dentro del área de detección
    private bool targetEsVisible; // Si el enemigo tiene línea de visión con el jugador
    private bool targetDetectado; // Si el enemigo ha detectado al jugador
    private bool persistente; // Si el enemigo nunca pierde al jugador
    private bool puedeResucitar; // Si el enemigo puede volver a la vida tras ser derrotado

    //  CONTROL DE ESTADOS INTERMEDIOS
    private bool devolverse; // Si el enemigo debe volver a su spawn
    private bool enUltimaPosJugador; // Si el enemigo está en la última posición conocida del jugador
    private bool enSupuestaPosJugador; // Si el enemigo ha llegado a la posición predicha del jugador
    private bool cicloCompleto; // Si el ciclo de búsqueda ha terminado
    public bool paso1, paso2, paso3, paso4; // Pasos del ciclo de búsqueda

    //  TEMPORIZADORES PARA DIFERENTES ACCIONES
    public float tiempoDeBusquedaInicial = 6f; // Tiempo total que el enemigo buscará al jugador
    private float tiempoDeBusqueda; // Temporizador de búsqueda
    private float tiempoPerdidaVistaInicial = 2f; // Tiempo antes de que el enemigo estime una nueva posición del jugador
    private float tiempoPerdidaVista; // Temporizador de pérdida de visión
    public float temporizadorDetectarJugador; // Temporizador para que el enemigo deje de buscar al jugador

    //  VARIABLES PARA DETECCIÓN DE ATASCO
    private float tiempoAtascoInicial = 0.7f; // Tiempo de espera antes de considerar que el enemigo está atascado
    private float tiempoAtasco; // Contador para medir si el enemigo ha estado sin moverse
    private bool permitirComparacion; // Permite comparar la posición para detectar atasco
    private bool enemigoAtascado; // Indica si el enemigo está atascado
    // Start is called before the first frame update
    void Start()
    {
        permitirComparacion = false;
        tiempoAtasco = tiempoAtascoInicial;
        tiempoDeBusqueda = tiempoDeBusquedaInicial;
        agente = GetComponent<NavMeshAgent>(); // Inicializar el agente
        estadoJugador = FindAnyObjectByType<ControladorEstadoJugador>();
        target = GameObject.FindWithTag("Player");
        if (target == null)
        {
            Debug.LogError(" ERROR: No se encontró un objeto con la etiqueta 'Player'.");
        }
        else
        {
            Debug.Log($" Player encontrado: {target.name}");
        }
        animator = GetComponent<Animator>();
        controladorBarra = FindObjectOfType<ControladorBarraDeVidaEnemigo>();
        enemyManager = FindObjectOfType<EnemyManager>();

        Renderer = estadoEnemigo.GetComponent<Renderer>();

        //animator.SetTrigger("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadAnimator = animator.GetFloat("XSpeed");
        Debug.Log("Jugador Detectado Static:" + jugadorDetectado);
        //Debug.Log($"El valor actual de 'XSpeed' en el Animator es: {velocidadAnimator}");

        switch (estadoActual)
        {
            case EstadoEnemigo.Idle: // <<<!!! Estado Incial- estará quieto y haciendo ruidos pero es un estado previo a todo lo que implica comportamientos, debe ser cambiado siempre de manera mecanica  !!!<<<
                EstadoIdle();

                break;

            case EstadoEnemigo.Vagar:// Pasa siempre del Estado Idel a Vagar de manera mecanica, hay que llamarlo , se mantiende haciendo algo sumamente basico esperando que el jugador entre en el area trigger para cambiar a estado de Alerta
                EstadoVagar();

                break;

            case EstadoEnemigo.Alerta: // jugador entra en el area trigger , se activa Estado Alerta y enemigo espera un estimulo para pasar a Estado Perseguir ( linea de vision, interaccion con el entorno del jugador, recibir luz de la linterna, recibir alerta de un enemigo que alerta a los demas)
                EstadoAlerta();

                break;

            case EstadoEnemigo.Perseguir: // el enemigo detectó al jugador mediante un estimulo, se activa Estado Perseguir y cuando esté suficientemnete cerca pasa a Estado Atacar o si recibe daño a Estado Golpeado
                EstadoPerseguir();
                break;
            case EstadoEnemigo.Buscando: // cuando el enemigo en perseguir pierde devista al player viene a buscar, si no ves al jugador en un tiempo de reconmoiento vueloves al spawn lentamente 

                EstadoBuscando();
                break;

            case EstadoEnemigo.Atacar: //en Estado Atacar el enemigo realiza un ataque, puede pasar a Estado Perseguir si el jugador se aleja o Estado Golpeado si recibe daño
                EstadoAtacar();
                break;

                //case EstadoEnemigo.Golpeado: //en Estado Golpeado el enemigo se estunea y recibe daño, puede pasar a Estado Atacar si el jugador sigue cerca o a Estado Perseguir si jugador se alejó o a Estado Morir si vida llega a Cero
                //EstadoGolpeado);
                //break;

                // case EstadoEnemigo.Morir: // en estado morir vida es cero y animacion y sonido de morir 
                //   EstadoMorir();
                // break;
        }
    }

    private void EstadoIdle()
    {
        //Debug.Log($"{datosEnemigo.nombre} está en reposo.");
        Renderer.material.color = Color.white;
        estadoActual = EstadoEnemigo.Vagar;
        // estadoEnemigo.
        //animacion basica 

    }
    private void EstadoVagar()
    {
        devolverse = false;
        //Debug.Log($"{datosEnemigo.nombre} está Vagueando espoerando que el jugador entre en el trigger");
        Renderer.material.color = Color.green;

        animator.SetFloat("XSpeed", 0, 0.2f, Time.deltaTime);
        //agregar animacion de lo que sea que esté haciendo y cubito de colores para ver su estado 
        //si acaba de volver del estado perseguir debe volver a su posicion original 
        // Mover hacia el punto de spawn si no está en él

        if (JugadorDentroRango)
        {
            //Debug.Log("el jugador esta en el trigger");
            estadoActual = EstadoEnemigo.Alerta;
        }
        if (jugadorDetectado)
        {
            estadoActual = EstadoEnemigo.Perseguir;
        }

    }
    private void EstadoAlerta() // HAY QUE AGREGAR QUE SI EL JUGADOR SALE DEL TRIGGER VOLVER AL ESTADO VAGAR
    {

        //Debug.Log($"{datosEnemigo.nombre} está Alerta por el jugador esperando estimulo directo");
        animator.SetFloat("XSpeed", 0, 0.2f, Time.deltaTime);
        Renderer.material.color = Color.yellow;

        if (JugadorDentroRango && JugadorDentroDeCampoDeVision(60) || jugadorDetectado)
        {
            estadoActual = EstadoEnemigo.Perseguir;
        }
        // el enemigo debe recibir un estimulo para pasar a Estado Perseguir ( linea de vision (principal por ahora), interaccion con el entorno del jugador, recibir luz de la linterna, recibir alerta de un enemigo que alerta a los demas)

    }

    public void EstadoPerseguir()
    {
        ReiniciarEstadoBuscando();

        enUltimaPosJugador = false;
        enSupuestaPosJugador = false;
        targetEsVisible = true;
        Renderer.material.color = Color.red;
        // Debug.Log($"{datosEnemigo.nombre} está persiguiendo al jugador");

        Vector3 destinoJugador = target.transform.position;

        // Si el jugador está en el campo de visión:
        if (JugadorDentroDeCampoDeVision(120) || jugadorDetectado)
        {
            // Si el jugador sigue visible, moverse directamente a su posición.
            SeguirTarget(destinoJugador, velocidadEnemigo, 1.1f); //seguir jugador

            ultimaPosicionRecordadaTarget = target.transform.position;


            // ultimaDireccionJugador = target.transform.forward;
            //velocidadJugador = target.GetComponent<MovimientoJugador>().speed;

        }
        else // Si se pierde la visión, calcula la posición predicha
        {
            TemporizadorGuardarSupuestaPos();

            return;
        }


        if (Vector3.Distance(transform.position, target.transform.position) < distanciaParaDetenerse)
        {
            estadoActual = EstadoEnemigo.Atacar;
        }
    }


    private void EstadoBuscando()
    {

        Debug.Log("Paso1" + paso1);
        Debug.Log("Paso2" + paso2);
        Debug.Log("Paso3" + paso3);
        Debug.Log("Paso4" + paso4);
        if (paso1 && paso2 && paso3 && paso4)
        {
            cicloCompleto = true;
        }
        Renderer.material.color = Color.magenta;
        if (!paso1 & !paso2 & !paso3 & !paso4)
        {

            SeguirTarget(ultimaPosicionRecordadaTarget, velocidadEnemigo, 1.1f);
            // Debug.Log("el enemigo esta yendo a la ultima posicion conocida");
        }

        if (Vector3.Distance(transform.position, ultimaPosicionRecordadaTarget) < 1.5f)
        {
            paso1 = true;

            //Debug.Log("el enemigo llegó a la ultima posicion vista del jugador ");
        }

        if (paso1 && !paso2 && !paso3 && !paso4 || jugadorDetectado)
        {

            SeguirTarget(supuestaPosicionTarget, velocidadEnemigo, 1.1f);
            // Debug.Log("el enemigo esta yendo a la SUPUESTA posicion conocida");
        }

        if (Vector3.Distance(transform.position, supuestaPosicionTarget) < 1.5f)
        {
            paso2 = true;
            // Debug.Log("el enemigo llegó a la supuesta posicion del jugador ");

            TemporizadorBusqueda();
            //  Debug.Log("tiempo de busqueda es" + tiempoDeBusqueda);
        }

        if (tiempoDeBusqueda <= 0.2f)
        {
            paso3 = true;

        }

        if (paso1 && paso2 && paso3 && !paso4)
        {
            SeguirTarget(spawnPoint, velocidadEnemigo / 2, 0.1f);

        }

        if (Vector3.Distance(transform.position, spawnPoint) < 0.5f)
        {
            paso4 = true;
            // Debug.Log("el enemigo llegó al spawnPoint");

        }
        if (cicloCompleto)
        {
            estadoActual = EstadoEnemigo.Vagar;
        }
        if (JugadorDentroDeCampoDeVision(120) || jugadorDetectado)
        {
            estadoActual = EstadoEnemigo.Perseguir;
            // Debug.Log("Jugador detectado, se reanuda la persecución.");
        }

    }


    private void EstadoAtacar()
    {
        jugadorDetectado = true;
        GirarHacia(target.transform.position);
        //Debug.Log(("atacando"));
        Renderer.material.color = Color.blue;
        // velocidadEnemigo = 0; // se ajusta velocidad para que el enemigo no se mueva mientras golpea al jugador 
        animator.SetFloat("XSpeed", 2, 0.2f, Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) > distanciaParaDetenerse)
        {
            estadoActual = EstadoEnemigo.Perseguir;
        }
    }
    private void EstadoAturdido()
    {
        Debug.Log($"{datosEnemigo.nombre} está aturdido.");
        //DetectarRangoTarget();
        // Aquí puedes añadir un temporizador para el aturdimiento
        // Después de que pase el tiempo, vuelve al estado anterior
    }

    private void EstadoMuriendo()
    {
        Debug.Log($"{datosEnemigo.nombre} está muriendo.");
        //SerDestruido();
    }

    private void ReiniciarEstadoBuscando()
    {
        paso1 = false;
        paso2 = false;
        paso3 = false;
        paso4 = false;
        cicloCompleto = false;
    }

    private void TemporizadorGuardarSupuestaPos()
    {
        tiempoPerdidaVista -= Time.deltaTime;
        Debug.Log("El temporizador de la segunda posicion va por " + tiempoPerdidaVista);
        if (tiempoPerdidaVista < 0)
        {
            supuestaPosicionTarget = target.transform.position;
            tiempoPerdidaVista = tiempoPerdidaVistaInicial;
            estadoActual = EstadoEnemigo.Buscando;
            jugadorDetectado = false;
        }
    }

    private void TemporizadorBusqueda()
    {
        tiempoDeBusqueda -= Time.deltaTime;
        if (tiempoDeBusqueda < 0)
        {
            tiempoDeBusqueda = tiempoDeBusquedaInicial;
        }
    }
    public void RecibirGolpe(float daño)
    {
        if (datosEnemigo == null)
        {
            Debug.LogError("El enemigo no tiene datos asignados.");
            return;
        }

        datosEnemigo.vida -= daño;
        animator.SetTrigger("EnemigoDolor");


        Debug.Log($"{datosEnemigo.nombre} recibió {daño} de daño. Vida restante: {datosEnemigo.vida}");


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
        enemyManager.enemigosActivos.Remove(gameObject);
        Destroy(gameObject); // Destruye el objeto cuando la vida llega a 0
        // Aquí puedes agregar lógica adicional, como otorgar recompensas al jugador
    }

    private void SeguirTarget(Vector3 destino, float velocidad, float distanciaParaDetenerse)
    {
        animator.SetFloat("XSpeed", 1, 0.2f, Time.deltaTime);
        if (agente != null)
        {
            agente.SetDestination(destino);
            agente.speed = velocidad;
            agente.stoppingDistance = MathF.Max(1.2f, distanciaParaDetenerse);
            //ComprobarAtasco();
        }
    }
    private void ComprobarAtasco()
    {
        TemporizadorAtasco();


        if (tiempoAtasco < 0.2f && !permitirComparacion)
        {

            permitirComparacion = true;

            //Debug.Log("la posicion nueva es:" + posicionAtasco);
        }
        if (permitirComparacion)
        {
            float distancia = Vector3.Distance(transform.position, posicionAtasco);
            //Debug.Log("La distancia entre la posición actual y la posición de atasco es: " + distancia);
            if (distancia < 0.01f)
            {
                Debug.Log("el enemigo esta atascado");
                enemigoAtascado = true;
            }
        }
    }
    private void TemporizadorAtasco()
    {
        tiempoAtasco -= Time.deltaTime;
        if (tiempoAtasco < 0)
        {
            tiempoAtasco = tiempoAtascoInicial;
            posicionAtasco = transform.position; // Actualizamos la posición de referencia para el próximo ciclo
            permitirComparacion = false;           // Reiniciamos la bandera para la próxima comprobación
        }
    }


    private void GirarHacia(Vector3 destino)
    {
        Vector3 direction = (destino - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Velocidad de giro
    }
    private bool JugadorDentroDeCampoDeVision(float rangoDeVision)
    {
        Vector3 direccionAlJugador = (target.transform.position - transform.position).normalized;

        float angulo = Vector3.Angle(transform.forward, direccionAlJugador);

        // Dibujar el rayo en la escena (de la posición del enemigo hacia el jugador)
        // **Dibujar los límites del campo de visión**
        Vector3 direccionRayoIzquierdo = Quaternion.Euler(0, -rangoDeVision, 0) * transform.forward;
        Vector3 direccionRayoDerecho = Quaternion.Euler(0, rangoDeVision, 0) * transform.forward;

        Debug.DrawRay(transform.position + Vector3.up, direccionRayoIzquierdo * rangoDeDeteccion, Color.cyan); // Límite izquierdo
        Debug.DrawRay(transform.position + Vector3.up, direccionRayoDerecho * rangoDeDeteccion, Color.cyan);  // Límite derecho
        Debug.DrawRay(transform.position + Vector3.up, transform.forward * rangoDeDeteccion, Color.blue);  // Rayo central
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), direccionAlJugador * rangoDeDeteccion, Color.red);

        // Comprobar si el jugador está dentro del ángulo de visión
        if (angulo < rangoDeVision) // Por ejemplo, un campo de visión de 120 grados (60 hacia cada lado)
        {
            // Verificar si hay línea de visión clara (sin obstáculos)
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direccionAlJugador, out hit, rangoDeDeteccion))
            {
                //Debug.Log("El rayo impactó contra: " + hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Cuerpo"))
                {
                    Debug.DrawRay(transform.position + Vector3.up, direccionAlJugador * hit.distance, Color.green);
                    Debug.Log("El jugador es visible");
                    jugadorDetectado = true;
                    return true; // Jugador visible

                }
                else
                {
                    Debug.Log("el jugador no es visible");
                    jugadorDetectado = false;
                    return false;
                }
            }

        }
        //Debug.Log("El jugador NO es visible");
        return false;
    }
    public void AtacarJugador()
    {
        if (estadoJugador != null)
        {

            estadoJugador.vidaJugadorRest -= datosEnemigo.daño;
            estadoJugador.AnimarDaño();
            Debug.Log("daño");

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cuerpo"))
        {
            JugadorDentroRango = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cuerpo"))
        {
            JugadorDentroRango = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cuerpo"))
        {
            JugadorDentroRango = false;
        }
    }

}


