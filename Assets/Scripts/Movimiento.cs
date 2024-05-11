using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public float Velocidad; // Velocidad horizontal del personaje
    public float FuerzaSalto; // Fuerza del salto
    public AudioClip sonidoMovimiento; // Sonido de movimiento
    public AudioClip sonidoKonami; // Sonido del c�digo Konami

    private Rigidbody2D Rigidbody2D; // Referencia al componente Rigidbody2D
    private Animator Animator; // Referencia al componente Animator
    private AudioSource audioSource; // Componente de audio general
    private float Horizontal; // Valor de entrada horizontal
    private bool Grounded; // Estado de si el personaje est� en el suelo
    private AudioSource audioSourceMovimiento; // Componente de audio para el sonido de movimiento
    private AudioSource audioSourceKonami; // Componente de audio para el sonido del c�digo Konami

    private int indiceTeclaActual = 0; // �ndice de la tecla actual en el c�digo Konami
    private readonly KeyCode[] codigoKonami = { // C�digo Konami
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
        audioSourceKonami = audioSources[1]; // Asigna el segundo AudioSource para el sonido del c�digo Konami
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal") * Velocidad; // Obt�n la entrada horizontal

        // Reproducir sonido de movimiento si se est� moviendo horizontalmente y en el suelo
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
            audioSourceMovimiento.Stop(); // Detiene el sonido de movimiento si no se est� moviendo horizontalmente o no est� en el suelo
        }

        // Verificar el c�digo Konami
        if (Input.GetKeyDown(codigoKonami[indiceTeclaActual]))
        {
            indiceTeclaActual++;
            if (indiceTeclaActual >= codigoKonami.Length) // Si se complet� el c�digo Konami
            {
                MostrarMensajeKonami(); // Muestra un mensaje en la consola
                indiceTeclaActual = 0; // Reinicia el �ndice del c�digo Konami
                audioSourceKonami.clip = sonidoKonami; // Asigna el sonido del c�digo Konami al AudioSource
                audioSourceKonami.Play(); // Reproduce el sonido del c�digo Konami
            }
        }
        else if (Input.anyKeyDown)
        {
            indiceTeclaActual = 0; // Reinicia el �ndice si se presiona una tecla incorrecta
        }

        // Cambiar la escala del personaje seg�n su direcci�n horizontal
        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-1.0f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(1.0f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Actualizar la animaci�n de correr
        Animator.SetBool("Running", Horizontal != 0.0f);

        // Verificar si el personaje est� en el suelo
        if (Physics2D.Raycast(transform.position, Vector3.down, 1.3f))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }

        // Saltar si se presiona la tecla W y el personaje est� en el suelo
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }
        Animator.SetBool("Saltar", !Grounded); // Actualizar la animaci�n de saltar en funci�n del estado de estar en el suelo
    }

    // M�todo para realizar el salto
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * FuerzaSalto);
    }

    // M�todo para actualizar la f�sica del personaje
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal, Rigidbody2D.velocity.y);
    }

    // M�todo para mostrar un mensaje cuando se activa el c�digo Konami
    void MostrarMensajeKonami()
    {
        Debug.Log("C�digo Konami Activado");
    }

}
