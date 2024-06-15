using System.Collections;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int indiceEnLista;
    private bool destruido = false;
    public AudioClip sonidoParpadeo;
    public AudioClip sonidoDestruccion;
    public int Vida;

    private ShadowManager shadowManager;

    public float blinkTime;
    private SpriteRenderer spriteRenderer;

    public Color blinkColor1;
    public Color blinkColor2;

    private Color originalColor;

    private AudioSource audioSource;

    public bool EstaDestruido()
    {
        return destruido;
    }

    private void Start()
    {
        shadowManager = FindObjectOfType<ShadowManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Vida <= 0)
        {
            if (sonidoDestruccion != null)
            {
                AudioSource.PlayClipAtPoint(sonidoDestruccion, transform.position);
            }

            Destroy(gameObject);
            gameObject.SetActive(false);
            FindObjectOfType<GestorEnemigos>().SiguienteEnemigo();
            shadowManager.IncrementarSubditos();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Principal"))
        {
            if (coll.GetComponent<Movimiento>().VidaMin > 0)
            {
                coll.GetComponent<Movimiento>().Animator.SetTrigger("daño");
                coll.GetComponent<Movimiento>().Daño_ = true;

                if (transform.position.x > coll.transform.position.x)
                {
                    coll.GetComponent<Movimiento>().Empuje = -6;
                    coll.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    coll.GetComponent<Movimiento>().Empuje = 6;
                    coll.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                coll.GetComponent<Movimiento>().VidaMin -= 10;
            }
        }
    }

    public void Blink(float blinkDuration)
    {
        StartCoroutine(BlinkCoroutine(blinkDuration));
    }

    IEnumerator BlinkCoroutine(float blinkDuration)
    {
        float endTime = Time.time + blinkDuration;
        if (sonidoParpadeo != null)
        {
            audioSource.PlayOneShot(sonidoParpadeo);
        }
        while (Time.time < endTime)
        {
            float t = Mathf.PingPong(Time.time, blinkTime) / blinkTime;
            spriteRenderer.color = Color.Lerp(blinkColor1, blinkColor2, t);
            yield return null;
        }
        spriteRenderer.color = originalColor;
    }

    void OnDestroy()
    {
        if (shadowManager != null)
        {
        }
    }
}
