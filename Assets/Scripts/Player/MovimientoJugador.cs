using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float sensibilidadMouseX; // Sensibilidad horizontal del mouse
    public float sensibilidadMouseY; // Sensibilidad Vertical del mouse
    public float rotacionY = 0f; // Controla la inclinación vertical de la cámara
    public float speed; // velocidad del juador
    private float rotacionHaciaAbajo = 40; //limite de rotacion de la camara hacia abajo
    private float rotacionHaciaArriba = 0; // limite de rotacion de la camara hacia arriba
    public float rotacionHaciaAbajoShootMode;
    public float rotacionHaciaArribaShootMode;
    public Transform camara; // referencia a la camara del jugador
    private Rigidbody rb;   // referencia al rigidbody del jugador
    private GameManager gameManager;
    private Animator animator;
    public Canvas canvas;
    public GameObject posShootPoint; // Posición objetivo para el modo shoot
    public GameObject posWalkPoint; // Posición objetivo para el modo caminar

    public float velocidadTransicionCamara; // Velocidad de transición para el movimiento de la cámara

    private Vector3 posShootModeCam; // Posición almacenada para el modo shoot
    private Vector3 posWalkModeCam; // Posición almacenada para el modo caminar

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 

        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el raton
        gameManager = FindAnyObjectByType<GameManager>();
    }
    private void Update()
    {
        ActivarModoShoot();
        GuardarPosicionCamara();
    }
    private void LateUpdate()
    {
        MoverCamaraModoShoot();

    }
    void FixedUpdate() //para trabajar con fisicas
    {
        MoverJugador(); // Manejar movimiento con físicas
        RotarJugador(); // Manejar rotación
    }
    public void ActivarModoShoot()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameManager.isInShootMode= true;
            animator.SetBool("ShootMode",true);
            canvas.gameObject.SetActive(true);
            Debug.Log("Clic derecho presionado y modo shoot activo");
            
        }
        if (Input.GetMouseButtonUp(1))
        {
            gameManager.isInShootMode = false;
            animator.SetBool("ShootMode", false);
            canvas.gameObject.SetActive(false);
            Debug.Log("Clic derecho soltado y modo shoot inactivo");
            
        }
    }
    public void MoverJugador()
    {
        // recibe el movimiento con WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        animator.SetFloat("XSpeed",horizontal,0.2f, Time.deltaTime);
        animator.SetFloat("YSpeed",vertical,0.2f, Time.deltaTime);
        // Calcula el movimiento relativo a la dirección actual del jugador
        Vector3 movimiento = transform.forward * vertical + transform.right * horizontal;
        rb.MovePosition(transform.position + movimiento * speed * Time.fixedDeltaTime);
    }
    public void RotarJugador()
    {
        //recibe el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouseX * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouseY *Time.fixedDeltaTime;

        // Rotación vertical de la cámara
        if (!gameManager.isInShootMode)
        {
            rotacionY -= mouseY;
            rotacionY = Mathf.Clamp(rotacionY, rotacionHaciaArriba, rotacionHaciaAbajo);
            camara.localRotation = Quaternion.Euler(rotacionY, 0, 0);
        }
        if (gameManager.isInShootMode)
        {
            rotacionY -= mouseY;
            rotacionY = Mathf.Clamp(rotacionY, rotacionHaciaArribaShootMode, rotacionHaciaAbajoShootMode);
            camara.localRotation = Quaternion.Euler(rotacionY, 0, 0);
        }


        // Rotación horizontal del jugador
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0,mouseX,0));
    }

    public void MoverCamaraModoShoot()
    {

        if (gameManager.isInShootMode)
        {
            camara.position = Vector3.Lerp(camara.position, posShootModeCam, Time.deltaTime * velocidadTransicionCamara);
           
        }
        if (!gameManager.isInShootMode)
        {
            camara.position = Vector3.Lerp(camara.position, posWalkModeCam, Time.deltaTime * velocidadTransicionCamara);
        }
    }
    public void GuardarPosicionCamara()
    {
        posWalkModeCam = posWalkPoint.transform.position; 
        posShootModeCam = posShootPoint.transform.position;
    }
}
