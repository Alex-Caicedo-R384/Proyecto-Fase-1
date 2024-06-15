using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpearJugador : MonoBehaviour
{
    public int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            collision.GetComponent<Enemigo>().Vida -= damage;
            collision.GetComponent<Enemigo>().Blink(3f);
        }
    }
}
