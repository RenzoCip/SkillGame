using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float sensibilidadMouseX; // Sensibilidad horizontal del mouse
    public float sensibilidadMouseY; // Sensibilidad Vertical del mouse
    private float rotacionY = 0f; // Controla la inclinación vertical de la cámara
    public float speed; // velocidad del juador

    public Transform camara; // referencia a la camara del jugador
    private Rigidbody rb;   // referencia al rigidbody del jugador

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; 
        Cursor.lockState = CursorLockMode.Locked;  // Bloquear el raton
    }
    void FixedUpdate() //para trabajar con fisicas
    {
        MoverJugador(); // Manejar movimiento con físicas
        RotarJugador(); // Manejar rotación
    }

    public void MoverJugador()
    {
        // recibe el movimiento con WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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
        rotacionY -= mouseY;
        rotacionY = Mathf.Clamp(rotacionY, -90, 90);
        camara.localRotation = Quaternion.Euler(rotacionY, 0, 0);

        // Rotación horizontal del jugador
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0,mouseX,0));
    }
}
