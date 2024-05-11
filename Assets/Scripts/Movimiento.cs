using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float Velocidad; // Velocidad horizontal del personaje
    public float FuerzaSalto; // Fuerza del salto
    public AudioClip sonidoMovimiento; // Sonido de movimiento
    public AudioClip sonidoKonami; // Sonido del código Konami

    private Rigidbody2D Rigidbody2D; // Referencia al componente Rigidbody2D
    private Animator Animator; // Referencia al componente Animator
    private AudioSource audioSource; // Componente de audio general
    private float Horizontal; // Valor de entrada horizontal
    private bool Grounded; // Estado de si el personaje está en el suelo
    private AudioSource audioSourceMovimiento; // Componente de audio para el sonido de movimiento
    private AudioSource audioSourceKonami; // Componente de audio para el sonido del código Konami

    private int indiceTeclaActual = 0; // Índice de la tecla actual en el código Konami
    private readonly KeyCode[] codigoKonami = { // Código Konami
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>(); // Obtiene el componente Rigidbody2D del GameObject
        Animator = GetComponent<Animator>(); // Obtiene el componente Animator del GameObject
        AudioSource[] audioSources = GetComponents<AudioSource>(); // Obtiene todos los componentes AudioSource del GameObject
        audioSourceMovimiento = audioSources[0]; // Asigna el primer AudioSource para el sonido de movimiento
        audioSourceKonami = audioSources[1]; // Asigna el segundo AudioSource para el sonido del código Konami
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal") * Velocidad; // Obtén la entrada horizontal

        // Reproducir sonido de movimiento si se está moviendo horizontalmente y en el suelo
        if (Horizontal != 0.0f && Grounded)
        {
            audioSourceMovimiento.clip = sonidoMovimiento;
            if (!audioSourceMovimiento.isPlaying)
            {
                audioSourceMovimiento.Play();
            }
        }
        else
        {
            audioSourceMovimiento.Stop(); // Detiene el sonido de movimiento si no se está moviendo horizontalmente o no está en el suelo
        }

        // Verificar el código Konami
        if (Input.GetKeyDown(codigoKonami[indiceTeclaActual]))
        {
            indiceTeclaActual++;
            if (indiceTeclaActual >= codigoKonami.Length) // Si se completó el código Konami
            {
                MostrarMensajeKonami(); // Muestra un mensaje en la consola
                indiceTeclaActual = 0; // Reinicia el índice del código Konami
                audioSourceKonami.clip = sonidoKonami; // Asigna el sonido del código Konami al AudioSource
                audioSourceKonami.Play(); // Reproduce el sonido del código Konami
            }
        }
        else if (Input.anyKeyDown)
        {
            indiceTeclaActual = 0; // Reinicia el índice si se presiona una tecla incorrecta
        }

        // Cambiar la escala del personaje según su dirección horizontal
        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1.0f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(1.0f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Actualizar la animación de correr
        Animator.SetBool("Running", Horizontal != 0.0f);

        // Verificar si el personaje está en el suelo
        if (Physics2D.Raycast(transform.position, Vector3.down, 1.3f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        // Saltar si se presiona la tecla W y el personaje está en el suelo
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }
        Animator.SetBool("Saltar", !Grounded); // Actualizar la animación de saltar en función del estado de estar en el suelo
    }

    // Método para realizar el salto
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
    }

    // Método para actualizar la física del personaje
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }

    // Método para mostrar un mensaje cuando se activa el código Konami
    void MostrarMensajeKonami()
    {
        Debug.Log("Código Konami Activado");
    }

}
