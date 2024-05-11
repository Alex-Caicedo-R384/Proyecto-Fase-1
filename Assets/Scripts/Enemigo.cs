using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int indiceEnLista; // Índice del enemigo en la lista de enemigos
    private bool destruido = false; // Indica si el enemigo ha sido destruido
    public AudioClip sonidoColision; // Sonido que se reproduce cuando el enemigo colisiona con el jugador
    public Renderer rend; // Renderer del enemigo

    private void Start()
    {
        rend = GetComponent<Renderer>(); // Obtiene el componente Renderer del enemigo
    }

    public bool EstaDestruido()
    {
        return destruido; // Devuelve si el enemigo ha sido destruido
    }

    private void Update()
    {
        // Aquí puedes agregar código que se ejecuta en cada frame
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Principal") && !destruido) // Si el enemigo colisiona con el jugador y no ha sido destruido...
        {
            Debug.Log("Colisión con jugador: " + this.name); // ...muestra un mensaje en la consola
            if (sonidoColision != null) // Si hay un sonido de colisión...
            {
                AudioSource.PlayClipAtPoint(sonidoColision, transform.position); // ...reproduce el sonido de colisión
            }

            GestorEnemigos gestor = FindObjectOfType<GestorEnemigos>(); // Busca el gestor de enemigos
            if (gestor != null && indiceEnLista == gestor.ObtenerIndiceActual()) // Si el gestor existe y este enemigo es el enemigo actual...
            {
                gestor.SiguienteEnemigo(); // ...pasa al siguiente enemigo
            }

            destruido = true; // Marca el enemigo como destruido
            Destroy(this.gameObject); // Destruye el objeto del enemigo
        }
    }
}
