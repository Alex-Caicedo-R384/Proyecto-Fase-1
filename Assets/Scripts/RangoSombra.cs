using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoSombra : MonoBehaviour
{
    public Animator ani;
    public Sombras sombras;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemigo"))
        {
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            sombras.atacando = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
