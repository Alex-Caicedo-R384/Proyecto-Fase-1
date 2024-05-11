using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestorEnemigos : MonoBehaviour
{
    public List<Enemigo> listaEnemigos; // Lista de todos los enemigos
    private int indiceActual = 0; // Índice del enemigo actual
    public AudioSource victoriaSound; // Sonido que se reproduce cuando todos los enemigos son derrotados
    private GameObject objetoPrincipal; // Objeto con el tag 'Principal'
    private Animator animatorPrincipal; // Animator del objeto principal

    void Start()
    {
        objetoPrincipal = GameObject.FindGameObjectWithTag("Principal"); // Busca el objeto con el tag 'Principal'
        if (objetoPrincipal != null) // Si el objeto existe...
        {
            animatorPrincipal = objetoPrincipal.GetComponent<Animator>(); // ...obtiene su componente Animator
        }
        DesactivarTodosEnemigos(); // Desactiva todos los enemigos al inicio
        IniciarEnemigo(); // Inicia el primer enemigo
    }

    void DesactivarTodosEnemigos()
    {
        foreach (var enemigo in listaEnemigos) // Para cada enemigo en la lista...
        {
            enemigo.gameObject.SetActive(false); // ...desactiva el enemigo
        }
    }

    public void IniciarEnemigo()
    {
        if (indiceActual < listaEnemigos.Count && !listaEnemigos[indiceActual].EstaDestruido()) // Si hay más enemigos y el enemigo actual no está destruido...
        {
            listaEnemigos[indiceActual].gameObject.SetActive(true); // ...activa el enemigo actual
        }
        else // Si no hay más enemigos...
        {
            Debug.Log("¡Todos los enemigos eliminados!"); // ...muestra un mensaje en la consola
            ReproducirSonidoVictoria(); // ...reproduce el sonido de victoria
            if (animatorPrincipal != null) // Si el objeto principal tiene un Animator...
            {
                animatorPrincipal.SetTrigger("Desaparecer"); // ...activa la animación 'Desaparecer'
            }
            StartCoroutine(ReiniciarEscenaConRetraso()); // ...y reinicia la escena después de un retraso
        }
    }

    public void SiguienteEnemigo()
    {
        if (indiceActual < listaEnemigos.Count && !listaEnemigos[indiceActual].EstaDestruido()) // Si hay más enemigos y el enemigo actual no está destruido...
        {
            listaEnemigos[indiceActual].gameObject.SetActive(false); // ...desactiva el enemigo actual
            indiceActual++; // ...y avanza al siguiente enemigo
            IniciarEnemigo(); // ...e inicia el siguiente enemigo
        }
    }

    public int ObtenerIndiceActual()
    {
        return indiceActual; // Devuelve el índice del enemigo actual
    }

    private void ReproducirSonidoVictoria()
    {
        if (victoriaSound != null) // Si hay un sonido de victoria...
        {
            victoriaSound.Play(); // ...reproduce el sonido de victoria
        }
    }

    private IEnumerator ReiniciarEscenaConRetraso()
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name); // Carga la escena actual de nuevo
    }
}
