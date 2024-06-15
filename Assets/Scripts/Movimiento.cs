using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Movimiento : MonoBehaviour
{
    public float Velocidad;
    public float AlturaSalto;
    public float FuerzaSalto;
    public bool Saltando;
    public float Fallen;
    public Vector3 v3;
    public float distance;
    public LayerMask layer;
    public AudioClip sonidoMovimiento;
    public AudioClip sonidoKonami;
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoDaño;
    public Animator Animator;
    public bool Daño_;
    public int Empuje;
    public float VidaMax;
    public float VidaMin;
    public Image Barra;
    private int muerto;
    private int combo;
    public bool accion;
    public AudioSource combos;
    public AudioClip[] sonido;
    public Vector3 ray_pose;
    public float distance2;
    public bool slide;
    public bool Saltando2;
    public float delay;
    public bool airAttack;


    private bool atacando;
    private RaycastHit2D hit;
    private RaycastHit2D hit2;
    private RaycastHit2D hit3;
    private float Ypos;
    private int sky_;
    public float Gravedad;
    private int Fase1;
    private int Fase2;
    private Rigidbody2D Rigidbody2D;
    private AudioSource audioSourceMovimiento;
    private AudioSource audioSourceKonami;
    private AudioSource audioSourceSalto;
    public AudioSource audioSourceMuerte;
    public AudioSource audioSourceDaño;
    private int indiceTeclaActual = 0;
    private readonly KeyCode[] codigoKonami = {
        KeyCode.UpArrow, KeyCode.UpArrow,
        KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.B, KeyCode.A
    };

    void Start()
    {
        InicializarComponentes();
    }

    void Update()
    {
        Vida();
        if (VidaMin > 0)
        {
            Daño();
            if (!Daño_)
            {
                Golpe_Aereo();
                WallJump();
                VerificarCodigoKonami();
                Combos_();
                Detector_Plataforma();
            }
        }
        else
        {
            switch (muerto)
            {
                case 0:
                    Animator.SetTrigger("muerto");
                    audioSourceMovimiento.Stop();
                    audioSourceKonami.Stop();
                    muerto++;
                    if (!audioSourceMuerte.isPlaying)
                    {
                        audioSourceMuerte.clip = sonidoMuerte;
                        audioSourceMuerte.Play();
                    }
                    StartCoroutine(RestartGameAfterDelay(5));
                    break;
            }

        }
    }

    private void FixedUpdate()
    {
        if (VidaMin > 0)
        {
            if (!Daño_)
            {
                Move();
                Jump();
            }
        }

        if (Saltando2 && CheckCollision3)
        {
            transform.Translate(Vector3.left * Gravedad * 1.5f * Time.deltaTime);
            transform.Translate(Vector3.up * Gravedad * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * Gravedad * Time.deltaTime);
        }
    }

    private void InicializarComponentes()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSourceMovimiento = audioSources[0];
        audioSourceKonami = audioSources[1];
        audioSourceSalto = audioSources[2];
        audioSourceMuerte = audioSources[3]; 
        audioSourceDaño = audioSources[4];
    }

    private void VerificarCodigoKonami()
    {
        if (Input.GetKeyDown(codigoKonami[indiceTeclaActual]))
        {
            indiceTeclaActual++;
            if (indiceTeclaActual >= codigoKonami.Length)
            {
                MostrarMensajeKonami();
                indiceTeclaActual = 0;
                audioSourceKonami.clip = sonidoKonami;
                audioSourceKonami.Play();
            }
        }
        else if (Input.anyKeyDown)
        {
            indiceTeclaActual = 0;
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.W) && !accion && !atacando)
        {
            combo = 0;

            switch (Fase1)
            {
                case 0:
                    if (CheckCollision)
                    {
                        Gravedad = AlturaSalto;
                        Fase1 = 1;
                        Saltando = true;
                        PlayJumpSound(); // Inicia la reproducción del sonido al inicio del salto
                    }
                    if (slide && delay <= 0)
                    {
                        Gravedad = AlturaSalto;
                        Fase1 = 1;
                        Saltando = true;
                        Saltando2 = true;
                        delay = 1;
                        PlayJumpSound(); // Inicia la reproducción del sonido al inicio del salto
                    }
                    break;
                case 1:
                    if (Gravedad > 0)
                    {
                        Gravedad -= FuerzaSalto * Time.deltaTime;
                    }
                    else
                    {
                        Fase1 = 2;
                    }
                    Saltando = true;
                    if (slide)
                    {
                        Saltando2 = true;
                    }
                    break;
                case 2:
                    Saltando = false;
                    Saltando2 = false;
                    break;
            }
        }
        else
        {
            Saltando = false;
            Saltando2 = false;
        }
    }

    private void PlayJumpSound()
    {
        if (!audioSourceSalto.isPlaying)
        {
            audioSourceSalto.clip = sonidoSalto;
            audioSourceSalto.Play();
        }
        else
        {
            audioSourceSalto.Stop();
            audioSourceSalto.Play();
        }
    }

    private void MostrarMensajeKonami()
    {
        Debug.Log("Código Konami Activado");
    }

    public void Daño()
    {
        if (Daño_)
        {
            transform.Translate(Vector3.right * Empuje * Time.deltaTime, Space.World);
            if (!audioSourceDaño.isPlaying)
            {
                audioSourceDaño.clip = sonidoDaño;
                audioSourceDaño.Play();
            }
        }
    }


    public void TerminarDaño()
    {
        Daño_ = false;
    }

    public void Vida()
    {
        Barra.fillAmount = VidaMin / VidaMax;
    }

    IEnumerator RestartGameAfterDelay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Cronometro()
    { 
        yield return new WaitForSeconds(.1f);
        accion = false;
    }

    public void PlayON()
    {
        StartCoroutine(Cronometro());
    }

    public void Combos_()
    {
        if (Input.GetKeyDown(KeyCode.C) && !atacando && CheckCollision)
        {
            atacando = true;
            accion = true;
            Animator.SetTrigger("" + combo);
            combos.clip = sonido[combo];
            combos.Play();
            Animator.SetBool("run", false);
        }
    }

    public void Start_Combo()
    {
        atacando = false;
        if (combo < 3)
        {
            combo++;
        }
    }

    public void Stop_Combo()
    {
        atacando = false;
        combo = 0;
        PlayON();
    }

    public void Golpe_Aereo()
    {
        if (Input.GetKeyDown(KeyCode.C) && !airAttack && !CheckCollision && !slide)
        { 
            airAttack = true;
            Animator.SetTrigger("air");
            combos.clip = sonido[combo];
            combos.Play();
        }
    }

    public void Final_Air()
    {
        airAttack = false;
    }



    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + v3, Vector3.up * -1 * distance);
        Gizmos.DrawRay(transform.position + ray_pose, transform.right * distance2);
    }

    public bool CheckCollision
    {
        get
        {
            hit = Physics2D.Raycast(transform.position + v3, transform.up * -1, distance, layer);
            return hit.collider != null;
        }
    }

    public bool CheckCollision2
    {
        get
        {
            hit2 = Physics2D.Raycast(transform.position + ray_pose, transform.right, distance2, layer);
            return hit2.collider != null;
        }
    }

    public bool CheckCollision3
    {
        get
        {
            hit3 = Physics2D.Raycast(transform.position + ray_pose, transform.right, distance2 * 2, layer);
            return hit3.collider != null;
        }
    }

    public void Detector_Plataforma()
    {
        if (CheckCollision || slide)
        {
            airAttack = false;
            Animator.SetBool("sky", false);
            sky_ = 0;
            if (!Saltando)
            {
                if (!slide)
                {
                    Gravedad = 0;
                }
                Fase1 = 0;
                Fase2 = 0;
            }
        }
        else
        {
            Animator.SetBool("sky", true);
            if (!Saltando)
            {
                switch (Fase2)
                {
                    case 0:
                        if (!slide)
                        {
                            Gravedad = 0;
                        }
                        Fase2 = 1;
                        break;
                    case 1:
                        if (Gravedad > -10)
                        {
                            Gravedad -= AlturaSalto / Fallen * Time.deltaTime;
                        }
                        break;
                }
            }
        }

        if (!slide)
        {
            if (transform.position.y > Ypos && !airAttack)
            {
                Animator.SetFloat("gravedad", 1);
            }
            if (transform.position.y < Ypos && !airAttack)
            {
                Animator.SetFloat("gravedad", 0);
                switch (sky_)
                {
                    case 0:
                        Animator.Play("Base Layer.Sky", 0, 0);
                        sky_++;
                        break;
                }
            }
        }
        Ypos = transform.position.y;
    }

    public void Move()
    {
        bool isMoving = false;

        if (Input.GetKey(KeyCode.D) && !atacando)
        {
            if (CheckCollision)
            {
                if (!atacando && !accion)
                {
                    transform.Translate(Vector3.right * Velocidad * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    PlayMoveSound();
                    isMoving = true;
                }
            }
            else
            {
                transform.Translate(Vector3.right * Velocidad * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                PlayMoveSound();
                isMoving = true;
            }

            if (!slide)
            {
                Animator.SetBool("run", true);
            }
            else
            {
                Animator.SetBool("run", false);
            }
            distance2 = 0.55f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (CheckCollision)
            {
                if (!atacando && !accion)
                {
                    transform.Translate(Vector3.right * Velocidad * Time.deltaTime);
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    PlayMoveSound();
                    isMoving = true;
                }
            }
            else
            {
                transform.Translate(Vector3.right * Velocidad * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                PlayMoveSound();
                isMoving = true;
            }
            if (!slide)
            {
                Animator.SetBool("run", true);
            }
            else
            {
                Animator.SetBool("run", false);
            }
            distance2 = 0.55f;
        }
        else
        {
            Animator.SetBool("run", false);
            distance2 = 0;
        }

        if (!isMoving)
        {
            audioSourceMovimiento.Stop();
        }
    }

    private void PlayMoveSound()
    {
        if (!Saltando && CheckCollision)
        {
            if (!audioSourceMovimiento.isPlaying)
            {
                audioSourceMovimiento.clip = sonidoMovimiento;
                audioSourceMovimiento.Play();
            }
        }
        else
        {
            audioSourceMovimiento.Stop(); // Detener el sonido si no estás en el suelo o no estás tocando la plataforma
        }
    }

    public void WallJump()
    {
        if (CheckCollision2 && !CheckCollision)
        {
            if (transform.position.y < Ypos)
            {
                if (!Saltando)
                {
                    Gravedad = -2.5f;
                }

                slide = true;
                Animator.SetBool("slide", true);
            }
        }
        else
        {
            slide = false;
            slide = false;
            Animator.SetBool("slide", false);
        }

        if (delay > 0)
        {
            delay -= 4 * Time.deltaTime;
        }
    }
}