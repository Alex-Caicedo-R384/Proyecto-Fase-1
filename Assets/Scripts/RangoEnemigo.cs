using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    public Animator aniE;
    public EnemigoBusca enemigo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Principal"))
        {
            aniE.SetBool("walk", false);
            aniE.SetBool("run", false);
            aniE.SetBool("attack", true);
            enemigo.atacandoE = true;
            GetComponent<BoxCollider2D>().enabled = false;        }
    }
}
